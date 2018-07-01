using System;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class Sound
    {
        public string Name;
        public AudioClip AudioClip;

        [Range(0f, 1f)]
        public float Volume;

        [Range(.1f, 3f)]
        public float Pitch;

        public bool Loop;

        [Range(0f, 1f)]
        public float SpatialBlend;

        public float MaxDistance;

        [HideInInspector]
        public AudioSource AudioSource;

    }
}
