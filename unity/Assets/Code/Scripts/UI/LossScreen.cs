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

    // Start is called before the first frame update
    async void Start()
    {

        tryAgain.onClick.AddListener(TryAgain);
        quit.onClick.AddListener(EndGame);
    }

    private async void EndGame()
    {
        BossData bossData = GameObject.FindWithTag("GameInfo").GetComponent<BossData>();
        await RoomManager.Instance.EndGame("hydra", false, 1);
        SwapScreens.Instance.QuitGame();
    }

    private void TryAgain()
    {
        var clientData = GameObject.FindWithTag("GameInfo").GetComponent<PlayerData>();

        if (clientData.IsPlayerHost())
        {
            // destroy the dragon that survived 
            GameObject dragon = GameObject.FindWithTag("Dragon");
            if (dragon != null)
            {
                Destroy(dragon);
            }
            
            // add the logic to restore player hp and remove boss object...

            SwapScreens.Instance.ToggleHostScreen();
        }
        else
        {
            SwapScreens.Instance.ToggleJoinScreen();
        }

        gameObject.SetActive(false);
    }
}
