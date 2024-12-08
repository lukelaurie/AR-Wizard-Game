using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwapScreens : MonoBehaviour
{
    // ensure that this is a singleton object 
    public static SwapScreens Instance { get; private set; } // can only set/modify in this class

    // the screen to toggle between
    [SerializeField] private GameObject mainScreens;
    [SerializeField] private GameObject homeScreen;
    [SerializeField] private GameObject storeScreen;

    [SerializeField] private GameObject startGameScreens;
    [SerializeField] private GameObject createRoomScreen;
    [SerializeField] private GameObject joinRoomScreen;
    [SerializeField] private GameObject takePictureScreen;
    [SerializeField] private GameObject joinRoomEnterIdScreen;
    [SerializeField] private GameObject joinRoomStartScreen;

    [SerializeField] private GameObject gameStateScreens;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject crossHair;

    [SerializeField] private GameObject spellBar;
    [SerializeField] private GameObject partyHealth;
    private PlayerData playerData;

    void Awake()
    {
        playerData = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<PlayerData>();
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void QuitGame()
    {
        // the game is already in reset state 
        if (GameObject.FindWithTag(TagManager.Scenary) == null)
            return;

        NetworkManager.Singleton.Shutdown();

        Destroy(GameObject.FindWithTag(TagManager.Scenary));
        Destroy(GameObject.FindWithTag(TagManager.NetworkManagerAr));
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void ToggleMainFromStore()
    {
        homeScreen.SetActive(true);
        storeScreen.SetActive(false);
    }

    public void ToggleStore()
    {
        homeScreen.SetActive(false);
        storeScreen.SetActive(true);
    }

    public void ToggleMainPage()
    {
        startGameScreens.SetActive(true);
        mainScreens.SetActive(true);
        gameStateScreens.SetActive(true);
    }

    public void ToggleTakePictureScreen()
    {
        mainScreens.SetActive(false);
        startGameScreens.SetActive(true);
        takePictureScreen.SetActive(true);
    }

    public void ToggleHostScreen()
    {
        takePictureScreen.SetActive(false);
        createRoomScreen.SetActive(true);
    }

    public void ToggleJoinScreen()
    {
        takePictureScreen.SetActive(false);
        joinRoomScreen.SetActive(true);
    }

    public void ToggleSpellBarOn()
    {
        spellBar.SetActive(true);
    }

    public void ToggleGameBackgroundClient()
    {
        joinRoomScreen.SetActive(false);
        partyHealth.SetActive(true);
    }

    public void ToggeOnCrosshair()
    {
        crossHair.SetActive(true);
    }

    public void ToggleResetGameClient()
    {
        loseScreen.SetActive(false);
        partyHealth.SetActive(false);
        joinRoomScreen.SetActive(true);
    }

    public void ToggleClientJoinGame()
    {
        joinRoomEnterIdScreen.SetActive(false);
        joinRoomStartScreen.SetActive(true);
    }

    public void ToggleLossScreen()
    {
        ToggleOffScreens();
        partyHealth.SetActive(false);
        loseScreen.SetActive(true);

        // have the host and client display different text
        TMPro.TMP_Text childText = loseScreen.transform.GetChild(1).GetComponent<TMPro.TMP_Text>();
        Transform tryAgainButton = loseScreen.transform.Find("TryAgain");
        if (playerData.IsPlayerHost())
        {
            childText.text = "Try Again...";
            tryAgainButton.gameObject.SetActive(true);
        }
        else
        {
            childText.text = "Waiting For Host...";
            tryAgainButton.gameObject.SetActive(false);
        }
    }

    public void ToggleWinScreen(int userReward)
    {
        ToggleOffScreens();
        partyHealth.SetActive(false);
        winScreen.SetActive(true);

        TMPro.TMP_Text childText = winScreen.transform.GetChild(3).GetComponent<TMPro.TMP_Text>();
        childText.text = userReward.ToString();
    }

    public void ToggleDeathScreen()
    {
        ToggleOffScreens();
        deathScreen.SetActive(true);
    }

    private void ToggleOffScreens()
    {
        spellBar.SetActive(false);
        winScreen.SetActive(false);
        deathScreen.SetActive(false);
        loseScreen.SetActive(false);
        crossHair.SetActive(false);
    }
}
