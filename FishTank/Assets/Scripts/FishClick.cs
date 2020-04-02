using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script  that handles what happens when clicking on a fish
/// uses scriptable objects for audio, points reward ect.
/// </summary>
public class FishClick : Clickable
{
    /// <summary>
    /// Scriptable objects to define rewards and audio, to dittach it 
    /// from the code, and make it easier for the "Game designers" 
    /// </summary>
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
        //if player this click event is not on cooldown
        if (timestamp + fishClick.coolDown <= Time.time)
        {
            //award points (or take away points if negative)
            ScoreManager.Score += fishClick.reward *
               Upgrades.GetUpgradeModifier(fishClick.type);

            //set timestamp / start cooldown, if any
            timestamp = Time.time;

            //Plays audio
            base.OnClick();
        }
    }
}
