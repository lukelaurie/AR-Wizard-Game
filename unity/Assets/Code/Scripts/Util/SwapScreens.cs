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
    [SerializeField] private GameObject joinRoomEnterIdScreen;
    [SerializeField] private GameObject joinRoomStartScreen;
    
    [SerializeField] private GameObject GameStateScreens;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject deathScreen;
    
    [SerializeField] private GameObject gameScreen;
    [SerializeField] private GameObject spellBar;
    [SerializeField] private GameObject partyHealth;
    private PlayerData playerData;


    void Awake()
    {
        playerData = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<PlayerData>();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void QuitGame()
    {
        NetworkManager.Singleton.Shutdown();
        List<GameObject> netObjects =
            FindObjectsOfType<NetworkObject>().Select(obj => obj.transform.gameObject).ToList();

        foreach (var obj in netObjects)
        {
            Destroy(obj);
        }

        Destroy(FindObjectOfType<StartGameAr>().gameObject);

        Destroy(FindObjectOfType<NetworkManager>().transform.gameObject);
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void ToggleHostScreen()
    {
        mainScreens.SetActive(false);
        joinRoomScreen.SetActive(false);
        createRoomScreen.SetActive(true);
    }

    public void ToggleJoinScreen()
    {
        mainScreens.SetActive(false);
        createRoomScreen.SetActive(false);
        joinRoomScreen.SetActive(true);
    }

    public void ToggleGameBackgroundClient()
    {
        joinRoomScreen.SetActive(false);
        gameScreen.SetActive(true);
        spellBar.SetActive(true);
        partyHealth.SetActive(true);
    }

    public void ToggleResetGameClient()
    {
        loseScreen.SetActive(false);
        joinRoomScreen.SetActive(true);
    }

    public void ToggleClientJoinGame()
    {
        joinRoomEnterIdScreen.SetActive(false);
        joinRoomStartScreen.SetActive(true);
    }

    public void ToggleLossScreen()
    {
        gameScreen.SetActive(false);
        deathScreen.SetActive(false);
        spellBar.SetActive(false);
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
        gameScreen.SetActive(false);
        winScreen.SetActive(true);

        TMPro.TMP_Text childText = winScreen.transform.GetChild(3).GetComponent<TMPro.TMP_Text>();
        childText.text = userReward.ToString();
    }

    public void ToggleDeathScreen()
    {
        gameScreen.SetActive(false);
        deathScreen.SetActive(true);
    }
}
