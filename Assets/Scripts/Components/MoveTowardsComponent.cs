using System;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Components
{
    [RequireComponent(typeof(MoveComponent))]
    public class MoveTowardsComponent : MonoBehaviour
    {
        public event Action TargetReachedEvent;

        public float DistanceFromPointToStop;
        public Vector3 PositionToMoveTowards;
        public bool MoveToPoint;

        private MoveComponent _moveComponent;

        private void Awake()
        {
            _moveComponent = GetComponent<MoveComponent>();
        }

        private void FixedUpdate()
        {
            if (MoveToPoint && !ReachedTarget())
            {
                MoveToPosition(PositionToMoveTowards);
            }
        }

        private void MoveToPosition(Vector3 position)
        {
            var targetDirection = VectorUtilities.Direction(transform.position, position);

            var angleDirection = AngleUtilities.AngleDirection(transform.forward, targetDirection, transform.up);

            var isBehind = AngleUtilities.AngleIsBehind(transform.forward, targetDirection);

            if (isBehind && angleDirection == 0)
            {
                _moveComponent.MoveBack(Time.fixedDeltaTime);
                return;
            }

            _moveComponent.MoveForward(Time.fixedDeltaTime);

            switch (angleDirection)
            {
                case -1:
                    _moveComponent.TurnLeft(Time.fixedDeltaTime);
                    break;
                case 1:
                    _moveComponent.TurnRight(Time.fixedDeltaTime);
                    break;
                case 0:
                    break;
            }
        }

        private bool ReachedTarget()
        {
            if (Vector3.Distance(transform.position, PositionToMoveTowards) >= DistanceFromPointToStop)
            {
                return false;
            }
            
            if (TargetReachedEvent != null)
            {
                TargetReachedEvent();
            }

            return true;
        }
    }
}
