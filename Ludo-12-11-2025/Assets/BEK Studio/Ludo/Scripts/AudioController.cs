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

        [Header("SFX Effect")]
        public bool isSFXEnable = true;
        private const string SFX_PREF_KEY = "SFX_ENABLED";


        void Awake() {
            if (Instance == null) {
                Instance = this;
                isSFXEnable = PlayerPrefs.GetInt(SFX_PREF_KEY, 1) == 1;
            }
        }

        public void SetSFXEnable(bool isEnable) {
            isSFXEnable = isEnable;
            PlayerPrefs.SetInt(SFX_PREF_KEY, isSFXEnable ? 1 : 0);
            PlayerPrefs.Save();
        }

        public void PlayPawnMoveSound() {
            if (!isSFXEnable) return;
            if (pawnMoveAudioSource == null) return;
            if (pawnMoveClip == null) return;

            if (pawnMoveAudioSource.isPlaying) pawnMoveAudioSource.Stop();
            pawnMoveAudioSource.PlayOneShot(pawnMoveClip);
        }

        public void PlayDiceSound() {
            if (!isSFXEnable) return;
            if (diceAudioSource == null) return;
            if (diceClip == null) return;

            if (diceAudioSource.isPlaying) diceAudioSource.Stop();
            diceAudioSource.PlayOneShot(diceClip);
        }

        public void PlayButtonSound() {
            if (!isSFXEnable) return;
            if (buttonAudioSource == null) return;
            if (buttonClip == null) return;

            if (buttonAudioSource.isPlaying) buttonAudioSource.Stop();
            buttonAudioSource.PlayOneShot(buttonClip);
        }
    }
}