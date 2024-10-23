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
        if (camera != null) // Ensure the camera reference is valid
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                hitObj = hit.transform.gameObject;
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
