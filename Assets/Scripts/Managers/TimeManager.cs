using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField]
        private float _timeScale;

        public float TimeScale
        {
            get { return _timeScale; }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                if (value > 100)
                {
                    value = 100;
                }

                _timeScale = value;
                UpdateTimeScale();
            }
        }

        private void UpdateTimeScale()
        {
            Time.timeScale = _timeScale;
        }

        private void Start()
        {
            Time.timeScale = _timeScale;
        }
    }
}
