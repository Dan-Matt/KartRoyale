using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Managers
{
    public class InputManager : MonoSingleton<InputManager>
    {
        public static bool OverUi()
        {
            return FindObjectOfType<EventSystem>().IsPointerOverGameObject(0);
        }

        public static bool ScreenReleased()
        {
            return Input.GetMouseButtonUp(0);
        }

        public static bool ScreenPressed()
        {
            return Input.GetMouseButtonDown(0);
        }

        public static bool ScreenHeld()
        {
            return Input.GetMouseButton(0);
        }

        public static Vector3 ScreenPointPosition()
        {
            return Input.mousePosition;
        }
        
        public static Vector3 ScreenPointPosition(int zOffset)
        {
            var pos = Input.mousePosition;
            pos.z = zOffset;
            return pos;
        }

        public static DeviceOrientation GetOrientation()
        {
            return Input.deviceOrientation;
        }

        public static bool LeftPressed()
        {
            return Input.GetKey(KeyCode.LeftArrow);
        }
        public static bool RightPressed()
        {
            return Input.GetKey(KeyCode.RightArrow);
        }
        public static bool UpPressed()
        {
            return Input.GetKey(KeyCode.UpArrow);
        }
        public static bool DownPressed()
        {
            return Input.GetKey(KeyCode.DownArrow);
        }

        public static bool SpacePressed()
        {
            return Input.GetKey(KeyCode.Space);
        }

        public static bool RPressed()
        {
            return Input.GetKey(KeyCode.R);
        }

        public static bool OnePressedDown()
        {
            return Input.GetKeyDown(KeyCode.Alpha1);
        }

        public static bool TwoPressedDown()
        {
            return Input.GetKeyDown(KeyCode.Alpha2);
        }

        public static bool ThreePressedDown()
        {
            return Input.GetKeyDown(KeyCode.Alpha3);
        }

        public static bool HashPressedDown()
        {
            return Input.GetKeyDown(KeyCode.Hash);
        }
    }
}
