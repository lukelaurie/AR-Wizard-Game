using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMPro.TMP_Text playerName;
    [SerializeField] private Transform fill;
    private float xScale = 1f;

    public void UpdateHealthBar(float curVal, float maxValue)
    {
        slider.value = curVal / maxValue;
        if (curVal <= 0)
        {
            Vector3 currentScale = fill.localScale;
            xScale = currentScale.x;
            fill.localScale = new Vector3(0.0f, currentScale.y, currentScale.z);
        }
        else
        {
            Vector3 currentScale = fill.localScale;
            fill.localScale = new Vector3(xScale, currentScale.y, currentScale.z);
        }
    }

    public void SetPlayerName(string name)
    {
        playerName.text = name;
    }
}
