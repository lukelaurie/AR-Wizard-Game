using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainPage : MonoBehaviour
{
    [SerializeField] private Button openStoreButton;
    [SerializeField] private Button returnButton;
    [SerializeField] private Button logoutButton;

    [SerializeField] private Button joinServerButton;
    [SerializeField] private Button startServerButton;
    [SerializeField] private TMPro.TMP_Text coinText;

    private PlayerData playerData;
    // Start is called before the first frame update
    void Start()
    {
        playerData = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<PlayerData>();
        int userCoins = playerData.GetCoinTotal();
        coinText.text = $"Coins:\n{userCoins}";

        openStoreButton.onClick.AddListener(SwapScreens.Instance.ToggleStore);

        returnButton.onClick.AddListener(SwapScreens.Instance.ToggleMainFromStore);

        joinServerButton.onClick.AddListener(StartJoinScreen);
        startServerButton.onClick.AddListener(StartHostScreen);

        logoutButton.onClick.AddListener(() =>
        {
            UnityWebRequest.ClearCookieCache();
            SwapScreens.Instance.QuitGame();
        });
    }

    private void StartJoinScreen()
    {
        playerData.SetIsPlayerHost(false);
        SwapScreens.Instance.ToggleTakePictureScreen();
    }

    private void StartHostScreen()
    {
        playerData.SetIsPlayerHost(true);
        SwapScreens.Instance.ToggleTakePictureScreen();
    }

}
