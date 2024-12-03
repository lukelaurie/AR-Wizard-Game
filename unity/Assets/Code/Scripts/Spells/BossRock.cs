using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BossRock : MonoBehaviour
{
    private float damageAmount;
    private float lifetime = 4f;
    [SerializeField] private GameObject crumble;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(TagManager.Player))
        {
            return;
        }
        var networkObject = collision.gameObject.GetComponent<NetworkObject>();
        if (networkObject.IsOwner)
        {
            PlayerData playerData = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<PlayerData>();
            playerData.PlayerTakeDamage(25); //TODO
            Instantiate(crumble, collision.contacts[0].point, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void SetDamage(float newDamage)
    {
        damageAmount = newDamage;
    }
}
