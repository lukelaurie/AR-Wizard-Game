using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public float mouseSensitivity = 100f; // Adjust to set how fast the player rotates
    public Transform playerBody;          // Reference to the player's body to rotate it
    public Transform playerCamera;        // Reference to the player's camera

    private float xRotation = 0f;         // To keep track of the vertical rotation

    void Start()
    {
        // Lock the cursor to the center of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Get the mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate the player horizontally (Y-axis)
        playerBody.Rotate(Vector3.up * mouseX);

        // Rotate the camera vertically (X-axis)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp to avoid flipping

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
