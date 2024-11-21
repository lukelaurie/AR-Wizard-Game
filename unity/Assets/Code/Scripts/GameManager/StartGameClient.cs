using System;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class StartGameClient : NetworkBehaviour
{
    [SerializeField] private GameObject initialScreen;
    [SerializeField] private GameObject pictureScreen;

    [SerializeField] private TMPro.TMP_Text invalidIdText;
    [SerializeField] private TMPro.TMP_InputField roomInput;
    [SerializeField] private Button continueServerButton;
    [SerializeField] private Button joinGameButton;

    public static event Action OnJoinSharedSpaceClient;

    void Start()
    {
        continueServerButton.onClick.AddListener(JoinRoom);
        // joinGameButton.onClick.AddListener(JoinGame);
    }

    private async void JoinRoom()
    {
        string roomId = roomInput.text;

        // attempt to join the room in the go server 
        if (!await RoomManager.Instance.JoinRoom(roomId))
        {
            invalidIdText.gameObject.SetActive(true);
            await Task.Delay(2000); // wait for 2 seconds
            invalidIdText.gameObject.SetActive(false);

            return;
        }

        StartGameAr.SetRoomId(roomId);

        SwapScreens();

        OnJoinSharedSpaceClient?.Invoke();

        // join the game as a new player
        // StartGameAr.StartNewGame();

        NetworkManager.Singleton.StartClient();
        Debug.Log("Starting AR Client...");
    }

    // This RPC will be sent to all of the clients 
    [ClientRpc]
    public void JoinGameClientRpc(string targetUsername)
    {
        Debug.Log("at least method is called");
        if (IsOwner)
        {
            Debug.Log("calling method here " + targetUsername);
        }
        // if (PlayerData.username == targetUsername)
        // {
        //     StartGameAr.StartNewGame();

        //     NetworkManager.Singleton.StartClient();
        //     Debug.Log("Starting AR Client...");

        //     // gameObject.SetActive(false);
        // }
    }

    private void SwapScreens()
    {
        initialScreen.SetActive(false);
        pictureScreen.SetActive(true);
    }
}
