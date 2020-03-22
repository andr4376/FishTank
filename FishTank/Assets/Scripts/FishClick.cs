using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishClick : Clickable
{
    public ScriptableFishClick fishClick;

    private float timestamp;

    private void Start()
    {
        this.clickSound = fishClick.sounds;
        this.volume = fishClick.soundVolume;

        Time.timeScale = 1;
    }

    protected override void OnClick()
    {
   
        if (timestamp+fishClick.coolDown <= Time.time)
        {
        Debug.Log(fishClick.reward);

        timestamp = Time.time;

        base.OnClick();
        }
    }
}
