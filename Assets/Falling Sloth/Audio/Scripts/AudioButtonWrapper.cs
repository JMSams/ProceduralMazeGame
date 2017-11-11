using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FallingSloth.Audio
{
    /// <summary>
    /// Wrapper that allows a UI element to play and stop a sound from an <see cref="AudioManager"/>.
    /// </summary>
    public class AudioButtonWrapper : MonoBehaviour
    {
        /// <summary>
        /// The name of the sound to control.
        /// </summary>
        public string soundName = "sound1";

        /// <summary>
        /// Plays the sound with the given name.
        /// </summary>
        public void PlaySound()
        {
            AudioManager.PlaySound(soundName);
        }

        /// <summary>
        /// Stops the sound with the given name.
        /// </summary>
        public void StopSound()
        {
            AudioManager.StopSound(soundName);
        }
    }
}