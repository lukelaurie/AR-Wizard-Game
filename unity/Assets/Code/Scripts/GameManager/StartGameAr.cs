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

    [SerializeField] private Button joinServerButton;
    [SerializeField] private Button startServerButton;

    // the screen to toggle between
    [SerializeField] private GameObject initialScreen;
    [SerializeField] private GameObject hostScreen;
    [SerializeField] private GameObject joinScreen;

    public static event Action OnStartGame;
    public static event Action OnStartSharedSpace;

    private bool isHost;
    private static string roomId = "";

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        DontDestroyOnLoad(gameObject);

        SetRandomUserId();

        // check if running on the server 
        // if (Application.isBatchMode)
        // {
        //     StartServer();
        //     return;
        // }

        joinServerButton.onClick.AddListener(toggleJoinScreen);
        startServerButton.onClick.AddListener(toggleHostScreen);

        BlitImageForColocalization.OnTextureRendered += BlitImageForColocalizationOnTextureRender;
    }

    private void BlitImageForColocalizationOnTextureRender(Texture2D texture)
    {
        // when the user joins a game the image gets pushed into the texture so 
        // we can now set that texture and start up our room
        targetImage = texture;
        
        Debug.Log("Room Id Is: " + roomId);

        const int MAX_AMOUNT_CLIENTS_ROOM = 32;

        OnStartSharedSpace?.Invoke(); // show the image & retake button

        if (sharedSpaceManager.GetColocalizationType() == SharedSpaceManager.ColocalizationType.MockColocalization)
        {
            var mockTrackingArgs = ISharedSpaceTrackingOptions.CreateMockTrackingOptions();
            var roomArgs = ISharedSpaceRoomOptions.CreateLightshipRoomOptions(
                roomId, MAX_AMOUNT_CLIENTS_ROOM, "MockColocalizationDemo"
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
                roomId, MAX_AMOUNT_CLIENTS_ROOM, "ImageColocalizationDemo"
            );

            sharedSpaceManager.StartSharedSpace(imageTrackingOptions, roomArgs);
            return;
        }
    }

    private void SetRandomUserId()
    {
        System.Random random = new System.Random();
        int randomNumber = random.Next();
        PrivacyData.SetUserId(randomNumber.ToString());
    }
    private void OnClientConnectedCallback(ulong clientId)
    {
        Debug.Log($"Client connected: {clientId}");
    }

    private void toggleHostScreen()
    {
        initialScreen.SetActive(false);
        joinScreen.SetActive(false);
        hostScreen.SetActive(true);
    }

    private void toggleJoinScreen()
    {
        initialScreen.SetActive(false);
        hostScreen.SetActive(false);
        joinScreen.SetActive(true);
    }

    public static void SetRoomId(string newRoomId)
    {
        roomId = newRoomId;
    }

    public static void StartNewGame()
    {
        OnStartGame?.Invoke();
    }
}