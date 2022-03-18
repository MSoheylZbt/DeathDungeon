using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; //TextMeshPro Library : used for high quality texts.

public class ReflexUI : MonoBehaviour
{
    Slider slider; // Slider is a circle in UI that indicates timer to the player.
    TextMeshProUGUI textMesh;
    Image image;

    public void Init()
    {
        slider = GetComponent<Slider>();
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        image = slider.fillRect.gameObject.GetComponent<Image>();
    }

    public void SetSliderMaxValue(float length) // Total length of timer = Slider Max Value
    {
        slider.maxValue = length;
    }

    public void SetSliderFiller(float amount)
    {
        slider.value = amount;
    }

    public void SetSliderText(string txt) // This text is used for showing strike counts.
    {
        textMesh.text = "x" + txt;
    }


    public void SetSliderColor(Color color)
    {
        image.color = color;
    }
}
