using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class StartGameClient : MonoBehaviour
{
    [SerializeField] private GameObject initialScreen;
    [SerializeField] private GameObject pictureScreen;

    [SerializeField] private TMPro.TMP_InputField roomInput;
    [SerializeField] private Button continueServerButton;
    [SerializeField] private Button joinGameButton;

    public static event Action OnJoinSharedSpaceClient;

    void Start()
    {
        continueServerButton.onClick.AddListener(JoinRoom);
        joinGameButton.onClick.AddListener(JoinGame);
    }

    private void JoinRoom()
    {
        string roomId = roomInput.text;
        StartGameAr.SetRoomId(roomId);

        SwapScreens();

        OnJoinSharedSpaceClient?.Invoke();
    }
    
    private void JoinGame()
    {
        StartGameAr.StartNewGame();

        NetworkManager.Singleton.StartClient();
        Debug.Log("Starting AR Client...");

        gameObject.SetActive(false);
    }

    private void SwapScreens()
    {
        initialScreen.SetActive(false);
        pictureScreen.SetActive(true);
    }
}
