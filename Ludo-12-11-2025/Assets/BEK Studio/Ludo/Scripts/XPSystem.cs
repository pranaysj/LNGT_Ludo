using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPSystem : MonoBehaviour
{
    public Scrollbar scrollbar;
    public TMP_Text xpText;
    public TMP_Text levelText;

    public int level = 1;
    public int currentXP = 0;

    int[] fixedLevelXP =
    {
        100, 200, 350, 550, 800,
        1100, 1500, 2000, 2600, 3300
    };

    void Start()
    {
        //  LOAD SAVED DATA
        level = PlayerPrefs.GetInt("XP_Level", 1);
        currentXP = PlayerPrefs.GetInt("XP_Current", 0);

        if (level < 1) level = 1;

        UpdateUI();
    }

    public void AddXP(string result)
    {
        int xp = 5; // Finish match base XP

        if (result == "Win") xp += 10;
        else if (result == "Second") xp += 6;
        else if (result == "Third") xp += 3;

        currentXP += xp;

        while (currentXP >= GetMaxXPForLevel(level))
        {
            currentXP -= GetMaxXPForLevel(level);
            level++;
        }

        SaveXP();   //  SAVE AFTER EVERY MATCH
        UpdateUI();
    }

    int GetMaxXPForLevel(int level)
    {
        if (level <= 10)
            return fixedLevelXP[level - 1];

        int extraLevel = level - 10;
        return 3300 + (extraLevel * extraLevel * 500);
    }

    void UpdateUI()
    {
        int maxXP = GetMaxXPForLevel(level);

        scrollbar.value = 0f;
        scrollbar.size = (float)currentXP / maxXP;

        xpText.text = currentXP + "/" + maxXP;
        levelText.text = "Level " + level;
    }

    //  SAVE FUNCTION
    void SaveXP()
    {
        PlayerPrefs.SetInt("XP_Level", level);
        PlayerPrefs.SetInt("XP_Current", currentXP);
        PlayerPrefs.Save();
    }

    //  EXTRA SAFETY (optional but good)
    void OnApplicationQuit()
    {
        SaveXP();
    }
}
