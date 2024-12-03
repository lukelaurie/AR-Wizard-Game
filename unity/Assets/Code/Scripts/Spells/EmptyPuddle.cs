using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyPuddle : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 4f);
    }
}
