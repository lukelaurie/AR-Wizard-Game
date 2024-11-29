using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class StartGameHost : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text roomIdText;
    [SerializeField] private Button startGameButton;
    [SerializeField] private TMPro.TMP_Dropdown bossDropdown;

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

        var clientData = GameObject.FindWithTag("GameInfo").GetComponent<PlayerData>();
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
        BossData bossData = GameObject.FindWithTag("GameInfo").GetComponent<BossData>();

        AllClientsInvoker.Instance.InvokeJoinGameAllClients();

        StartGameAr.StartNewGame();

        // have the server manage the game being started 
        await RoomManager.Instance.StartGameInRoom();

        // select the boss and difficulty to use
        int bossHealth = 200;
        bossData.InitializeBossData(bossDropdown.value + 1, bossHealth);

        gameObject.SetActive(false);
    }
}
