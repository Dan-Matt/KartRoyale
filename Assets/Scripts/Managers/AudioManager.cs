using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class AudioManager : MonoBehaviour {

        public GameObject Target;
        public List<Sound> Sounds;

        public void LoadSounds()
        {
            foreach (var sound in Sounds)
            {
                sound.AudioSource = gameObject.AddComponent<AudioSource>();
                sound.AudioSource.clip = sound.AudioClip;
                sound.AudioSource.volume = sound.Volume;
                sound.AudioSource.pitch = sound.Pitch;
                sound.AudioSource.loop = sound.Loop;
                sound.AudioSource.spatialBlend = sound.SpatialBlend;
                sound.AudioSource.rolloffMode = AudioRolloffMode.Linear;
                sound.AudioSource.maxDistance = sound.MaxDistance;
            }
        }

        public void PlaySound(string soundName)
        {
            var sound = Sounds.FirstOrDefault(s => s.Name == soundName);
            if (sound == null)
            {
                Debug.Log(string.Format("'{0}' sound not found", soundName));
                return;
            }

            sound.AudioSource.transform.position = Target.transform.position;
            sound.AudioSource.PlayOneShot(sound.AudioClip);
        }

        public bool IsSoundPlaying(List<string> soundNames)
        {
            var sounds = Sounds.Where(s => soundNames.Contains(s.Name));
            return sounds.Any(s => s.AudioSource.isPlaying);
        }

        public void PlayMusic(string soundName)
        {
            var sound = Sounds.First(s => s.Name == soundName);
            sound.AudioSource.transform.position = Target.transform.position;
            sound.AudioSource.Play();
        }

        public void PauseMusic(string soundName)
        {
            var sound = Sounds.First(s => s.Name == soundName);
            sound.AudioSource.Pause();
        }

        private void Awake()
        {
            if (Target == null)
            {
                Target = Camera.main.gameObject;
            }

            LoadSounds();
            PlayMusic("Soundtrack");
        }
    }
}
