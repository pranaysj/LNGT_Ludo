using BEKStudio;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardController : MonoBehaviour
{
    public static RewardController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    [Header("Claim Button")]
    [SerializeField] private Button claimAllButton;

    [Header("Daily Chest UI")]
    [SerializeField] private TMP_Text dailyTimerText;
    [SerializeField] private Image dailyBar;

    [Header("Weekly Chest UI")]
    [SerializeField] private TMP_Text weeklyTimerText;
    [SerializeField] private Image weeklyBar;

    [Header("Special Chest UI")]
    [SerializeField] private TMP_Text specialTimerText;
    [SerializeField] private Image specialBar;

    private const string DAILY_KEY = "Daily_LastClaim";
    private const string WEEKLY_KEY = "Weekly_LastClaim";
    private const string SPECIAL_KEY = "Special_LastClaim";

    private readonly TimeSpan DAILY_CD = TimeSpan.FromHours(24);
    private readonly TimeSpan WEEKLY_CD = TimeSpan.FromDays(7);
    private readonly TimeSpan SPECIAL_CD = TimeSpan.FromDays(14);

    private void Start()
    {
        InvokeRepeating(nameof(UpdateAllUI), 0f, 1f);
    }

    // ===================== CLAIM ALL =====================

    public void ClaimAll()
    {
        AudioController.Instance.PlayButtonSound();

        int totalCoins = 0;

        if (CanClaim(DAILY_KEY, DAILY_CD))
        {
            totalCoins += 5;
            SaveClaimTime(DAILY_KEY);
        }

        if (CanClaim(WEEKLY_KEY, WEEKLY_CD))
        {
            totalCoins += 20;
            SaveClaimTime(WEEKLY_KEY);
        }

        if (CanClaim(SPECIAL_KEY, SPECIAL_CD))
        {
            totalCoins += 50;
            SaveClaimTime(SPECIAL_KEY);
        }

        if (totalCoins > 0)
        {
            AddCoins(totalCoins);
        }

        UpdateAllUI();
    }

    // ===================== UI UPDATE =====================

    private void UpdateAllUI()
    {
        bool anyAvailable = false;

        UpdateChestUI(DAILY_KEY, DAILY_CD, dailyTimerText, dailyBar, ref anyAvailable);
        UpdateChestUI(WEEKLY_KEY, WEEKLY_CD, weeklyTimerText, weeklyBar, ref anyAvailable);
        UpdateChestUI(SPECIAL_KEY, SPECIAL_CD, specialTimerText, specialBar, ref anyAvailable);

        claimAllButton.interactable = anyAvailable;
    }

    private void UpdateChestUI(
        string key,
        TimeSpan cooldown,
        TMP_Text timerText,
        Image bar,
        ref bool anyAvailable)
    {
        DateTime lastClaim = GetLastClaimTime(key, cooldown);
        TimeSpan elapsed = DateTime.UtcNow - lastClaim;

        if (elapsed >= cooldown)
        {
            timerText.text = "READY";
            bar.fillAmount = 1f;
            anyAvailable = true;
        }
        else
        {
            TimeSpan remaining = cooldown - elapsed;
            timerText.text = FormatTime(remaining);

            float progress = (float)(elapsed.TotalSeconds / cooldown.TotalSeconds);
            bar.fillAmount = Mathf.Clamp01(progress);
        }
    }

    // ===================== TIME LOGIC =====================

    private bool CanClaim(string key, TimeSpan cooldown)
    {
        return DateTime.UtcNow - GetLastClaimTime(key, cooldown) >= cooldown;
    }

    private DateTime GetLastClaimTime(string key, TimeSpan cooldown)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return DateTime.Parse(
                PlayerPrefs.GetString(key),
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.RoundtripKind
            );
        }

        // First-time behavior
        if (key == DAILY_KEY)
        {
            return DateTime.UtcNow - cooldown; // Daily is ready immediately
        }

        return DateTime.UtcNow; // Weekly & Special start countdown
    }

    private void SaveClaimTime(string key)
    {
        PlayerPrefs.SetString(key, DateTime.UtcNow.ToString("o")); // ISO-8601
        PlayerPrefs.Save();
    }

    // ===================== HELPERS =====================

    private string FormatTime(TimeSpan time)
    {
        int totalHours = time.Days * 24 + time.Hours;

        if (time.Days > 1)
            return $"{time.Days} days";

        return $"{totalHours:D2}:{time.Minutes:D2}:{time.Seconds:D2}";
    }

    private void AddCoins(int amount)
    {
        Debug.Log($"Claimed coins: {amount}");
        PlayerPrefs.SetInt("coin", PlayerPrefs.GetInt("coin", 0) + amount);
        PlayerPrefs.Save();
    }
}

