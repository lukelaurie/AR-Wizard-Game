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
        isInGame = true;
        StartGameAr.SetRoomId(roomId);

        StartGameAr startGameAr = GameObject.FindWithTag(TagManager.Scenary).GetComponent<StartGameAr>();
        startGameAr.BlitImageForColocalizationOnTextureRender();

        Debug.Log("Starting AR Client...");
        NetworkManager.Singleton.StartClient();
        
        SwapScreens.Instance.ToggleClientJoinGame();

        playerData.SetIsPlayerHost(false);

    }

    private async void LeaveGame()
    {
        if (isInGame)
            await RoomManager.Instance.LeaveGame();

        SwapScreens.Instance.QuitGame();
    }
}
