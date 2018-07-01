using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Components;
using Assets.Scripts.Constants;
using Assets.Scripts.Models;
using Assets.Scripts.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{
    public class RaceManager : MonoBehaviour
    {
        public Text CenterScreenText;
        public Text LapText;
        public Text PositionText;

        [SerializeField] private float EnemySlowAmount;
        [SerializeField] private float EnemySpeedAmount;

        [SerializeField] private float MinEnemySpeed;
        [SerializeField] private float MaxEnemySpeed;
        
        private const int EffortPerRace = 100;

        private List<EnemyCar> _enemyCars;
        private Player _player;
        private TimeManager _timeManager;
        private bool _raceEnd;
        private int _playerCurrentPosition;
        private int _laps;

        private List<LapTrackingComponent> LapTrackers()
        {
            var lapTrackers = _enemyCars.Select(e => e.LapTrackingComponent).ToList();
            lapTrackers.Add(_player.LapTrackingComponent);
            return lapTrackers;
        }
        
        public void Initialize()
        {
            GetEnemyCars();
            GetPlayer();

            SubscribeToDeathEvents();
            SubscribeToLapEvents();
            StartCoroutine(StartRace());
            StartCoroutine(VaryEnemySpeed());

            CalculateLaps(FindObjectsOfType<TrackTargetComponent>().Length);

            _timeManager = GetComponent<TimeManager>();
        }

        private IEnumerator VaryEnemySpeed()
        {
            yield return new WaitForSeconds(2);
            ChangeEnemySpeed(_playerCurrentPosition > 1 ? EnemySlowAmount : EnemySpeedAmount);
            StartCoroutine(VaryEnemySpeed());
        }

        private void ChangeEnemySpeed(float change)
        {
            var currentSpeed = _enemyCars.First().MoveComponent.ForwardSpeed;
            if (currentSpeed + change < MinEnemySpeed || currentSpeed + change > MaxEnemySpeed)
            {
                return;
            }
            _enemyCars.ForEach(e => e.MoveComponent.ForwardSpeed = e.MoveComponent.ForwardSpeed + change);
        }

        private void GetPlayer()
        {
            var player = GameObject.FindGameObjectWithTag(Tag.Player);
            _player = new Player
            {
                GameObject = player.gameObject,
                HealthComponent = player.GetComponent<HealthComponent>(),
                KeyboardInputComponent = player.GetComponent<KeyboardInputComponent>(),
                LapTrackingComponent = player.GetComponent<LapTrackingComponent>()
            };
        }

        private void GetEnemyCars()
        {
            var enemyCars = GameObject.FindGameObjectsWithTag(Tag.Enemy);
            _enemyCars = enemyCars.Select(e => new EnemyCar
            {
                GameObject = e.gameObject,
                LapTrackingComponent = e.GetComponent<LapTrackingComponent>(),
                HealthComponent = e.GetComponent<HealthComponent>(),
                AiMovementComponent = e.GetComponent<AiMovementComponent>(),
                MoveComponent = e.GetComponent<MoveComponent>()
            }).ToList();
        }

        private void SubscribeToLapEvents()
        {
            foreach (var lapTrackingComponent in _enemyCars.Select(e => e.LapTrackingComponent))
            {
                lapTrackingComponent.LapEvent += CarLapped;
            }

            _player.LapTrackingComponent.LapEvent += CarLapped;
        }

        private void CarLapped(LapTrackingComponent lapTrackingComponent)
        {
            if (lapTrackingComponent.gameObject == _player.GameObject)
            {
                if (lapTrackingComponent.Laps == _laps - 1)
                {
                    HandlePlayerDied();
                }
            }
        }

        private void SubscribeToDeathEvents()
        {
            foreach (var enemyCar in _enemyCars)
            {
                enemyCar.HealthComponent.DeathEvent += EnemyDied;
            }

            _player.HealthComponent.DeathEvent += PlayerDied;
        }

        private void PlayerDied(HealthComponent healthComponent)
        {
            HandlePlayerDied();
            Destroy(healthComponent.gameObject);
        }

        private void EnemyDied(HealthComponent healthComponent)
        {
            HandleAiDied(healthComponent.gameObject);
            Destroy(healthComponent.gameObject);
        }

        private void HandleAiDied(GameObject gameObj)
        {
            _enemyCars = _enemyCars.Where(e => e.GameObject != gameObj).ToList();
        }

        private void Update()
        {
            if (_raceEnd)
            {
                return;
            }

            UpdateLapCount();
            UpdatePosition();
        }

        public void Restart()
        {
            SceneManager.LoadScene(0);
        }

        private void UpdatePosition()
        {
            _playerCurrentPosition = GetPosition(_player.LapTrackingComponent);

            PositionText.text = StringUtility.GetNumberWithSuffix(_playerCurrentPosition);
        }

        private void UpdateLapCount()
        {
            LapText.text = UiMessages.Lap + " " + (_player.LapTrackingComponent.Laps + 1) + " / " + _laps;
        }

        private IEnumerator StartRace()
        {
            SetCarsActive(false);

            LapText.text = string.Empty;
            PositionText.text = string.Empty;

            CenterScreenText.text = "3";
            yield return new WaitForSeconds(1);
            CenterScreenText.text = "2";
            yield return new WaitForSeconds(1);
            CenterScreenText.text = "1";
            yield return new WaitForSeconds(1);
            CenterScreenText.text = UiMessages.Go;

            SetCarsActive(true);

            yield return new WaitForSeconds(1);
            CenterScreenText.text = string.Empty;
        }

        private void SetCarsActive(bool active)
        {
            foreach (var enemyCar in _enemyCars)
            {
                enemyCar.AiMovementComponent.Active = active;
            }

            _player.KeyboardInputComponent.enabled = active;
        }

        private void HandlePlayerDied()
        {
            _timeManager.TimeScale = 0;

            CenterScreenText.fontSize = 50;
            CenterScreenText.text = 
                UiMessages.YouFinished + " " + StringUtility.GetNumberWithSuffix(_playerCurrentPosition);

            _raceEnd = true;
        }

        public int GetPosition(LapTrackingComponent lapTrackingComponent)
        {
            var carsOnLapAbove = LapTrackers()
                .Where(l => l.Laps > lapTrackingComponent.Laps).ToList();

            var carsWithHigherProgress = LapTrackers()
                .Where(l => l.Laps == lapTrackingComponent.Laps)
                .Where(l => l.ProgressOnCurrentLap > lapTrackingComponent.ProgressOnCurrentLap);

            var carsCloserToNextTarget = LapTrackers()
                .Where(l => l.Laps == lapTrackingComponent.Laps)
                .Where(l => l.ProgressOnCurrentLap == lapTrackingComponent.ProgressOnCurrentLap)
                .Where(l => l.DistanceToNextTarget < lapTrackingComponent.DistanceToNextTarget);

            return carsOnLapAbove.Concat(carsWithHigherProgress).Concat(carsCloserToNextTarget).Distinct().Count() + 1;
        }

        private void CalculateLaps(int trackParts)
        {
            _laps = EffortPerRace / trackParts;
        }
    }
}
