using UnityEngine;

namespace Assets.Scripts.Managers
{
    [RequireComponent(typeof(TimeManager))]
    public class DebugManager : MonoBehaviour
    {
        public bool ShowGizmos;

        private TimeManager _timeManager;
        private bool _paused;
        private float _previousTimeScale;
        
        private void Start()
        {
            _timeManager = GetComponent<TimeManager>();
        }

        private void Update()
        {
            HandleKeyPresses();
        }

        private void HandleKeyPresses()
        {
            if (InputManager.OnePressedDown())
            {
                _timeManager.TimeScale -= 0.1f;
            }
            if (InputManager.TwoPressedDown())
            {
                _timeManager.TimeScale += 0.1f;
            }
            if (InputManager.ThreePressedDown())
            {
                if (_paused)
                {
                    _timeManager.TimeScale = _previousTimeScale;
                }
                else
                {
                    _previousTimeScale = _timeManager.TimeScale;
                    _timeManager.TimeScale = 0;
                }
                _paused = !_paused;
            }
            if (InputManager.HashPressedDown())
            {
                ShowGizmos = !ShowGizmos;
            }
        }
    }
}
