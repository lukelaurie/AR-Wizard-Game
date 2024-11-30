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
        if (!collision.gameObject.CompareTag(TagManager.Boss))
        {
            return;
        }

        Transform collidedObjectTransform = collision.transform;

        //add explosion particle
        Instantiate(explosion, collision.contacts[0].point, Quaternion.identity);

        GameObject parentObject = collision.gameObject;

        if (collidedObjectTransform.parent == null)
        {
            Debug.Log("colided object parent is null");
        }
        else
        {
            parentObject = collidedObjectTransform.parent.gameObject;
        }

        if (parentObject.TryGetComponent<Enemy>(out Enemy enemyComponent))
        {
            enemyComponent.TakeDamage(damageAmount);
            Debug.Log($"fireball delt {damageAmount} damage to enemy");
        }

        if (collision.gameObject.TryGetComponent<PlayerScript>(out PlayerScript playerComponent))
        {
            playerComponent.TakeDamage(damageAmount);
            Debug.Log($"fireball delt {damageAmount} damage to player");
        }

        Debug.Log("fireball destroyed");
        Destroy(gameObject);

    }

    // Helper method to get the full hierarchy path of the object
    private string GetFullPath(Transform transform)
    {
        string path = transform.name;
        while (transform.parent != null)
        {
            transform = transform.parent;
            path = transform.name + "/" + path;
        }
        return path;
    }

}
