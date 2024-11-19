using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class StartGameHost : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text roomIdText;
    [SerializeField] private Button startGameButton;
    
    public static event Action OnStartSharedSpaceHost;

    void Start()
    {        
        string roomId = RoomManager.Instance.CreateRoom();
        
        roomIdText.text = roomId;
        StartGameAr.SetRoomId(roomId);
        
        OnStartSharedSpaceHost?.Invoke();

        startGameButton.onClick.AddListener(startGameHost);
    }

    private void startGameHost()
    {
        StartGameAr.StartNewGame();

        Debug.Log("Starting The AR Dedicated Server...");
        NetworkManager.Singleton.StartHost();

        gameObject.SetActive(false);
    }
}
