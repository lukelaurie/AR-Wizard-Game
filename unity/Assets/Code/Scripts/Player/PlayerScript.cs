using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

//keeps track of health, moveSpeed, animations, and following
//used for dragon enemies,etc
public class PlayerScript : NetworkBehaviour
{
    //private NetworkVariable<float> health = new NetworkVariable<float>();
    public float health;
    private float maxHealth = 200f;

    //[SerializeField] HealthBar healthBar;
    //possibly add a healthbar to each player later

    [SerializeField] private AudioClip takeDamageSound;

    private void Awake()
    {
        //healthBar = GetComponentInChildren<HealthBar>();
    }

    void Start()
    {
        maxHealth = 200f;
        health = maxHealth;
        //UpdateHealthServerRpc(maxHealth);
    }

    void Update()
    {
        //healthBar.UpdateHealthBar(health.Value, maxHealth);
    }

    public void TakeDamage(float damageAmount)
    {
        Debug.Log("took damage within PlayerScript.cs");

        //UpdateHealthServerRpc(health.Value - damageAmount);
        health -= damageAmount;
        AudioSource.PlayClipAtPoint(takeDamageSound, transform.position, 1f);

        if (health <= 0)//(health.Value <= 0)
        {
            //TODO: PUT ALL OF THE LOSS INFORMATION HERE
            //PLAYER DIES HERE
            //!!!
            //!!!
            //!!!
            //PUT DEATH SCREEN AND SO ON

            //play for everyone
            //FindObjectOfType<AudioManager>().Play("SomeSortOfLoss");

            StartCoroutine(WaitAndDestroy());
        }
    }

    private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(3f);
        //FindObjectOfType<AudioManager>().Play("StartMusic");
        Destroy(gameObject);
    }

    /*[ServerRpc(RequireOwnership = false)]
    public void UpdateHealthServerRpc(float newHealth)
    {
        health.Value = newHealth;
    }*/

}