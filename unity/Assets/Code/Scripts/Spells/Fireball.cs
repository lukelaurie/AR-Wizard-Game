using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float damageAmount = 10f;
    public float lifetime;
    public GameObject explosion;

    void Start()
    {
        //destroy object after certain period of time so it doesnt take up space in the scene
        lifetime = 4f;
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            Debug.Log("fireball destroyed");

            //add explosion particle
            Instantiate(explosion, collision.contacts[0].point, Quaternion.identity);

            GameObject colidedObject = collision.gameObject;

            if (colidedObject.TryGetComponent<Enemy>(out Enemy enemyComponent))
            {
                enemyComponent.TakeDamage(damageAmount);
                Debug.Log($"fireball delt {damageAmount} damage to enemy");
            }
            Destroy(gameObject);
        }
    }
}
