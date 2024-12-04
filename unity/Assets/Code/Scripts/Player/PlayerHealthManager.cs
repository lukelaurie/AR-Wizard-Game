using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    private PlayerData playerData;
    private NotifyServer server;

    void Start()
    {
        playerData = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<PlayerData>();
        server = GameObject.FindWithTag(TagManager.GameLogic).GetComponent<NotifyServer>();
    }

    void OnEnable()
    {
        playerData = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<PlayerData>();

        playerData.OnPlayerTakeDamage += HandlePlayerDamage;
        playerData.OnPlayerHealed += HandlePlayerHealed;
    }

    void OnDisable()
    {
        playerData.OnPlayerTakeDamage -= HandlePlayerDamage;
        playerData.OnPlayerHealed -= HandlePlayerHealed;
    }

    public void HandlePlayerDamage(float currentHealth)
    {
        if (playerData.IsGameOver())
            return;
            
        server.NotifyClientHealthServerRpc(playerData.GetUsername(), currentHealth);

        if (currentHealth <= 0 && !playerData.IsPlayerDead())
        {
            playerData.SetIsPlayerDead(true);

            // set the current play to the state of being dead
            FindObjectOfType<AudioManager>().Play("PlayerDie");
            SwapScreens.Instance.ToggleDeathScreen();

            server.NotifyClientDeathServerRpc();
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("PlayerTakeDamage");
        }
    }

    public void HandlePlayerHealed(float currentHealth)
    {
        server.NotifyClientHealthServerRpc(playerData.GetUsername(), currentHealth);
        FindObjectOfType<AudioManager>().Play("PlayerHeal");
    }

}
