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

    public void Init(float length)
    {
        slider = GetComponent<Slider>();
        slider.maxValue = length;
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        image = slider.fillRect.gameObject.GetComponent<Image>();
    }

    public void SetSliderFiller(float amount)
    {
        slider.value = amount;
    }

    public void SetSliderText(string txt)
    {
        textMesh.text = txt;
    }


    public void SetSliderColor(Color color)
    {
        image.color = color;
    }
}
