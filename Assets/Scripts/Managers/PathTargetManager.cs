using System;
using System.Linq;
using Assets.Scripts.Components;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class PathTargetManager : MonoSingleton<PathTargetManager>
    {
        private TrackTargetComponent[] _targets;

        public TrackTargetComponent GetNextTarget(int currentTarget)
        {
            if (currentTarget == _targets.Length - 1)
            {
                return _targets[0];
            }
            return _targets[++currentTarget];
        }
        
        public int TargetCount()
        {
            return _targets.Length;
        }

        public void Initialize()
        {
            GetAllTargets();
        }

        private void GetAllTargets()
        {
            _targets = FindObjectsOfType<TrackTargetComponent>().OrderBy(t => t.TargetId).ToArray();
        }

        private void OnDrawGizmos()
        {
            try
            {
                foreach (var target in _targets)
                {
                    Gizmos.DrawWireSphere(target.transform.position + target.TargetOffset, 2.5f);
                }
            }
            catch
            {
            }
        }
    }
}
