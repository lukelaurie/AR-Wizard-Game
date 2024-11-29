using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar: NetworkBehaviour
{
    [SerializeField] private Slider slider;

    public void UpdateHealthBar(float curVal, float maxValue)
    {
        slider.value = curVal / maxValue;
    }
}
