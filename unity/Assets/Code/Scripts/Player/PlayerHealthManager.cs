using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    private PlayerData playerData;

    void Start()
    {
        playerData = GameObject.FindWithTag("GameInfo").GetComponent<PlayerData>();
    }

    void OnEnable()
    {
        playerData = GameObject.FindWithTag("GameInfo").GetComponent<PlayerData>();

        playerData.OnPlayerTakeDamage += HandlePlayerDamage;
        playerData.OnPlayerHealed += HandlePlayerHealed;
    }

    void OnDisable()
    {
        playerData.OnPlayerTakeDamage -= HandlePlayerDamage;
        playerData.OnPlayerHealed -= HandlePlayerHealed;
    }

    public void HandlePlayerDamage(int currentHealth)
    {
        if (currentHealth <= 0)
        {
            playerData.SetIsPlayerDead(true);

            // set the current play to the state of being dead
            FindObjectOfType<AudioManager>().Play("PlayerDie");
            ScreenToggle.ToggleGameObjectWithTag(false, "InitiateSpellUi");
            ScreenToggle.ToggleGameObjectWithTag(true, "DeathBackground");
            
            var server = GameObject.FindWithTag("GameLogic").GetComponent<NotifyServer>();
            server.NotifyClientDeathServerRpc();


        }
        else
        {
            FindObjectOfType<AudioManager>().Play("PlayerTakeDamage");
        }
    }

    public void HandlePlayerHealed(int currentHealth)
    {
        FindObjectOfType<AudioManager>().Play("PlayerHeal");
    }

}
