using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishClick : Clickable
{
    [SerializeField]
    protected ScriptableFishClick fishClick;

    private float timestamp;

    private void Start()
    {
        this.clickSound = fishClick.sounds;
        this.volume = fishClick.soundVolume;

    }



    protected override void OnClick()
    {
   
        if (timestamp+fishClick.coolDown <= Time.time)
        {
            ScoreManager.Score += fishClick.reward *
               Upgrades.GetUpgradeModifier(fishClick.type);

            Debug.Log(fishClick.reward *
               Upgrades.GetUpgradeModifier(fishClick.type));

        timestamp = Time.time;

        base.OnClick();
        }
    }
}
