using Assets.Scripts.Components;
using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class PlaceTrackManager : MonoBehaviour
    {
        public Transform MapParent;
        public Transform CarParent;

        public GameObject Horizontal;
        public GameObject HorizontalStart;
        public GameObject Vertical;
        public GameObject VerticalStart;
        public GameObject TopLeft;
        public GameObject TopRight;
        public GameObject BottomLeft;
        public GameObject BottomRight;

        public GameObject Player;
        public GameObject Enemy;

        public int MapSize;

        private float GridSpacing = 6.4f;
        private GameObject _startTrackPiece;
        private RandomTrackManager _randomTrackManager;

        private const int CarsPerLine = 5;
        private const int PlayerPosition = 2;
        private const float SpacingMultiplier = 1.5f;
        private Vector3 _horizontalStartingOffset = new Vector3(-4.4f, -4.5f, 0);
        private Vector3 _verticalStartingOffset = new Vector3(0.5f, -9.5f, 0);

        public void Initialize()
        {
            _randomTrackManager = new RandomTrackManager();

            PlaceTrack();
            PlacePlayers();
        }

        private void PlacePlayers()
        {
            var startHorizontal = _startTrackPiece == Horizontal;

            var startPos = _startTrackPiece.transform.position + 
                (startHorizontal ? _horizontalStartingOffset : _verticalStartingOffset);

            for (var x = 0; x < CarsPerLine; x++)
            {
                for (var y = 0; y < CarsPerLine; y++)
                {
                    var car = Enemy;

                    if (x == PlayerPosition && y == PlayerPosition)
                    {
                        car = Player;
                    }

                    Instantiate(car, startPos + 
                        (startHorizontal ? new Vector3(y * SpacingMultiplier, x) : new Vector3(x, y * SpacingMultiplier)), 
                        (startHorizontal ? Quaternion.AngleAxis(-90, Vector3.forward) : Quaternion.identity), 
                        CarParent);
                }
            }
        }

        private void PlaceTrack()
        {
            var currentPosition = Vector3.zero;
            var currentTrackId = 0;
            var blueprint = _randomTrackManager.GetBlueprint(MapSize);
            
            foreach (var track in blueprint)
            {
                switch (track.TrackDirection)
                {
                    case TrackDirection.None:
                        currentPosition += Vector3.zero;
                        break;
                    case TrackDirection.Above:
                        currentPosition += new Vector3(0, GridSpacing, 0);
                        break;
                    case TrackDirection.Below:
                        currentPosition += new Vector3(0, -GridSpacing, 0);
                        break;
                    case TrackDirection.Left:
                        currentPosition += new Vector3(-GridSpacing, 0, 0);
                        break;
                    case TrackDirection.Right:
                        currentPosition += new Vector3(GridSpacing, 0, 0);
                        break;
                }

                GameObject trackPiece = Horizontal;

                switch (track.TrackType)
                {
                    case TrackEnum.TopLeft:
                        trackPiece = TopLeft;
                        break;
                    case TrackEnum.TopRight:
                        trackPiece = TopRight;
                        break;
                    case TrackEnum.BottomLeft:
                        trackPiece = BottomLeft;
                        break;
                    case TrackEnum.BottomRight:
                        trackPiece = BottomRight;
                        break;
                    case TrackEnum.Horizontal:
                        trackPiece = Horizontal;
                        break;
                    case TrackEnum.Vertical:
                        trackPiece = Vertical;
                        break;
                    case TrackEnum.HorizontalStart:
                        trackPiece = Horizontal;
                        _startTrackPiece = trackPiece;
                        Instantiate(HorizontalStart, currentPosition, HorizontalStart.transform.rotation, MapParent);
                        break;
                    case TrackEnum.VerticalStart:
                        trackPiece = Vertical;
                        _startTrackPiece = trackPiece;
                        Instantiate(VerticalStart, currentPosition, VerticalStart.transform.rotation, MapParent);
                        break;
                }

                var obj = Instantiate(trackPiece, currentPosition, trackPiece.transform.rotation, MapParent);
                obj.GetComponent<TrackTargetComponent>().TargetId = currentTrackId;
                currentTrackId++;
            }
        }
    }
}
