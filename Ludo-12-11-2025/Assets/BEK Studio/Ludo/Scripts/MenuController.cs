using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;


namespace BEKStudio {
    public class MenuController : MonoBehaviour {
        public static MenuController Instance;
        public GameObject dontDestroyPrefab;
        [Header("Top")]
        public GameObject splashScreen;
        public GameObject menuScreen;
        [Header("")]
        public Sprite[] avatars;
        [Header("Top")]
        public Image topAvatarImg;
        public TextMeshProUGUI topUsernameText;
        public TextMeshProUGUI topCoinText;
        [Header("Main")]
        public GameObject mainBottom;
        public GameObject mainBottomHomeActive;
        public GameObject mainBottomStoreActive;
        [Header("Home")]
        public GameObject homeScreen;
        public RectTransform homeTitle;
        public RectTransform homeOnline;
        public RectTransform homeComputer;
        public RectTransform homecomputer_offlinemultiplayer;
        [Header("Store")]
        public GameObject storeScreen;
        public GameObject storePanel;
        [Header("Pawn Select")]
        public GameObject pawnSelectScreen;
        public GameObject pawnSelectPanel;
        [Header("Player Count")]
        public GameObject playerCountScreen;
        public GameObject playerCountPanel;
        public TextMeshProUGUI playerCountEntryFee;
        [Header("Online")]
        public GameObject onlineScreen;
        public GameObject onlinePanel;
        public TextMeshProUGUI onlineInfoText;
        public Button onlineCancelButton;
        [Header("Username")]
        public GameObject usernameScreen;
        public GameObject usernamePanel;
        public TMP_InputField usernameInput;


        void Awake() {
            if (Instance == null) {
                Instance = this;
            }
        }

        void Start() {

            //Strat Splash Screen
            StartCoroutine(StartSplashScreen());

            if (PlayerPrefs.HasKey("pawnColor")) {
                PlayerPrefs.DeleteKey("pawnColor");
            }

            if(PlayerPrefs.HasKey("IsOfflineMultiplayer"))
            {
                PlayerPrefs.DeleteKey("IsOfflineMultiplayer");
            }

            GameObject dontDestroyObj = GameObject.Find("DontDestroy");
            if (dontDestroyObj == null){
                dontDestroyObj = Instantiate(dontDestroyPrefab);
                dontDestroyObj.name = "DontDestroy";
                DontDestroyOnLoad(dontDestroyObj);
            }


            if (!PlayerPrefs.HasKey("firstTime")) {
                PlayerPrefs.SetInt("avatar", Random.Range(0, avatars.Length));
                PlayerPrefs.SetInt("coin", Constants.START_COIN);
                PlayerPrefs.SetInt("firstTime", 1);
                PlayerPrefs.Save();
            }

            topAvatarImg.sprite = avatars[PlayerPrefs.GetInt("avatar")];
            UpdateCoinText();

            if (!PlayerPrefs.HasKey("username")) {
#if UNITY_WEBGL
                string rand = Random.Range(999, 999999).ToString();
                PlayerPrefs.SetString("username", "Player" + rand);
                PlayerPrefs.Save();
                topUsernameText.text = PlayerPrefs.GetString("username");
                HomeShow();
#else
                UsernameShow();
#endif
            } else {
                topUsernameText.text = PlayerPrefs.GetString("username");
                HomeShow();
            }

            
            AdsManager.Instance.DestoryBannerAd();
        }

        private IEnumerator StartSplashScreen()
        {
            splashScreen.SetActive(true);
            menuScreen.SetActive(false);
            float splashTime = 4.0f;

            yield return new WaitForSeconds(splashTime);

            splashScreen.SetActive(false);
            menuScreen.SetActive(true);
            
        }

        public void UpdateCoinText() {
            int coin = PlayerPrefs.GetInt("coin");
            topCoinText.text = coin == 0 ? "0" : coin.ToString("###,###,###");
        }

        void HomeShow() {
            homeTitle.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            homeOnline.anchoredPosition = new Vector2(-765f, -24.7f);
            homeComputer.anchoredPosition = new Vector2(765f, -461.7f);
            homecomputer_offlinemultiplayer.anchoredPosition = new Vector2(0, -1337f);

            homeScreen.SetActive(true);
            MainBottomCheckTabs();

            LeanTween.alpha(homeTitle, 1, 0.2f);
            LeanTween.move(homeOnline, new Vector2(0, 192f), 0.2f).setDelay(0.1f);
            LeanTween.move(homeComputer, new Vector2(0, -215f), 0.2f).setDelay(0.2f);
            LeanTween.move(homecomputer_offlinemultiplayer, new Vector2(0, -592f), 0.2f).setDelay(0.3f);
        }

        void HomeClose() {
            LeanTween.alpha(homeTitle, 0, 0.2f);
            LeanTween.move(homeOnline, new Vector2(-765f, -24.7f), 0.2f).setDelay(0.1f);
            LeanTween.move(homeComputer, new Vector2(765f, -461.7f), 0.2f).setDelay(0.2f).setOnComplete(() => {
                homeScreen.SetActive(false);
            });
        }

        void MainBottomCheckTabs() {
            mainBottomHomeActive.SetActive(homeScreen.activeInHierarchy);
            mainBottomStoreActive.SetActive(storeScreen.activeInHierarchy);
        }

        public void MainOnlineBtn() {
            AudioController.Instance.PlayButtonSound();
            if (onlineScreen.activeInHierarchy) return;

            PlayerPrefs.SetString("mode", "online");
            PlayerPrefs.Save();
            PlayerCountShow();
        }

        public void MainVsComputerBtn() {
            AudioController.Instance.PlayButtonSound();
            if (pawnSelectScreen.activeInHierarchy) return;

            PlayerPrefs.SetInt("IsOfflineMultiplayer", 0);
            PlayerPrefs.SetString("mode", "computer");
            PlayerPrefs.Save();
            PawnSelectShow();
        }

        public void MainVsComputerBtn_OfflineMultiplayer()
        {
            AudioController.Instance.PlayButtonSound();
            if (pawnSelectScreen.activeInHierarchy) return;

            PlayerPrefs.SetInt("IsOfflineMultiplayer", 1);
            PlayerPrefs.SetString("mode", "computer");
            PlayerPrefs.Save();
            PawnSelectShow();
        }

        public void MainHomeBtn() {
            AudioController.Instance.PlayButtonSound();
            if (homeScreen.activeInHierarchy) return;

            if (storeScreen.activeInHierarchy && !LeanTween.isTweening(storePanel)) {
                StoreClose();
            }
        }

        public void MainWatchVideoBtn() {
            AdsManager.Instance.ShowRewardedAd();
        }

        public void MainStoreBtn() {
            AudioController.Instance.PlayButtonSound();
            if (storeScreen.activeInHierarchy) return;

            StoreShow();
        }

        void StoreShow() {
            storePanel.transform.localScale = Vector2.zero;
            storeScreen.SetActive(true);

            if (homeScreen.activeInHierarchy) {
                HomeClose();
            }

            LeanTween.scale(storePanel, Vector2.one, 0.2f).setDelay(0.41f).setEaseOutBack().setOnStart(() => {
                MainBottomCheckTabs();
            });
        }

        void StoreClose(bool showHome = true) {
            LeanTween.scale(storePanel, Vector2.zero, 0.2f).setEaseInBack().setOnComplete(() => {
                storeScreen.SetActive(false);

                if (showHome) {
                    HomeShow();
                    MainBottomCheckTabs();
                }
            });
        }

        public void StoreItemBtn(int id) {
            Purchaser.Instance.BuyConsumable(id);
        }

        public void StoreRestoreBtn() {
            Purchaser.Instance.RestorePurchases();
        }

        public void StoreCloseBtn() {
            AudioController.Instance.PlayButtonSound();
            if (LeanTween.isTweening(storePanel)) return;

            StoreClose();
        }

        void PawnSelectShow() {
            pawnSelectPanel.transform.localScale = Vector2.zero;
            pawnSelectScreen.SetActive(true);

            LeanTween.scale(pawnSelectPanel, Vector2.one, 0.2f).setEaseOutBack();
        }

        public void PawnSelectItemBtn(string pawnColor) {
            AudioController.Instance.PlayButtonSound();
            PlayerPrefs.SetString("pawnColor", pawnColor);
            PlayerPrefs.Save();
            PawnSelectClose();
        }

        public void PawnSelectCloseBtn() {
            AudioController.Instance.PlayButtonSound();
            if (LeanTween.isTweening(pawnSelectPanel)) return;

            PlayerPrefs.DeleteKey("pawnColor");
            PawnSelectClose();
        }

        void PawnSelectClose() {
            LeanTween.scale(pawnSelectPanel, Vector2.zero, 0.2f).setEaseInBack().setOnComplete(() => {
                pawnSelectScreen.SetActive(false);

                if (PlayerPrefs.HasKey("pawnColor")) {
                    PlayerCountShow();
                }
            });
        }

        void PlayerCountShow() {
            playerCountPanel.transform.localScale = Vector2.zero;
            playerCountEntryFee.text = PhotonController.Instance.gameEntryPrice().ToString("###,###,###");
            playerCountScreen.SetActive(true);

            LeanTween.scale(playerCountPanel, Vector2.one, 0.2f).setEaseOutBack();
        }

        public void PlayerCountItemBtn(int playerCount) {
            AudioController.Instance.PlayButtonSound();

            int entryFee = PhotonController.Instance.gameEntryPrice();

            if (PlayerPrefs.GetInt("coin") < entryFee) {
                playerCountScreen.SetActive(false);
                StoreShow();
                return;
            }
            
            PlayerPrefs.SetInt("playerCount", playerCount);
            PlayerPrefs.Save();
            if (PhotonController.Instance.gameMode() == "online") {
                OnlineShow();
                PhotonController.Instance.Connect();
            } else {
                SceneManager.LoadScene("Game");
            }
        }

        public void PlayerCountCloseBtn() {
            AudioController.Instance.PlayButtonSound();
            if (LeanTween.isTweening(playerCountPanel)) return;

            PlayerPrefs.DeleteKey("pawnSelect");
            PlayerCountClose();
        }

        void PlayerCountClose() {
            LeanTween.scale(playerCountPanel, Vector2.zero, 0.2f).setEaseInBack().setOnComplete(() => {
                playerCountScreen.SetActive(false);
            });
        }

        public void OnlineShow() {
            onlineCancelButton.interactable = true;
            onlinePanel.transform.localScale = Vector2.zero;
            onlineInfoText.text = "Connecting to server...";
            onlineScreen.SetActive(true);

            LeanTween.scale(onlinePanel, Vector2.one, 0.2f).setEaseOutBack();
        }

        public void OnlineClose() {
            if (LeanTween.isTweening(onlinePanel)) return;

            LeanTween.scale(onlinePanel, Vector2.zero, 0.2f).setEaseInBack().setOnComplete(() => {
                onlineScreen.SetActive(false);
            });
        }

        public void OnlineInfoMsg(string msg) {
            onlineInfoText.text = msg;
        }

        public void OnlineCancelBtn() {
            AudioController.Instance.PlayButtonSound();
            onlineCancelButton.interactable = false;
            PhotonNetwork.AutomaticallySyncScene = false;

            if (PhotonNetwork.IsConnectedAndReady) {
                PhotonNetwork.Disconnect();
            } else {
                OnlineClose();
            }
        }

        void UsernameShow() {
            usernamePanel.transform.localScale = Vector2.zero;
            usernameScreen.SetActive(true);

            LeanTween.scale(usernamePanel, Vector2.one, 0.2f).setEaseOutBack();
        }

        public void UsernameSaveBtn() {
            AudioController.Instance.PlayButtonSound();
            if (usernameInput.text.Length >= 4) {
                PlayerPrefs.SetString("username", usernameInput.text);
                PlayerPrefs.Save();
                topUsernameText.text = usernameInput.text;
                UsernameClose();
            }
        }

        void UsernameClose() {
            LeanTween.scale(usernamePanel, Vector2.zero, 0.2f).setEaseInBack().setOnComplete(() => {
                usernameScreen.SetActive(false);

                if (!homeScreen.activeInHierarchy) {
                    HomeShow();
                }
            });
        }

    }
}