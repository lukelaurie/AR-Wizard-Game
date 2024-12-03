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

    private static string roomId = "";
    private PlayerData playerData;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        playerData = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<PlayerData>();
        SetRandomUserId();

        joinServerButton.onClick.AddListener(StartJoinScreen);
        startServerButton.onClick.AddListener(StartHostScreen);

        NetworkManager.Singleton.OnClientDisconnectCallback += OnDisconnectedFromHost;
    }

    private void StartJoinScreen()
    {
        playerData.SetIsPlayerHost(false);
        SwapScreens.Instance.ToggleTakePictureScreen();
    }

    private void StartHostScreen()
    {
        playerData.SetIsPlayerHost(true);
        SwapScreens.Instance.ToggleTakePictureScreen();
    }

    private void SetRandomUserId()
    {
        System.Random random = new System.Random();
        int randomNumber = random.Next();
        PrivacyData.SetUserId(randomNumber.ToString());
    }

    private void OnDisconnectedFromHost(ulong clientId)
    {
        if (NetworkManager.Singleton.LocalClientId == clientId)
        {
            try
            {
                SwapScreens.Instance.QuitGame();
            }
            catch
            {
                Debug.Log("Look! No error is thrown because it was caught and this printed instead!!!!");
            }

        }
    }

    public void BlitImageForColocalizationOnTextureRender()
    {
        targetImage = playerData.GetTargetImage();

        const int MAX_AMOUNT_CLIENTS_ROOM = 34;

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


    public static void SetRoomId(string newRoomId)
    {
        roomId = newRoomId;
    }

    public static void StartNewGame()
    {
        OnStartGame?.Invoke();
    }
}