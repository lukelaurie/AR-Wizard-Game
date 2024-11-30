using System;
using Unity.Netcode;
using UnityEngine;

public class BossData : NetworkBehaviour
{

    private int bossLevel;
    private string bossName;
    private NetworkVariable<float> bossHealth = new NetworkVariable<float>();
    private NetworkVariable<float> maxHealth = new NetworkVariable<float>();

    [SerializeField] HealthBar healthBar;

    void Update()
    {
        healthBar.UpdateHealthBar(bossHealth.Value, maxHealth.Value + 20);
    }

    public void InitializeBossData(int level)
    {
        bossLevel = level;
        // have the boss health scale with level of the boss
        float startHealth = 20 * level;

        SelectRandomBoss();
        UpdateHealthServerRpc(startHealth);
        SetMaxHealthServerRpc(startHealth);
    }

    public int GetBossLevel()
    {
        return bossLevel;
    }

    public string GetBossName()
    {
        return bossName;
    }

    public float GetBossHealth()
    {
        return bossHealth.Value;
    }


    public void BossTakeDamage(float damageAmt)
    {
        UpdateHealthServerRpc(bossHealth.Value - damageAmt);
    }


    private void SelectRandomBoss()
    {
        string[] bosses = { "hydra", "basilisk" };

        // select a random string from the list of bosses 
        System.Random random = new System.Random();
        int randIndex = random.Next(bosses.Length);

        bossName = bosses[randIndex];
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateHealthServerRpc(float newHealth)
    {
        bossHealth.Value = newHealth;

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
