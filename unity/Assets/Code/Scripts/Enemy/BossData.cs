using System;
using Unity.Netcode;
using UnityEngine;

public class BossData : NetworkBehaviour
{

    private int bossLevel;
    private NetworkVariable<float> bossHealth = new NetworkVariable<float>();
    private NetworkVariable<float> maxHealth = new NetworkVariable<float>();

    [SerializeField] HealthBar healthBar;

    void Update()
    {
        healthBar.UpdateHealthBar(bossHealth.Value, maxHealth.Value);
    }

    public void InitializeBossData(int level)
    {
        bossLevel = level;
        // have the boss health scale with level of the boss
        float startHealth = (float)(200f * Math.Pow(2, (double)level - 1));

        UpdateHealthServerRpc(startHealth);
        SetMaxHealthServerRpc(startHealth);
    }

    public int GetBossLevel()
    {
        return bossLevel;
    }

    public float GetBossHealth()
    {
        return bossHealth.Value;
    }


    public void BossTakeDamage(float damageAmt)
    {
        UpdateHealthServerRpc(bossHealth.Value - damageAmt);
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateHealthServerRpc(float newHealth)
    {
        bossHealth.Value = newHealth;

        PlayerData playerData = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<PlayerData>();
        if (!playerData.IsGameOver() && bossHealth.Value <= 0)
        {
            playerData.SetIsGameOver();
            AllClientsInvoker.Instance.InvokePartyWinGameAllClients();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetMaxHealthServerRpc(float newMaxHealth)
    {
        maxHealth.Value = newMaxHealth;
    }
}
