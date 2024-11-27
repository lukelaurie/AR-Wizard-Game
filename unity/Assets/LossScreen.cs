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
    void Start()
    {
        
        tryAgain.onClick.AddListener(TryAgain);
        quit.onClick.AddListener(SwapScreens.Instance.QuitGame);
    }

    private void TryAgain()
    {
        var clientData = GameObject.FindWithTag("playerInfo").GetComponent<PlayerData>();

        if (clientData.IsPlayerHost())
        {
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
