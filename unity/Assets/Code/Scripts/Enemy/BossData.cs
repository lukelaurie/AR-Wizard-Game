using System;
using Unity.Netcode;
using UnityEngine;

public class BossData : NetworkBehaviour
{

    private int bossLevel; 
    private string bossName;
    private NetworkVariable<float> bossHealth = new NetworkVariable<float>();
    private int maxHealth;
    [SerializeField] HealthBar healthBar;

    public void InitializeBossData(int level, int health)
    {
        bossLevel = level; 
        maxHealth = health; 
        bossHealth.Value = health; 

        SelectRandomBoss();
        UpdateHealthServerRpc(maxHealth);
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
        bossHealth.Value -= damageAmt;
        UpdateHealthServerRpc(bossHealth.Value);
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateHealthServerRpc(float newHealth)
    {
        bossHealth.Value = newHealth;
    }

    private void SelectRandomBoss()
    {
        string[] bosses = {"hydra", "basilisk"};

        // select a random string from the list of bosses 
        System.Random random = new System.Random();
        int randIndex = random.Next(bosses.Length);

        bossName = bosses[randIndex];
    }
}
