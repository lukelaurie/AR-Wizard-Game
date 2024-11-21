using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class StartGameHost : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text roomIdText;
    [SerializeField] private Button startGameButton;

    public static event Action OnStartSharedSpaceHost;

    async void Start()
    {
        string roomId = await RoomManager.Instance.CreateRoom();

        // have text pop up on screen saying invalid credentials
        if (roomId == null)
        {
            Debug.LogError("Invalid credentials");
            return;
        }

        roomIdText.text = roomId;
        StartGameAr.SetRoomId(roomId);

        OnStartSharedSpaceHost?.Invoke();

        StartGameAr.StartNewGame();

        Debug.Log("Starting The AR Dedicated Server...");
        NetworkManager.Singleton.StartHost();

        startGameButton.onClick.AddListener(startGameHost);
    }

    private void startGameHost()
    {
        // get the other players in the room to notify them that game is starting
        NotifyClientsStartGame();

        gameObject.SetActive(false);

    }

    private async void NotifyClientsStartGame() {
        string[] players = await RoomManager.Instance.GetPlayersInRoom();

        Debug.Log("top");
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            Debug.Log("in");
            var clientObj = client.PlayerObject.GetComponent<StartGameClient>();
            Debug.Log(clientObj);
            if (clientObj != null)
            {
                clientObj.JoinGameClientRpc("test");
            }
        }
    }
}
