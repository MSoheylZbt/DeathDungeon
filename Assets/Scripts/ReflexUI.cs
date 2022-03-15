using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReflexUI : MonoBehaviour
{
    Slider slider;
    TextMeshProUGUI textMesh;
    Image image;

    public void Init()
    {
        slider = GetComponent<Slider>();
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        image = slider.fillRect.gameObject.GetComponent<Image>();
    }

    public void SetSliderMaxValue(float length)
    {
        slider.maxValue = length;
    }

    public void SetSliderFiller(float amount)
    {
        slider.value = amount;
    }

    public void SetSliderText(string txt)
    {
        textMesh.text = "x" + txt;
    }


    public void SetSliderColor(Color color)
    {
        image.color = color;
    }
}
