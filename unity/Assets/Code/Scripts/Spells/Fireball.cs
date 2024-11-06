using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    //public Transform target;
    //public float speed = 5f;
    public float damageAmount = 10f;
    public float lifetime = 3f;

    void Start()
    {
        //destroy object after certain period of time so it doesnt take up space in the scene
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        //Vector3 direction = (target.position - transform.position).normalized;
        //transform.position += direction * speed * Time.deltaTime;
        //this.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 0))
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            // Check if the object collided with a specific tag
            Destroy(gameObject);
            Debug.Log("fireball destroyed");

            //add explosion particle
            GameObject colidedObject = collision.gameObject;
            //Debug.log(colidedObject.transform.parent);
            //GameObject parentObject = colidedObject.transform.parent.gameObject;
            if (colidedObject.TryGetComponent<Enemy>(out Enemy enemyComponent))
            {
                enemyComponent.TakeDamage(damageAmount);
                Debug.Log($"fireball delt {damageAmount} damage to enemy");
                // Handle collision logic here
            }
            //tell enemies to take damage
        }
    }
}
