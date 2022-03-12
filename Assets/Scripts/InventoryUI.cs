using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI blueTxt;
    [SerializeField] TextMeshProUGUI redTxt;
    [SerializeField] TextMeshProUGUI greenTxt;
    [SerializeField] TextMeshProUGUI upgradeTxt;

    [SerializeField] Knight_Data data;

    private void OnEnable()
    {
        Knight_Data.OnSetGreenPotion += SetGreenPotionTxt;
        Knight_Data.OnSetHealthPotion += SetRedPotionTxt;
        Knight_Data.OnSetInvisPotion += SetBluePotionTxt;
        Knight_Data.OnSetUpgrade += SetUpgradeTxt;
    }

    public void SetBluePotionTxt()
    {
        blueTxt.text = data.InvisPotionCount.ToString();
    }

    public void SetRedPotionTxt()
    {
        redTxt.text = data.HealthPotionCount.ToString();
    }

    public void SetGreenPotionTxt()
    {
        greenTxt.text = data.GreePotionCount.ToString();
    }

    public void SetUpgradeTxt()
    {
        upgradeTxt.text = data.UpgradeLevel.ToString();
    }

    private void OnDisable()
    {
        Knight_Data.OnSetGreenPotion -= SetGreenPotionTxt;
        Knight_Data.OnSetHealthPotion -= SetRedPotionTxt;
        Knight_Data.OnSetInvisPotion -= SetBluePotionTxt;
        Knight_Data.OnSetUpgrade -= SetUpgradeTxt;

    }
}
