using UnityEngine;

namespace Assets.Scripts.Components
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MoveComponent : MonoBehaviour
    {
        public float ForwardSpeed;

        [SerializeField]
        private float _reverseSpeed;

        [SerializeField]
        private float _leftRightSpeed;

        private Rigidbody2D _rigidBody;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
        }

        public void MoveForward(float deltaTime)
        {
            _rigidBody.AddForce(transform.up * ForwardSpeed * deltaTime, ForceMode2D.Impulse);
        }

        public void MoveBack(float deltaTime)
        {
            _rigidBody.AddForce(-transform.up * _reverseSpeed * deltaTime, ForceMode2D.Impulse);
        }

        public void TurnLeft(float deltaTime)
        {
            _rigidBody.AddTorque(_leftRightSpeed * deltaTime, ForceMode2D.Impulse);
        }

        public void TurnRight(float deltaTime)
        {
            _rigidBody.AddTorque(-_leftRightSpeed * deltaTime, ForceMode2D.Impulse);
        }
    }
}
