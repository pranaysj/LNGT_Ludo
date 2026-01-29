using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;




public class OnClickProfileButton : MonoBehaviour
{
 
    [Header("Profile Stats")]
    public TextMeshProUGUI winText;
    public TextMeshProUGUI loseText;
    public TextMeshProUGUI matchText;

    [Header("Avatar System")]
public Sprite[] avatars;                 // ALL avatars list
public Image homeAvatarImg;              // Home page avatar
public Image profileCenterAvatarImg;     // Profile center avatar
public Image profileLeftAvatarImg;       // Profile left avatar


    [Header("Profile Username")]
    public TextMeshProUGUI profileUsernameText;   //  PROFILE NAME TEXT Chhe
    public TMP_InputField nameInputField;
    public GameObject inputFieldRoot; // Image + InputField parent
    public TextMeshProUGUI thirdUsernameText; //Top Name Profile Text Devani Chhe


    [Header("Buttons")]
    public GameObject editButton;
    public GameObject saveButton;
    private TextMeshPro username;

    //public GameObject EditAvtarButton;

    void OnEnable()
    {
        UpdateProfileStats();
        RefreshUsernameFromServer();
        RefreshAvatarFromServer();
    }

    public void OnClickProfile()
    {
       
    }

    //  Edit button
    public void OnClickEditProfile()
    {
        editButton.SetActive(false);
        nameInputField.gameObject.SetActive(true);

        profileUsernameText.gameObject.SetActive(false);
        //EditAvtarButton.SetActive(true);
        inputFieldRoot.SetActive(true);

        saveButton.SetActive(true);

        
        nameInputField.text = profileUsernameText.text; 
    }

    //  Save button
    public void OnClickSaveName()
    {
        string newName = nameInputField.text;

        if (string.IsNullOrEmpty(newName) || newName.Length < 3)
            return;

        // SINGLE SOURCE UPDATE
        PhotonNetwork.NickName = newName;
        PlayerPrefs.SetString("username", newName);
        PlayerPrefs.Save();
        username.text = PlayerPrefs.GetInt("username").ToString();
        RefreshUsernameFromServer();

        nameInputField.gameObject.SetActive(false);
        profileUsernameText.gameObject.SetActive(true);
        inputFieldRoot.SetActive(false);
        saveButton.SetActive(false);
        editButton.SetActive(true);
    }

    // Stats
    public void UpdateProfileStats()
    {
        winText.text = PlayerPrefs.GetInt("win", 0).ToString();
        loseText.text = PlayerPrefs.GetInt("lose", 0).ToString();
        matchText.text = PlayerPrefs.GetInt("match", 0).ToString();
    }

   


    public void RefreshUsernameFromServer()
{
    string finalName;

    if (!string.IsNullOrEmpty(PhotonNetwork.NickName))
        finalName = PhotonNetwork.NickName;
    else
        finalName = PlayerPrefs.GetString("username", "Player");

    // Center profile name
    profileUsernameText.text = finalName;

    //  Left side icon ke paas wala name (3rd place)
    if (thirdUsernameText != null)
        thirdUsernameText.text = finalName;
}


public void OnClickEditAvatar()
{
   
    

}

public void RefreshAvatarFromServer()
{
    int avatarIndex = PlayerPrefs.GetInt("avatar", 0);

    if (avatarIndex < 0 || avatarIndex >= avatars.Length)
        avatarIndex = 0;

    // Home page avatar
    if (homeAvatarImg != null)
        homeAvatarImg.sprite = avatars[avatarIndex];

    // Profile center avatar
    if (profileCenterAvatarImg != null)
        profileCenterAvatarImg.sprite = avatars[avatarIndex];

    // Profile left avatar
    if (profileLeftAvatarImg != null)
        profileLeftAvatarImg.sprite = avatars[avatarIndex];

}

public void OnAvatarSelected(int avatarIndex)
{
    PlayerPrefs.SetInt("avatar", avatarIndex);
    PlayerPrefs.Save();

    RefreshAvatarFromServer(); // 👈 teeno jagah update
}



}
