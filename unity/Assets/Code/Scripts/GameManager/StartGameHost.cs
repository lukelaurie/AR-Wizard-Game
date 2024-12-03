using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class StartGameHost : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text roomIdText;
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private TMPro.TMP_Dropdown bossDropdown;
    private PlayerData playerData;

    async void Start()
    {
        playerData = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<PlayerData>();
        string roomId = await RoomManager.Instance.CreateRoom();

        // have text pop up on screen saying invalid credentials
        if (roomId == null)
        {
            Debug.LogError("Invalid credentials");
            return;
        }

        roomIdText.text = roomId;
        StartGameAr.SetRoomId(roomId);

        StartGameAr startGameAr = GameObject.FindWithTag(TagManager.Scenary).GetComponent<StartGameAr>();
        startGameAr.BlitImageForColocalizationOnTextureRender();

        Debug.Log("Starting The AR Dedicated Server...");
        NetworkManager.Singleton.StartHost();

        playerData.SetIsPlayerHost(true);

        startGameButton.onClick.AddListener(StartGame);
        homeButton.onClick.AddListener(SwapScreens.Instance.QuitGame);

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

        NetworkObject playerPrefab = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
        NotifyClient clientNotifyObj = playerPrefab.GetComponent<NotifyClient>();
        clientNotifyObj.JoinGameClientRpc();
    }

    private async void StartGame()
    {
        AllClientsInvoker.Instance.InvokeJoinGameAllClients();

        StartGameAr.StartNewGame();

        // have the server manage the game being started 
        await RoomManager.Instance.StartGameInRoom();

        // select the boss and difficulty to use
        playerData.SetBossLevel(bossDropdown.value + 1);

        gameObject.SetActive(false);
    }
}
