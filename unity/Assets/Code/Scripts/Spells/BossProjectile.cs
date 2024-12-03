using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    private float damageAmount;
    private float lifetime = 4f;
    [SerializeField] private GameObject puddle;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("here");
        if (!collision.gameObject.CompareTag(TagManager.Player))
        {
            return;
        }
        Debug.Log("here1");
        var networkObject = collision.gameObject.GetComponent<NetworkObject>();
        if (networkObject.IsOwner)
        {
            Debug.Log("here2");
            PlayerData playerData = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<PlayerData>();
            playerData.PlayerTakeDamage(25); //TODO
            Instantiate(puddle, collision.contacts[0].point, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void SetDamage(float newDamage)
    {
        damageAmount = newDamage;
    }
}