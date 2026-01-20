using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BEKStudio {
    public class AudioController : MonoBehaviour {
        public static AudioController Instance;
        public AudioSource pawnMoveAudioSource;
        public AudioClip pawnMoveClip;
        public AudioSource diceAudioSource;
        public AudioClip diceClip;
        public AudioSource buttonAudioSource;
        public AudioClip buttonClip;

        void Awake() {
            if (Instance == null) {
                Instance = this;
            }
        }

        public void PlayPawnMoveSound() {
            if (pawnMoveAudioSource == null) return;
            if (pawnMoveClip == null) return;

            if (pawnMoveAudioSource.isPlaying) pawnMoveAudioSource.Stop();
            pawnMoveAudioSource.PlayOneShot(pawnMoveClip);
        }

        public void PlayDiceSound() {
            if (diceAudioSource == null) return;
            if (diceClip == null) return;

            if (diceAudioSource.isPlaying) diceAudioSource.Stop();
            diceAudioSource.PlayOneShot(diceClip);
        }

        public void PlayButtonSound() {
            if (buttonAudioSource == null) return;
            if (buttonClip == null) return;

            if (buttonAudioSource.isPlaying) buttonAudioSource.Stop();
            buttonAudioSource.PlayOneShot(buttonClip);
        }
    }
}