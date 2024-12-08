using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    private float damageAmount;
    private float lifetime = 4f;
    [SerializeField] private GameObject explosion;


    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(TagManager.Player))
        {
            Destroy(gameObject);
            return;
        }

        PlayerData playerData = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<PlayerData>();
        playerData.PlayerTakeDamage(damageAmount);
        if (explosion != null)
            Instantiate(explosion, collision.contacts[0].point, Quaternion.identity);
            
        Destroy(gameObject);
    }


    public void SetDamage(float newDamage)
    {
        damageAmount = newDamage;
    }
}