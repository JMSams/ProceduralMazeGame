using UnityEngine;
using UnityEngine.Audio;

namespace FallingSloth.Audio
{
    /// <summary>
    /// Holds information about a sound for use in the <see cref="AudioManager"/> class.
    /// </summary>
    [System.Serializable]
    public class Sound
    {
        /// <summary>
        /// The name of the sound.
        /// </summary>
        public string name;

        /// <summary>
        /// The AudioClip that this sound plays.
        /// </summary>
        public AudioClip audioClip;

        /// <summary>
        /// Whether the sound should be played automatically at start.
        /// </summary>
        public bool playOnAwake;

        /// <summary>
        /// Whether the sound should loop when played.
        /// </summary>
        public bool loop;

        /// <summary>
        /// The volume of the sound (0.0 - 1.0).
        /// </summary>
        [Range(0f, 1f)]
        public float volume = 1f;

        /// <summary>
        /// The pitch of the sound (0.1 - 3.0).
        /// </summary>
        [Range(.1f, 3f)]
        public float pitch = 1f;
        
        /// <summary>
        /// The audio source used to play this sound.
        /// </summary>
        [HideInInspector]
        public AudioSource source;

        /// <summary>
        /// Set the volume and pitch to 1.
        /// </summary>
        public Sound()
        {
            volume = 1f;
            pitch = 1f;
        }
    }
}