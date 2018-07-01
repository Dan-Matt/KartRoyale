using Assets.Scripts.Constants;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class CameraFollowComponent : MonoBehaviour
    {
        [SerializeField]
        private float _followSpeed;

        private Transform _target;

        private void Start()
        {
            _target = GameObject.FindGameObjectWithTag(Tag.Player).transform;

            transform.position = new Vector3(
                _target.position.x, _target.position.y, transform.position.z);
        }

        private void Update()
        {
            if (_target == null)
            {
                return;
            }

            var targetPos = new Vector3(
                _target.position.x, _target.position.y, transform.position.z);

            transform.position = Vector3.Slerp(
                transform.position, targetPos, _followSpeed * Time.deltaTime);
        }
    }
}
