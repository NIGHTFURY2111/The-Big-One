using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;   

public class ContentSliderSetup : MonoBehaviour
{
    TextMeshProUGUI slidername, value;


    FloatValue vla;
    Slider slider;
    float maxVal, minVal, currentVal, basevar;
    private void Awake()
    {
        slidername = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        value = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        slider = GetComponentInChildren<Slider>();
    }

    private void Start()
    {
    }
    private void Update()
    {
        updateSliderValue(slider.value);
        value.text = (slider.value).ToString();
    }

    public void SetupBlock( string name, float currentVal, float minVal, float maxVal , FloatValue val)
    {
        slidername.text = name;
        value.text = currentVal.ToString();
        slider.maxValue = maxVal;
        slider.minValue = minVal;
        slider.value = 1f;
        basevar = currentVal;
        vla = val;
    }

    public void updateSliderValue(float val)
    {
        vla.value = val;
    }
}
