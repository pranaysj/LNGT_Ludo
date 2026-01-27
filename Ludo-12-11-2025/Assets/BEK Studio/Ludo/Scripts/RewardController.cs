using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewardController : MonoBehaviour
{
    public static RewardController Instance;

    public TMP_Text dailyTimerText;
    private DateTime gameOpenDT;
    private DateTime currentDT;
    private DateTime nextDailyRewardTime;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
