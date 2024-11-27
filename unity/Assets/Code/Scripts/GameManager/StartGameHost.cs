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


        NetworkManager.Singleton.StartHost();
        Debug.Log("Starting The AR Dedicated Server...");

        var clientData = GameObject.FindWithTag("playerInfo").GetComponent<PlayerData>();
        clientData.SetIsPlayerHost(true);

        startGameButton.onClick.AddListener(StartGame);
        
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;

    }

    // once connected join the room if it has already started 
    private async void OnClientConnected(ulong clientId)
    {
        bool isGameStarted = await RoomManager.Instance.IsGameStarted();

        if (!isGameStarted)
        {
            return;
        }

        var playerPrefab = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
        var clientNotifyObj = playerPrefab.GetComponent<NotifyClient>(); 
        clientNotifyObj.JoinGameClientRpc();
    }

    private async void StartGame()
    {
        NotifyClientsStartGame();

        StartGameAr.StartNewGame();
        // have the server manage the game being started 
        await RoomManager.Instance.StartGameInRoom();
        gameObject.SetActive(false);
    }

    private async void NotifyClientsStartGame()
    {
        string[] roomPlayers = await RoomManager.Instance.GetPlayersInRoom();

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            var clientNotifyObj = client.PlayerObject.GetComponent<NotifyClient>();
            var clientData = GameObject.FindWithTag("playerInfo").GetComponent<PlayerData>();
            Debug.Log(clientData.GetUsername());

            if (clientNotifyObj != null && Array.Exists(roomPlayers, player => player == clientData.GetUsername()))
            {
                clientNotifyObj.JoinGameClientRpc();
            }
        }
    }
}
