using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Components
{
    [RequireComponent(typeof(MoveTowardsComponent))]
    public class AiMovementComponent : MonoBehaviour
    {
        public bool Active
        {
            set { GetComponent<MoveTowardsComponent>().MoveToPoint = value; }
        }

        [SerializeField]
        private float _distanceFromTargetToContinue;

        private int _currentTargetId = -1;
        private TrackTargetComponent _currentTarget;
        private MoveTowardsComponent _moveTowardsComponent;

        private void Start()
        {
            _moveTowardsComponent = GetComponent<MoveTowardsComponent>();
            _moveTowardsComponent.TargetReachedEvent += OnReachedTarget;
            _moveTowardsComponent.DistanceFromPointToStop = _distanceFromTargetToContinue;

            GetNextTarget();
        }

        private void OnReachedTarget()
        {
            GetNextTarget();
        }

        private void GetNextTarget()
        {
            _currentTarget = PathTargetManager.Instance.GetNextTarget(_currentTargetId);
            _currentTargetId = _currentTarget.TargetId;
        }

        private void Update()
        {
            _moveTowardsComponent.PositionToMoveTowards =
                _currentTarget.transform.position + _currentTarget.TargetOffset;
        }
    }
}
