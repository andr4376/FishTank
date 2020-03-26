using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class BuyFishScript : MonoBehaviour
{
    private TextMeshProUGUI priceText;
    private TextMeshProUGUI currentFishAmountText;

    [SerializeField]
    private FISH fishType;

    private readonly float fishPriceRiseModifier = 0.010f;
    private readonly float chromisBasePrice = 25;
    private readonly float eelBasePrice = 250;
    private readonly float molaBasePrice = 1000;

    private float myBasePrice;
    private float myPopulation;

    private bool initialized = false;


    private float Price
    {
        get
        {
            return (myBasePrice * myPopulation
                * 1 + fishPriceRiseModifier);
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

        //subscribe to on populationchange
        BoidsManager.onPopulationChanged += delegate ()
        {
            UpdateText();
        };

        //find textmeshpros in case they're not set in the unity editor
        foreach (Transform child in transform)
        {


            if (priceText == null)
            {
                foreach (Transform childChild in child)
                {

                    if (childChild.name == "Price")
                    {
                        priceText = childChild.GetComponent<TextMeshProUGUI>();

                        continue;
                    }
                }
            }
            if (currentFishAmountText == null)
            {

                if (child.name == "CurrentAmount")
                {
                    currentFishAmountText = child.GetComponent<TextMeshProUGUI>();
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
            BoidsManager.Spawn(fishType);

            ScoreManager.Score -= Price;


            Debug.Log(fishType.ToString() + " bought!");
        }
        else
        {
            Debug.Log("not enough money");

            // SoundManager.PlayAudio(SOUNDS.INVALID_INPUT, 0.3f);
        }
    }


    private void UpdateText()
    {
        string _priceText = "";
        switch (fishType)
        {
            case FISH.CHROMIE:
                myPopulation = BoidsManager.ChromisCount;
                break;
            case FISH.EEL:
                myPopulation = BoidsManager.EelCount;
                break;
            case FISH.MOLA:
                myPopulation = BoidsManager.MolaCount;
                break;

            default:
                break;
        }

        _priceText += Mathf.Round(Price).ToString("0");


        priceText.text = _priceText;


        currentFishAmountText.text = "x" + myPopulation;

    }


}
