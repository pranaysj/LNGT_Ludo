using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardController : MonoBehaviour
{
    public static RewardController Instance;


    [Header("UI")]
    [SerializeField] private Button claimButton;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Image progressBar; // FILL IMAGE

    [Header("Reward")]
    [SerializeField] private int coinReward = 5;

    private const string LAST_CLAIM_KEY = "DailyChest_LastClaim";
    private readonly TimeSpan COOLDOWN = TimeSpan.FromHours(24);

    private DateTime lastClaimTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        LoadLastClaimTime();
        UpdateUI();
    }

    void Update()
    {
        UpdateUI();
    }

    bool CanClaim()
    {
        return DateTime.UtcNow - lastClaimTime >= COOLDOWN;
    }

    public void Claim()
    {
        if (!CanClaim()) return;

        // Give reward
        AddCoins(coinReward);

        // Save claim time
        lastClaimTime = DateTime.UtcNow;
        PlayerPrefs.SetString(LAST_CLAIM_KEY, lastClaimTime.ToString());
        PlayerPrefs.Save();

        UpdateUI();
    }

    void UpdateUI()
    {
        if (CanClaim())
        {
            claimButton.interactable = true;
            timerText.text = "READY";
            progressBar.fillAmount = 1f;
        }
        else
        {
            claimButton.interactable = false;

            TimeSpan elapsed = DateTime.UtcNow - lastClaimTime;
            TimeSpan remaining = COOLDOWN - elapsed;

            timerText.text = FormatTime(remaining);

            // Fill decreases as time passes
            float progress = 1f - (float)(elapsed.TotalSeconds / COOLDOWN.TotalSeconds);
            progressBar.fillAmount = Mathf.Clamp01(progress);
        }
    }

    string FormatTime(TimeSpan time)
    {
        return $"{time.Hours:D2}:{time.Minutes:D2}:{time.Seconds:D2}";
    }

    void LoadLastClaimTime()
    {
        if (PlayerPrefs.HasKey(LAST_CLAIM_KEY))
        {
            lastClaimTime = DateTime.Parse(
                PlayerPrefs.GetString(LAST_CLAIM_KEY)
            );
        }
        else
        {
            // Allows immediate first claim
            lastClaimTime = DateTime.UtcNow - COOLDOWN;
        }
    }

    void AddCoins(int amount)
    {
        // Replace with your coin system
        Debug.Log($"Daily Chest claimed: +{amount} coins");
    }

}
