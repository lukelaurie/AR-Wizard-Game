using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject fireball;
    public float speed = 8f;
    public float spawnDist = 1f;
    public Camera camera;

    //public Transform targetPosition;

    void Start()
    {
        //quick fix to stop the fireballs from coliding with the player
        //gameObject.GetComponent<Collider>().enabled = false;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ShootFireball();
        }
    }

    void ShootFireball()
    {
        
        // Get the mouse position on the screen (2D coordinates)
        Vector3 mouseScreenPos = Input.mousePosition;

        // Convert the mouse position to a ray in the world space
        Ray ray = camera.ScreenPointToRay(mouseScreenPos);

        // The point in world space where the ray hits (we assume a flat plane at spawnDistance distance)
        RaycastHit hit;
        Physics.Raycast(ray, out hit, Mathf.Infinity);
        Vector3 spawnPos = transform.position + transform.forward * spawnDist;
        // Spawn the object at the point where the ray hit, with the forward direction based on the camera
        GameObject projectile = Instantiate(fireball, spawnPos, Quaternion.LookRotation(ray.direction));
        projectile.GetComponent<Rigidbody>().velocity = ray.direction * speed;


        /*
        //calculate the spawn position
        Vector3 spawnPos = transform.position + transform.forward * spawnDist;

        //spawn fireball
        GameObject projectile = Instantiate(fireball, spawnPos, transform.rotation);

        //add force
        projectile.GetComponent<Rigidbody>().velocity = transform.forward * speed;
        




        /* optional code to add force in the direction of where the screen was tapped:

        //get the tap position
        Vector3 tapPosition = Input.mousePosition;
        //tapPosition.z = transform.position.z - Camera.main.transform.position.z;
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(tapPosition);

        //calc direction from player to target
        Vector3 direction = (targetPosition - transform.position).normalized;

        //spawn fireball
        GameObject projectile = Instantiate(fireball, transform.position, Quaternion.identity);

        //apply force in direction
        projectile.GetComponent<Rigidbody>().velocity = direction * speed;
        */
    }
}
