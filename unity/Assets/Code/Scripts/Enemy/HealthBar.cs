using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar: NetworkBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private BossData bossData;
    [SerializeField] private Transform fill;
    private float xScale = 1f;

    public void UpdateHealthBar(float curVal, float maxValue)
    {
        int bossHealth = (int)System.Math.Ceiling(bossData.GetBossHealth());
        if(bossHealth < 0)
        {
            bossHealth = 0;
        }
        TMP_Text textField = gameObject.GetComponentInChildren<TMP_Text>();
        textField.text = $"{bossHealth} HP";

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
}
