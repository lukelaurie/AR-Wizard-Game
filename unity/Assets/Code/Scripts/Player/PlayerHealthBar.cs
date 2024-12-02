using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMPro.TMP_Text playerName;

    public void UpdateHealthBar(float curVal, float maxValue)
    {
        slider.value = curVal / maxValue;
    }

    public void SetPlayerName(string name)
    {
        playerName.text = name;
    }

    public void SetSliderDimensions(int width, int height)
    {
        // set the width and height of the health bars 
        RectTransform rectTransform = slider.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(width, height);
    }
}
