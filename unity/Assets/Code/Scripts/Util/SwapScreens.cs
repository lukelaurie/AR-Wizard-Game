using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwapScreens : MonoBehaviour
{
    // ensure that this is a singleton object 
    public static SwapScreens Instance { get; private set;} // can only set/modify in this class
    
    // the screen to toggle between
    [SerializeField] private GameObject homeScreen;
    [SerializeField] private GameObject hostScreen;
    [SerializeField] private GameObject joinScreen;


    void Awake()
    {
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
        homeScreen.SetActive(false);
        joinScreen.SetActive(false);
        hostScreen.SetActive(true);
    }

    public void ToggleJoinScreen()
    {
        homeScreen.SetActive(false);
        hostScreen.SetActive(false);
        joinScreen.SetActive(true);
    }
}
