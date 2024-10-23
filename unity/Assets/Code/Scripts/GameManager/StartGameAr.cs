using System;
using System.Collections;
using System.Collections.Generic;
using Niantic.Lightship.AR.Settings;
using Niantic.Lightship.SharedAR.Colocalization;
using Niantic.Lightship.SharedAR.Netcode;
using Niantic.Lightship.SharedAR.Rooms;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class StartGameAr : MonoBehaviour
{
    [SerializeField] private SharedSpaceManager sharedSpaceManager;
    [SerializeField] private Texture2D targetImage;
    [SerializeField] private float targetImageSize;

    [SerializeField] private Button startGameButton;
    [SerializeField] private Button joinServerButton;
    [SerializeField] private Canvas createGameCanvas;
    [SerializeField] private Button startServerButton;

    public static event Action OnStartSharedSpaceHost;
    public static event Action OnJoinSharedSpaceClient;
    public static event Action OnStartGame;
    public static event Action OnStartSharedSpace;

    private bool isHost;

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        DontDestroyOnLoad(gameObject);

        // give each player a id
        System.Random random = new System.Random();
        int randomNumber = random.Next();
        PrivacyData.SetUserId(randomNumber.ToString());

        // check if running on the server 
        if (Application.isBatchMode)
        {
            StartServer();
            return;
        }

        toggleInitialButtons();

        startGameButton.onClick.AddListener(StartGame);
        joinServerButton.onClick.AddListener(JoinServer);
        startServerButton.onClick.AddListener(StartServer);

        BlitImageForColocalization.OnTextureRendered += BlitImageForColocalizationOnTextureRender;
    }

    private void BlitImageForColocalizationOnTextureRender(Texture2D texture)
    {
        // when the user joins a game the image gets pushed into the texture so 
        // we can now set that texture and start up our room
        SetTargetImage(texture);
        StartSharedSpace();
    }

    private void SetTargetImage(Texture2D texture)
    {
        targetImage = texture;
    }

    private void StartSharedSpace()
    {
        string roomName = "Random1Room";
        const int MAX_AMOUNT_CLIENTS_ROOM = 32;

        OnStartSharedSpace?.Invoke();

        if (sharedSpaceManager.GetColocalizationType() == SharedSpaceManager.ColocalizationType.MockColocalization)
        {
            var mockTrackingArgs = ISharedSpaceTrackingOptions.CreateMockTrackingOptions();
            var roomArgs = ISharedSpaceRoomOptions.CreateLightshipRoomOptions(
                roomName, MAX_AMOUNT_CLIENTS_ROOM, "MockColocalizationDemo"
            );

            sharedSpaceManager.StartSharedSpace(mockTrackingArgs, roomArgs);
            return;
        }

        if (sharedSpaceManager.GetColocalizationType() == SharedSpaceManager.ColocalizationType.ImageTrackingColocalization)
        {
            var imageTrackingOptions = ISharedSpaceTrackingOptions.CreateImageTrackingOptions(
                targetImage, targetImageSize
            );

            var roomArgs = ISharedSpaceRoomOptions.CreateLightshipRoomOptions(
                roomName, MAX_AMOUNT_CLIENTS_ROOM, "ImageColocalizationDemo"
            );

            sharedSpaceManager.StartSharedSpace(imageTrackingOptions, roomArgs);
            return;
        }
    }

    private void JoinServer()
    {
        isHost = false;
        toggleButtonsOff();
        OnJoinSharedSpaceClient?.Invoke();
    }

    private void StartServer()
    {
        isHost = true;
        toggleButtonsOff();
        OnStartSharedSpaceHost?.Invoke();

    }

    private void StartGame() {
        OnStartGame?.Invoke();

        if (isHost)
        {
            Debug.Log("Starting The AR Dedicated Server...");
            NetworkManager.Singleton.StartHost();
        }
        else 
        {
            NetworkManager.Singleton.StartClient();
            Debug.Log("Starting AR Client...");
        }

        createGameCanvas.gameObject.SetActive(false);
    }

    private void OnClientConnectedCallback(ulong clientId)
    {
        Debug.Log($"Client connected: {clientId}");
    }

    private void toggleInitialButtons() {
        startGameButton.interactable = false;
        joinServerButton.interactable = true;
        startServerButton.interactable = true;
    }

    private void toggleButtonsOff() {
        startGameButton.interactable = true;
        joinServerButton.interactable = false;
        startServerButton.interactable = false;
    }
}