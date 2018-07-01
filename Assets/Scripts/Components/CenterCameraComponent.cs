using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class CenterCameraComponent : MonoBehaviour
    {
        private const int GridSize = 6;

        private void Start()
        {
            var trackPieces = FindObjectsOfType<TrackTargetComponent>();

            var min = new Vector2(
                trackPieces.Min(t => t.transform.position.x),
                trackPieces.Min(t => t.transform.position.y) - GridSize);

            var max = new Vector2(
                trackPieces.Max(t => t.transform.position.x) + GridSize,
                trackPieces.Max(t => t.transform.position.y));

            var middle = (min + max) / 2;
            transform.position = new Vector3(middle.x, middle.y, -10);
        }
    }
}
