using System.Collections.Generic;
using UnityEngine;

namespace FallingSloth.Audio
{
    /// <summary>
    /// A singleton class that manages multiple sounds, incuding music and sound effects.
    /// </summary>
    [AddComponentMenu("Falling Sloth/Audio Manager")]
    public class AudioManager : SingletonBehaviour<AudioManager>
    {
        /// <summary>
        /// The list of sounds to manage.  Each should have a unique name.
        /// </summary>
        public List<Sound> sounds;

        [Range(0f, 1f)]
        public float globalVolume = 1f;

        /// <summary>
        /// Sets up individual AudioSource components for each sound on this GameObject, and plays any sounds set to play on awake.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            if (Instance != null && Instance != this)
                return;
            
            foreach (Sound sound in sounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.audioClip;
                sound.source.volume = sound.volume * globalVolume;
                sound.source.pitch = sound.pitch;
                sound.source.playOnAwake = sound.playOnAwake;
                sound.source.loop = sound.loop;

                if (sound.playOnAwake)
                    sound.source.Play();
            }
        }

        /// <summary>
        /// Plays the sound with the given name.  If there is no sound with the given name, an exception is thrown.
        /// </summary>
        /// <param name="name">The name of the sound to play.</param>
        public static void PlaySound(string name)
        {
            foreach (Sound sound in Instance.sounds)
            {
                if (sound.name.ToLower() == name.ToLower())
                {
                    sound.source.Play();
                    return;
                }
            }

            throw new System.ArgumentOutOfRangeException("name", "No sound with the given name found.");
        }

        /// <summary>
        /// Stops the sound with the given name.  If there is no sound with the given name, an exception is thrown.
        /// </summary>
        /// <param name="name">The name of the sound to stop.</param>
        public static void StopSound(string name)
        {
            foreach (Sound sound in Instance.sounds)
            {
                if (sound.name.ToLower() == name.ToLower())
                {
                    sound.source.Stop();
                    return;
                }
            }

            throw new System.ArgumentOutOfRangeException("name", "No sound with the given name found.");
        }

        public void ChangeGlobalVolume(float volume)
        {
            globalVolume = volume;

            foreach (Sound sound in sounds)
            {
                sound.source.volume = sound.volume * globalVolume;
            }
        }
    }
}