using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Constants;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class LapTrackingComponent : MonoBehaviour
    {
        public Action<LapTrackingComponent> LapEvent;

        public int Laps;
        public List<int> TargetsHit = new List<int>();
        
        public int ProgressOnCurrentLap
        {
            get { return Enumerable.Range(0, TargetsHit.Count).SequenceEqual(TargetsHit) ? TargetsHit.Count : 0; }
        }

        public float DistanceToNextTarget
        {
            get
            {
                var currentTarget = TargetsHit.Count > 0 ? TargetsHit.Max(t => t) : 0;
                var nextTarget = PathTargetManager.Instance.GetNextTarget(currentTarget);
                
                return Vector3.Distance(transform.position, 
                    nextTarget.transform.position + nextTarget.GetComponent<TrackTargetComponent>().TargetOffset);
            }
        }
        
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.tag != Tag.Track)
            {
                return;
            }

            var targetComponent = other.gameObject.GetComponent<TrackTargetComponent>();
            if (!TargetsHit.Contains(targetComponent.TargetId))
            {
                TargetsHit.Add(targetComponent.TargetId);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag != Tag.Finish)
            {
                return;
            }

            if (AllTargetsHit())
            {
                if (LapEvent != null)
                {
                    LapEvent(this);
                }
                Laps++;
            }

            TargetsHit.Clear();
        }

        private bool AllTargetsHit()
        {
            return TargetsHit.Count == PathTargetManager.Instance.TargetCount() &&
                   TargetsHit.OrderBy(t => t).SequenceEqual(TargetsHit);
        }
    }
}
