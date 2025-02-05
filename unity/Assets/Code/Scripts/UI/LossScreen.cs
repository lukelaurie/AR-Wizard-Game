using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LossScreen : MonoBehaviour
{
    [SerializeField] private Button tryAgain;
    [SerializeField] private Button quit;
    private PlaceCharacter placeBoss;

    private PlayerData playerData;

    private Enemy enemy;
    // Start is called before the first frame update
    void OnEnable()
    {
        playerData = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<PlayerData>();
        placeBoss = GameObject.FindWithTag(TagManager.GameLogic).GetComponent<PlaceCharacter>();
        enemy = ScreenToggle.FindGameObjectWithTag(TagManager.BossParent).GetComponent<Enemy>();

        playerData.SetIsGameOver();
        tryAgain.onClick.AddListener(TryAgain);
        quit.onClick.AddListener(EndGame);
        enemy.StopBossMusic();
    }

    private async void EndGame()
    {
        if (playerData.IsPlayerHost())
        {
            await RoomManager.Instance.EndGame(false, 1);
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

        var server = GameObject.FindWithTag(TagManager.GameLogic).GetComponent<NotifyServer>();
        server.ResetClientDeaths();

        if (playerData.IsPlayerHost())
        {            
            AllClientsInvoker.Instance.InvokePlayerRestartAllClients();

            SwapScreens.Instance.ToggleHostScreen();
            placeBoss.enabled = false;
        }

        gameObject.SetActive(false);
    }
}
