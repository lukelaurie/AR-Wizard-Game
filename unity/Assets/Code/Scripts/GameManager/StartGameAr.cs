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
    [SerializeField] private SharedSpaceManager sharedSpaceManager = null;
    [SerializeField] private Texture2D targetImage;
    [SerializeField] private float targetImageSize;

    [SerializeField] private Button joinServerButton;
    [SerializeField] private Button startServerButton;

    public static event Action OnStartGame;
    public static event Action OnStartSharedSpace;

    private bool isHost;

    private bool isRoomCreated;
    private static string roomId = "";

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;

        SetRandomUserId();

        joinServerButton.onClick.AddListener(SwapScreens.Instance.ToggleJoinScreen);
        startServerButton.onClick.AddListener(SwapScreens.Instance.ToggleHostScreen);

        BlitImageForColocalization.OnTextureRendered += BlitImageForColocalizationOnTextureRender;
    }

    private void OnDestroy()
    {
        // NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;
        BlitImageForColocalization.OnTextureRendered -= BlitImageForColocalizationOnTextureRender;
    }

    private void BlitImageForColocalizationOnTextureRender(Texture2D texture)
    {
        targetImage = texture;
        
        const int MAX_AMOUNT_CLIENTS_ROOM = 34;

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

    public static void SetRoomId(string newRoomId)
    {
        roomId = newRoomId;
    }

    public static void StartNewGame()
    {
        OnStartGame?.Invoke();
    }
}