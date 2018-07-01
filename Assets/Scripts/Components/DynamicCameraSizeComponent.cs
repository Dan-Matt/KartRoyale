using UnityEngine;

namespace Assets.Scripts.Components
{
    public class DynamicCameraSizeComponent : MonoBehaviour
    {
        [SerializeField]
        private int _landscapeSize;

        [SerializeField]
        private int _portraitSize;

        private Camera _camera;

        private void Start()
        {
            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            if (Screen.width > Screen.height)
            {
                _camera.orthographicSize = _landscapeSize;
                return;
            }
            _camera.orthographicSize = _portraitSize;
        }
    }
}
