using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.Managers
{
    public class RandomTrackManager
    {
        public int StartingStripLength = 4;
        public List<Track> MapPoints = new List<Track>();
        public bool StartsHorizontal;

        private int _mapSize;

        public List<TrackModel> GetBlueprint(int mapSize)
        {
            _mapSize = mapSize;

            var seed = (int)DateTime.Now.Ticks;
            Debug.Log(seed);
            var random = new Random(seed);
            StartsHorizontal = Convert.ToBoolean(random.Next(2));

            var valid = false;
            while (!valid)
            {
                MapPoints.Clear();
                PlaceStart(random);
                valid = PlaceTrack(random);
            }
            return ToTrackModels(MapPoints);
        }

        private List<TrackModel> ToTrackModels(List<Track> mapPoints)
        {
            foreach (var mapPoint in mapPoints)
            {
                mapPoint.Id -= StartingStripLength - 1;
                if (mapPoint.Id < 0)
                {
                    mapPoint.Id = mapPoints.Count + mapPoint.Id;
                }
            }
            mapPoints = mapPoints.OrderBy(m => m.Id).ToList();

            var trackModels = new List<TrackModel>();

            foreach (var mapPoint in mapPoints)
            {
                if (mapPoint.Id == 0)
                {
                    trackModels.Add(new TrackModel(
                        StartsHorizontal ? TrackEnum.HorizontalStart : TrackEnum.VerticalStart,
                        TrackDirection.None));

                    continue;
                }

                var previous = mapPoints.FirstOrDefault(m => m.Id == mapPoint.Id - 1) ?? mapPoints.First();
                var next = mapPoints.FirstOrDefault(m => m.Id == mapPoint.Id + 1) ?? mapPoints.First();

                if (previous.X < mapPoint.X && next.X > mapPoint.X)
                {
                    trackModels.Add(new TrackModel(TrackEnum.Horizontal, TrackDirection.Right));
                }
                else if (previous.X > mapPoint.X && next.X < mapPoint.X)
                {
                    trackModels.Add(new TrackModel(TrackEnum.Horizontal, TrackDirection.Left));
                }
                else if (previous.Y < mapPoint.Y && next.Y > mapPoint.Y)
                {
                    trackModels.Add(new TrackModel(TrackEnum.Vertical, TrackDirection.Below));
                }
                else if (previous.Y > mapPoint.Y && next.Y < mapPoint.Y)
                {
                    trackModels.Add(new TrackModel(TrackEnum.Vertical, TrackDirection.Above));
                }
                else if (previous.X > mapPoint.X && next.Y > mapPoint.Y)
                {
                    trackModels.Add(new TrackModel(TrackEnum.TopLeft, TrackDirection.Left));
                }
                else if (previous.Y > mapPoint.Y && next.X > mapPoint.X)
                {
                    trackModels.Add(new TrackModel(TrackEnum.TopLeft, TrackDirection.Above));
                }
                else if (previous.X < mapPoint.X && next.Y > mapPoint.Y)
                {
                    trackModels.Add(new TrackModel(TrackEnum.TopRight, TrackDirection.Right));
                }
                else if (previous.Y > mapPoint.Y && next.X < mapPoint.X)
                {
                    trackModels.Add(new TrackModel(TrackEnum.TopRight, TrackDirection.Above));
                }
                else if (previous.Y < mapPoint.Y && next.X > mapPoint.X)
                {
                    trackModels.Add(new TrackModel(TrackEnum.BottomLeft, TrackDirection.Below));
                }
                else if (previous.X > mapPoint.X && next.Y < mapPoint.Y)
                {
                    trackModels.Add(new TrackModel(TrackEnum.BottomLeft, TrackDirection.Left));
                }
                else if (previous.X < mapPoint.X && next.Y < mapPoint.Y)
                {
                    trackModels.Add(new TrackModel(TrackEnum.BottomRight, TrackDirection.Right));
                }
                else if (previous.Y < mapPoint.Y && next.X < mapPoint.X)
                {
                    trackModels.Add(new TrackModel(TrackEnum.BottomRight, TrackDirection.Below));
                }
            }
            return trackModels;
        }

        public bool PlaceTrack(Random random)
        {
            while (true)
            {
                var currentPosition = MapPoints.Last();
                var validPointsAroundCurrent = ValidPointsAround(random, currentPosition);
                if (!validPointsAroundCurrent.Any())
                {
                    return false;
                }

                if (validPointsAroundCurrent.Any(p => p.Id == 1))
                {
                    break;
                }

                MapPoints.Add(validPointsAroundCurrent[random.Next(0, validPointsAroundCurrent.Count)]);
            }
            return true;
        }

        public List<Track> PointsAround(Track currentPoint)
        {
            return new List<Track>
                {
                    new Track(0, currentPoint.X-1, currentPoint.Y),
                    new Track(0, currentPoint.X, currentPoint.Y-1),
                    new Track(0, currentPoint.X+1, currentPoint.Y),
                    new Track(0, currentPoint.X, currentPoint.Y+1)
                }
                .Where(t => t.X >= 0 && t.Y >= 0 && t.X < _mapSize && t.Y < _mapSize).ToList();
        }

        public List<Track> ValidPointsAround(Random random, Track currentPoint)
        {
            var pointsAround = PointsAround(currentPoint);

            var validPoints = new List<Track>();
            foreach (var point in pointsAround)
            {
                var existingPoint = ExistingPoint(point);
                if (existingPoint != null && existingPoint.Id == 1)
                {
                    return new List<Track> { new Track(1, point.X, point.Y) };
                }
                if (existingPoint == null)
                {
                    var pointValid = true;
                    foreach (var innerPoint in PointsAround(point))
                    {
                        var existPoint = ExistingPoint(innerPoint);
                        if (existPoint != null && existPoint.Id != MapPoints.Count && existPoint.Id != 1)
                        {
                            pointValid = false;
                        }
                    }
                    if (pointValid)
                        validPoints.Add(new Track(MapPoints.Count + 1, point.X, point.Y));
                }
            }
            return validPoints;
        }

        public Track ExistingPoint(Track point)
        {
            return MapPoints.FirstOrDefault(m => m.X == point.X && m.Y == point.Y);
        }

        public void PlaceStart(Random random)
        {
            var xPos = 0;
            var yPos = 0;

            if (StartsHorizontal)
            {
                xPos = random.Next(0, _mapSize - StartingStripLength);
                yPos = random.Next(0, _mapSize);
            }
            else
            {
                xPos = random.Next(0, _mapSize);
                yPos = random.Next(0 + StartingStripLength, _mapSize);
            }

            for (int i = 1; i <= StartingStripLength; i++)
            {
                MapPoints.Add(new Track(i, xPos, yPos));

                if (StartsHorizontal)
                    xPos++;
                else
                    yPos--;
            }
        }

        public class Track
        {
            public Track(int id, int x, int y)
            {
                Id = id;
                X = x;
                Y = y;
            }
            public int Id;
            public int X;
            public int Y;
        }
    }
}
