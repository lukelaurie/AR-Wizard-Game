using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaceCharacter : NetworkBehaviour
{
    [SerializeField] private GameObject placementObject;

    private bool isPlaced = false;
    private Camera mainCam;

    void Start()
    {
        if (mainCam == null)
        {
            mainCam = GameObject.FindObjectOfType<Camera>();
        }
    }

    public void HandlePlaceObjectUnity()
    {
        Debug.Log("here  " + isPlaced);
        if (isPlaced)
            return;

        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("UI Hit was recognized");
            return;
        }
        TouchToRay(Input.mousePosition);
    }

    public void HandlePlaceObjectIOS()
    {
        if (isPlaced)
            return;

        // Do raycasting to detect if any ui element was clicked on
        Touch touch = Input.GetTouch(0);

        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = touch.position;

        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0)
        {
            // We hit a UI element
            Debug.Log("We hit an UI Element");
            return;
        }

        Debug.Log("Touch detected, fingerId: " + touch.fingerId);  // Debugging line


        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
        {
            Debug.Log("Is Pointer Over GOJ, No placement ");
            return;
        }

        // use the location of where clicked to create an object
        TouchToRay(touch.position);
    }

    void TouchToRay(Vector3 touch)
    {
        Ray ray = mainCam.ScreenPointToRay(touch);
        RaycastHit hit;
        Debug.Log(1);

        // checks if the ray from the camera hit a physical game object
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(2);
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.tag != "Dragon" && hitObject.tag != "Fireball")
            {
                Debug.Log(3);
                // calculate rotation of the object relative to object location
                Quaternion rotation = Quaternion.Euler(0, 0, 0);
                Debug.Log("calling");
                SpawnPlayerServerRpc(hit.point, rotation, NetworkManager.Singleton.LocalClientId);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)] // saying that any client can execute this method
    void SpawnPlayerServerRpc(Vector3 positon, Quaternion rotation, ulong callerID)
    {
        Debug.Log("placing");
        GameObject boss = Instantiate(placementObject, positon, rotation);

        NetworkObject bossNetworkObject = boss.GetComponent<NetworkObject>();

        bossNetworkObject.SpawnWithOwnership(callerID);

        isPlaced = true;
    }

    public void ResetIsPlaced()
    {
        isPlaced = false;
    }
}
