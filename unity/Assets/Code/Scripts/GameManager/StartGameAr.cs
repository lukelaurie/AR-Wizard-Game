using System;
using System.Collections;
using System.Collections.Generic;
using Niantic.Lightship.SharedAR.Colocalization;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class StartGameAr : NetworkBehaviour
{
    [SerializeField] private SharedSpaceManager sharedSpaceManager;
    const int MAX_AMOUNT_CLIENTS_ROOM = 2;

    [SerializeField] private Texture2D targetImage;
    [SerializeField] private float targetImageSize;
    private readonly string roomName = "TestRoom";

    [SerializeField] private Button startGameButton;
    [SerializeField] private Button createRoomButton;
    [SerializeField] private Button joinRoomButton;
    [SerializeField] private Button clientJoinSever;
    private bool isHost;

    public static event Action OnStartSharedSpaceHost;
    public static event Action OnJoinSharedSpaceClient;
    public static event Action OnStartGame;
    public static event Action OnStartSharedSpace;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        sharedSpaceManager.sharedSpaceManagerStateChanged += SharedSpaceManagerStateChange;

        startGameButton.onClick.AddListener(StartGame);
        createRoomButton.onClick.AddListener(CreateGameHost);
        joinRoomButton.onClick.AddListener(JoinGameClient);

        startGameButton.interactable = false;
    }

    private void SharedSpaceManagerStateChange(SharedSpaceManager.SharedSpaceManagerStateChangeEventArgs obj)
    {
        if (obj.Tracking)
        {
            startGameButton.interactable = true;
            createRoomButton.interactable = false; 
            joinRoomButton.interactable = false;
        }
    }

    void StartGame()
    {
        OnStartGame?.Invoke();
        if (isHost)
        {
            NetworkManager.Singleton.StartHost();
        }
        else 
        {
            NetworkManager.Singleton.StartClient();
        }
    }

    void StartSharedSpace() {
        OnStartSharedSpace?.Invoke();

        if (sharedSpaceManager.GetColocalizationType() == SharedSpaceManager.ColocalizationType.MockColocalization) {
            var mockTrackingArgs = ISharedSpaceTrackingOptions.CreateMockTrackingOptions();
            var roomArgs = ISharedSpaceRoomOptions.CreateLightshipRoomOptions(
                roomName,MAX_AMOUNT_CLIENTS_ROOM,"MockColocalizationDemo"
            );

            sharedSpaceManager.StartSharedSpace(mockTrackingArgs, roomArgs);
            return;
        }

        if (sharedSpaceManager.GetColocalizationType() == SharedSpaceManager.ColocalizationType.ImageTrackingColocalization) {
            var imageTrackingOptions = ISharedSpaceTrackingOptions.CreateImageTrackingOptions(
                targetImage, targetImageSize
            );

            var roomArgs = ISharedSpaceRoomOptions.CreateLightshipRoomOptions(
                roomName,MAX_AMOUNT_CLIENTS_ROOM,"ImageColocalizationDemo"
            );

            sharedSpaceManager.StartSharedSpace(imageTrackingOptions, roomArgs);
            return;
        }
    }

    void CreateGameHost()
    {
        isHost = true;
        OnStartSharedSpaceHost?.Invoke();
        StartSharedSpace();
    }

    void JoinGameClient()
    {
        isHost = false;
        OnJoinSharedSpaceClient?.Invoke();
        StartSharedSpace();
    }
}