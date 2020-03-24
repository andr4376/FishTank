using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeFishScript : MonoBehaviour
{
    private TextMeshProUGUI priceText;
    private TextMeshProUGUI currentUpgradeModifierText;

    [SerializeField]
    private FISH fishType;

    private readonly float upgradePriceIncrease = 5f;
    private readonly float chromisBasePrice = 25;
    private readonly float eelBasePrice = 50;
    private readonly float molaBasePrice = 250;

    private float myBasePrice;
    private float myCurrentModifier;




    private int Price
    {
        get
        {
            return (int)(myBasePrice * myCurrentModifier
                * 1 + upgradePriceIncrease);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

        switch (fishType)
        {
            case FISH.CHROMIE:
                myBasePrice = chromisBasePrice;
                break;
            case FISH.EEL:
                myBasePrice = eelBasePrice;
                break;
            case FISH.MOLA:
                myBasePrice = molaBasePrice;
                break;

            default:
                break;
        }

        

        //find textmeshpros in case they're not set in the unity editor
        foreach (Transform child in transform)
        {
            if (priceText == null)
            {
                if (child.name == "Price")
                {
                    priceText = child.GetComponent<TextMeshProUGUI>();

                    continue;
                }
            }
            if (currentUpgradeModifierText == null)
            {

                if (child.name == "CurrentUpgrade")
                {
                    currentUpgradeModifierText = child.GetComponent<TextMeshProUGUI>();
                    continue;
                }
            }
        }

        UpdateText();

    }
    public void Buy()
    {


        if (ScoreManager.Score >= Price)
        {
            

            Upgrades.PointModifiers[fishType] +=  0.1f;

            ScoreManager.Score -= Price;

            UpdateText();


            Debug.Log(fishType.ToString() + " upgrade bought!");
        }
        else
        {
            Debug.Log("not enough money");

            // SoundManager.PlayAudio(SOUNDS.INVALID_INPUT, 0.3f);
        }
    }


    private void UpdateText()
    {
        string _priceText = "$";

        myCurrentModifier = Upgrades.PointModifiers[fishType];


        _priceText += Mathf.Round(Price).ToString("0");

        priceText.text = _priceText;

        currentUpgradeModifierText.text =  ((int)(myCurrentModifier*10)).ToString();

    }


}