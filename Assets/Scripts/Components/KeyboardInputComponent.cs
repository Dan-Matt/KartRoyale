using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Components
{
    [RequireComponent(typeof(MoveComponent))]
    public class KeyboardInputComponent : MonoBehaviour
    {
        private MoveComponent _moveComponent;
        private bool _upPressed;
        private bool _downPressed;
        private bool _leftPressed;
        private bool _rightPressed;

        private void Awake()
        {
            _moveComponent = GetComponent<MoveComponent>();
        }

        private void Update()
        {
            _upPressed = false;
            _downPressed = false;
            _leftPressed = false;
            _rightPressed = false;

            if (InputManager.UpPressed())
            {
                _upPressed = true;
            }
            if (InputManager.DownPressed())
            {
                _downPressed = true;
            }
            if (InputManager.LeftPressed())
            {
                _leftPressed = true;
            }
            if (InputManager.RightPressed())
            {
                _rightPressed = true;
            }
        }

        private void FixedUpdate()
        {
            if (_upPressed)
            {
                _moveComponent.MoveForward(Time.fixedDeltaTime);
            }
            if (_downPressed)
            {
                _moveComponent.MoveBack(Time.fixedDeltaTime);
            }
            if (_leftPressed)
            {
                _moveComponent.TurnLeft(Time.fixedDeltaTime);
            }
            if (_rightPressed)
            {
                _moveComponent.TurnRight(Time.fixedDeltaTime);
            }
        }
    }
}
