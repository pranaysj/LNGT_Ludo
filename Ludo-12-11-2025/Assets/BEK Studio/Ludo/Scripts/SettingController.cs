using BEKStudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{
    public Toggle sfxToggle;

    void Start()
    {
        Invoke(nameof(InitSfxToggle), 0.5f);
    }

    private void InitSfxToggle()
    {
        sfxToggle.isOn = AudioController.Instance.isSFXEnable;
        sfxToggle.onValueChanged.AddListener(OnSfxToggleChanged);
    }

    void OnSfxToggleChanged(bool isOn)
    {
        AudioController.Instance.PlayButtonSound();
        AudioController.Instance.SetSFXEnable(isOn);
    }
}
