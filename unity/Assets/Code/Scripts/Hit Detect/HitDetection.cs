using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitDetection : MonoBehaviour
{
    public GameObject hitObj;
    public Vector3 collision = Vector3.zero;
    public Camera camera;
    public Color newColor = Color.green;
    public Button fireButton;



    // Start is called before the first frame update
    void Start()
    {
        if (camera == null)
        {
            camera = GameObject.FindObjectOfType<Camera>();
        }

        fireButton.onClick.AddListener(() =>
        {
            Debug.Log("button click");
            Fire();
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("right click");
            Fire();
        }
    }

    private void Fire()
    {
        if (camera != null) // Ensure the camera reference is valid
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                hitObj = hit.transform.gameObject;

                if (hitObj.CompareTag("Dragon"))
                {
                    Debug.Log("hit enemy");
                    Enemy enemyScript = hitObj.GetComponent<Enemy>();

                    // Check if the component exists to avoid null reference errors
                    if (enemyScript != null)
                    {
                        Debug.Log("in this if statement!!!!!");
                        // Call the TakeDamage method from the Enemy script
                        enemyScript.TakeDamage(50);
                    }
                    else
                    {
                        Debug.LogError("Enemy script not found on the object.");
                    }
                }

                collision = hit.point;
                Renderer renderer = hitObj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = newColor;
                }
                Debug.Log("Hit object: " + hitObj.name);
            }
        }
    }
}
