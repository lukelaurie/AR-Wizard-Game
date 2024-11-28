using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    void Start()
    {
        PlayerData playerData = GameObject.FindWithTag("GameInfo").GetComponent<PlayerData>();
        Debug.Log(playerData + "    ??????");


        playerData.OnPlayerTakeDamage += HandlePlayerDamage;
        playerData.OnPlayerHealed += HandlePlayerHealed;
    }

    private void HandlePlayerDamage(int currentHealth)
    {
        Debug.Log($"Health updated: {currentHealth}");
    }

    private void HandlePlayerHealed(int currentHealth)
    {
        Debug.Log($"Health healed: {currentHealth}");
    }

}
