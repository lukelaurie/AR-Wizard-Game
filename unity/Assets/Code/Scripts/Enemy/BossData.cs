using System;
using UnityEngine;

public class BossData : MonoBehaviour
{
    private int bossLevel; 
    private string bossName; 


    public int GetBossLevel()
    {
        return bossLevel;
    }

    public string GetBossName()
    {
        return bossName;
    }

    public void SetBossLevel(int bossDifficulty)
    {
        bossLevel = bossDifficulty;
    }

    public void SelectRandomBoss()
    {
        string[] bosses = {"hydra", "basilisk"};

        // select a random string from the list of bosses 
        System.Random random = new System.Random();
        int randIndex = random.Next(bosses.Length);

        bossName = bosses[randIndex];
    }
}
