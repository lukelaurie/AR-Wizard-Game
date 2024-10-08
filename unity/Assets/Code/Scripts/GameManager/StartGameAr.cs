// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Niantic.Lightship.SharedAR.Colocalization;
// using Unity.Netcode;
// using UnityEngine;
// using UnityEngine.UI;

// public class StartGameAr : MonoBehaviour
// {
//     [SerializeField] private SharedSpaceManager sharedSpaceManager;
//     const int MAX_AMOUNT_CLIENTS_ROOM = 2;

//     [SerializeField] private Texture2D targetImage;
//     [SerializeField] private float targetImageSize;
//     private string roomName = "TestRoom";

//     [SerializeField] private Button startGameButton;
//     [SerializeField] private Button createRoomButton;
//     [SerializeField] private Button joinRoomButton;
//     private bool isHost;

//     public static event Action OnStartSharedSpaceHost;
//     public static event Action OnJoinSharedSpaceClient;
//     public static event Action OnStartGame;
//     public static event Action OnStartSharedSpace;

//     private void Awake()
//     {
//         DontDestroyOnLoad(gameObject);
//         sharedSpaceManager.sharedSpaceManagerStateChanged += SharedSpaceManagerOnSharedSpaceManagerStateChange;

//         startGameButton.onClick.AddListener(StartGame);
//         createRoomButton.onClick.AddListener(CreateGameHost);
//         joinRoomButton.onClick.AddListener(JoinGameClient);

//         startGameButton.interactable = false;
//     }

//     private void SharedSpaceManagerOnSharedSpaceManagerStateChange(SharedSpaceManager.SharedSpaceManagerStateChangeEventArgs obj)
//     {
//         if (obj.Tracking)
//         {
//             startGameButton.interactable = true;
//             createRoomButton.interactable = false; 
//             joinRoomButton.interactable = false;
//         }
//     }

//     void StartGame()
//     {
//         onStartGame?.Invoke();
//         if (isHost)
//         {
//             NetworkManager.Singleton.StartHost();
//         }
//         else 
//         {
//             NetworkManager.Singleton.StartClient();
//         }
//     }

//     void StartSharedSpace() {

//     }

//     void StartSharedSpace() {

//     }

//     void CreateGameHost()
//     {
//         isHost = true;
//         onStartSharedSpaceHost?.Invoke();
//         StartSharedSpace();
//     }

//     void JoinGameClient()
//     {
//         isHost = false;
//         onJoinSharedSpaceClient?.Invoke();
//         JoinSharedSpace();
//     }
// }
