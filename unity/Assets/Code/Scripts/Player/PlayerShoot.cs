using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject fireball;
    public float speed = 8f;
    public float spawnDist = 2f;
    public Camera camera;

    private float waitTime = 1f;
    private float timer = 0.0f;

    void Start()
    {
        //empty for now
    }

    void Update()
    {
        timer += Time.deltaTime;

        //must wait a second between shooting
        if (Input.GetMouseButtonDown(0) && timer >= waitTime)
        {
            ShootFireball();
            timer = 0;
        }
    }

    void ShootFireball()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        Ray ray = camera.ScreenPointToRay(mouseScreenPos);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, Mathf.Infinity);
        Vector3 spawnPos = camera.transform.position + camera.transform.forward * spawnDist;

        GameObject projectile = Instantiate(fireball, spawnPos, Quaternion.LookRotation(ray.direction));
        projectile.GetComponent<Rigidbody>().velocity = ray.direction * speed;
    }
}
