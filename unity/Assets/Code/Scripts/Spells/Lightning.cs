using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    //public Transform target;
    //public float speed = 5f;
    public float damageAmount = 1f;

    void Start()
    {
        /*
        Collider playerCollider = ...; // Reference to the player's collider
        Collider objectCollider = ...; // Reference to the other object's collider

        Physics.IgnoreCollision(playerCollider, objectCollider); */
        //destroy object instantly because it gets continually spawned
        Destroy(gameObject, 0.2f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            Debug.Log("lightning hit guy");

            GameObject colidedObject = collision.gameObject;

            if (colidedObject.TryGetComponent<Enemy>(out Enemy enemyComponent))
            {
                enemyComponent.TakeDamage(damageAmount);
                Debug.Log($"lightning delt {damageAmount} damage to enemy");
            }
        }
    }
}