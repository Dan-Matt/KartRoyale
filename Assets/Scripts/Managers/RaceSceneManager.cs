using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class RaceSceneManager : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<PlaceTrackManager>().Initialize();
            GetComponent<RaceManager>().Initialize();
            GetComponent<PathTargetManager>().Initialize();
        }
    }
}
