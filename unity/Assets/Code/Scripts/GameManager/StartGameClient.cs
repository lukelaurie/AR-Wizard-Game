using System;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class StartGameClient : MonoBehaviour
{
    [SerializeField] private GameObject initialScreen;
    [SerializeField] private GameObject pictureScreen;

    [SerializeField] private TMPro.TMP_Text invalidIdText;
    [SerializeField] private TMPro.TMP_InputField roomInput;
    [SerializeField] private Button continueServerButton;

    public static event Action OnJoinSharedSpaceClient;

    void Start()
    {
        continueServerButton.onClick.AddListener(JoinRoom);
    }

    private async void JoinRoom()
    {
        string roomId = roomInput.text;

        // attempt to join the room in the go server 
        if (!await RoomManager.Instance.JoinRoom(roomId))
        {
            invalidIdText.text = "Invalid Room ID";
            await Task.Delay(2000); // wait for 2 seconds
            invalidIdText.text = "";

            return;
        }

        StartGameAr.SetRoomId(roomId);

        SwapScreens();

        OnJoinSharedSpaceClient?.Invoke();

        // join the game as a new player
        // StartGameAr.StartNewGame();

        NetworkManager.Singleton.StartClient();
        Debug.Log("Starting AR Client...");

        var clientData = GameObject.FindWithTag("playerInfo").GetComponent<PlayerData>();
        clientData.SetIsPlayerHost(false);
    }

    private void SwapScreens()
    {
        initialScreen.SetActive(false);
        pictureScreen.SetActive(true);
    }
}
