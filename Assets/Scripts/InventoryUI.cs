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
    [SerializeField] Sounds_data sounds;

    Animator animator;
    AudioSource audioSource;

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        Knight_Data.OnSetGreenPotion += SetGreenPotionTxt;
        Knight_Data.OnSetHealthPotion += SetRedPotionTxt;
        Knight_Data.OnSetInvisPotion += SetBluePotionTxt;
        Knight_Data.OnSetUpgrade += SetUpgradeTxt;
    }

    public void SetBluePotionTxt(bool isUsed)
    {
        blueTxt.text = data.InvisPotionCount.ToString();
        if(isUsed)
        {
            animator.SetTrigger("Blue");
            audioSource.PlayOneShot(sounds.potionSound, sounds.potionAmount);
        }
    }

    public void SetRedPotionTxt(bool isUsed)
    {
        redTxt.text = data.HealthPotionCount.ToString();
        if (isUsed)
        {
            animator.SetTrigger("Red");
            audioSource.PlayOneShot(sounds.potionSound, sounds.potionAmount);
        }
    }

    public void SetGreenPotionTxt(bool isUsed)
    {
        greenTxt.text = data.GreePotionCount.ToString();
        if (isUsed)
        {
            animator.SetTrigger("Green");
            audioSource.PlayOneShot(sounds.potionSound, sounds.potionAmount);
        }
    }

    public void SetUpgradeTxt(bool isUsed) // No need for bool => but should match signature
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
