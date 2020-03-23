using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// For barracudas specefically, when you click them, instead of gaining
/// points, you lose some points, but in return you feed them
/// and thereby prevent them from eating your chromies
/// </summary>
public class barracudaClick : FishClick
{
    [SerializeField]
    private float feedAmount = 30;

    protected override void OnClick()
    {
        if (ScoreManager.Score>= (fishClick.reward*-1))
        {

        BaracudaScript b = GetComponent<BaracudaScript>();
        b.Hunger += feedAmount;

        base.OnClick();
        }
    }
}
