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
        float startHealth = 1 * level;

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
        Debug.Log(bossHealth.Value);
        bossHealth.Value = newHealth;
        Debug.Log(newHealth);

        if (bossHealth.Value <= 0)
        {
            AllClientsInvoker.Instance.InvokePartyWinGameAllClients();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetMaxHealthServerRpc(float newMaxHealth)
    {
        maxHealth.Value = newMaxHealth;
    }
}
