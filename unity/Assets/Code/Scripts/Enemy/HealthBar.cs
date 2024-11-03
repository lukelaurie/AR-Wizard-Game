using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    //[SerializeField] private Slider camera;
    //[SerializeField] private Slider target;
    //commented out stuff is for showing slider towards player, might remove

    public void UpdateHealthBar(float curVal, float maxValue)
    {
        slider.value = curVal / maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        //might need to rework for multiple players
        //also not sure how camera is setup for AR
        //transform.rotation = camera.transform.rotation;
    }
}
