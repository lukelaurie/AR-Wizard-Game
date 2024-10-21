using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDragon : MonoBehaviour
{
    public int health;
    public int maxHealth = 1000;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if(health <= 0)
        {
            //play death animation
            Destroy(gameObject);
        }
    }
}
