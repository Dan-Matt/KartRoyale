using Assets.Scripts.Enums;

namespace Assets.Scripts.Models
{
    public class TrackModel
    {
        public TrackModel(TrackEnum trackType, TrackDirection trackDirection)
        {
            TrackType = trackType;
            TrackDirection = trackDirection;
        }

        public TrackEnum TrackType;
        public TrackDirection TrackDirection;
    }
}
