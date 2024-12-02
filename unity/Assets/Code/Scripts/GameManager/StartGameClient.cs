using System;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class StartGameClient : MonoBehaviour
{
    [SerializeField] private Button homeButton;

    [SerializeField] private TMPro.TMP_Text invalidIdText;
    [SerializeField] private TMPro.TMP_InputField roomInput;
    [SerializeField] private Button continueServerButton;
    private bool isInGame;
    private PlayerData playerData;

    public static event Action OnJoinSharedSpaceClient;

    void Start()
    {
        continueServerButton.onClick.AddListener(JoinRoom);
        homeButton.onClick.AddListener(LeaveGame);

        playerData = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<PlayerData>();
        isInGame = false;
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
        
        // have user join room and take the picture
        isInGame = true;
        StartGameAr.SetRoomId(roomId);
        SwapScreens.Instance.ToggleClientJoinGame();
        OnJoinSharedSpaceClient?.Invoke();

        NetworkManager.Singleton.StartClient();
        Debug.Log("Starting AR Client...");

        playerData.SetIsPlayerHost(false);
    }

    private async void LeaveGame()
    {
        if (isInGame)
            await RoomManager.Instance.LeaveGame();
        
        SwapScreens.Instance.QuitGame();
    }
}
