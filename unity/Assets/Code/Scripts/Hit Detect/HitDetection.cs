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
        if (fireButton != null)
        {
            fireButton.onClick.AddListener(Fire);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Fire();
        }
    }

    private void Fire()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            hitObj = hit.transform.gameObject;

            if (hitObj.CompareTag(TagManager.Boss))
            {
                Enemy enemyScript = hitObj.GetComponent<Enemy>();

                // Check if the component exists to avoid null reference errors
                if (enemyScript != null)
                {
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
        }
    }
}
