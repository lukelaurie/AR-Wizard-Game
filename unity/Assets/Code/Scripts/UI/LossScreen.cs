using System.Collections;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LossScreen : NetworkBehaviour
{
    [SerializeField] private Button tryAgain;
    [SerializeField] private Button quit;
    private PlaceCharacter placeBoss;

    private PlayerData playerData;

    // Start is called before the first frame update
    async void Start()
    {
        playerData = GameObject.FindWithTag("GameInfo").GetComponent<PlayerData>();
        placeBoss = GameObject.FindWithTag("GameLogic").GetComponent<PlaceCharacter>();

        tryAgain.onClick.AddListener(TryAgain);
        quit.onClick.AddListener(EndGame);
    }

    private async void EndGame()
    {
        if (playerData.IsPlayerHost())
        {
            BossData bossData = GameObject.FindWithTag("GameInfo").GetComponent<BossData>();
            await RoomManager.Instance.EndGame("hydra", false, 1);
        }
        else
        {
            await RoomManager.Instance.LeaveGame();
    }
        SwapScreens.Instance.QuitGame();
    }

    private void TryAgain()
    {
        placeBoss.ResetIsPlaced();

        var server = GameObject.FindWithTag("GameLogic").GetComponent<NotifyServer>();
        server.ResetClientDeaths();

        if (playerData.IsPlayerHost())
        {
            AllClientsInvoker.Instance.InvokePlayerRestartAllClients();

            // destroy the dragon that survived 
            GameObject dragon = GameObject.FindWithTag("Dragon");
            if (dragon != null)
            {
                Destroy(dragon);
            }

            // add the logic to restore player hp and remove boss object...

            SwapScreens.Instance.ToggleHostScreen();
            placeBoss.enabled = false;
        }

        gameObject.SetActive(false);
    }
}
