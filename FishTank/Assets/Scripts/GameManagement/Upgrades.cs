using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Upgrades
{
    public static Dictionary<FISH, float> PointModifiers
    {
        get;
        private set;
    }

    static Upgrades()
    {
        //if previous save ecist
        if (SaveManager.ValidSave)
        {
            //load
            PointModifiers = new Dictionary<FISH, float>() {
            {FISH.CHROMIE,SaveManager.Save.chromiePointModifier},
            {FISH.MOLA,SaveManager.Save.eelPointModifier  },
            {FISH.EEL,SaveManager.Save.molaPointModifier },
            {FISH.BARRACUDA,1}
        };
        }
        else //set default
        {
            PointModifiers = new Dictionary<FISH, float>() {
            {FISH.CHROMIE,0 },
            {FISH.MOLA,0  },
            {FISH.EEL,0 },
            {FISH.BARRACUDA,1}
            };
        }
    }

    /// <summary>
    /// Returns the points modifier for a certain fish
    /// </summary>
    /// <param name="fish"></param>
    /// <returns></returns>
    public static float GetUpgradeModifier(FISH fish)
    {
        //Fish point value * this
        return 1 + (PointModifiers[fish] * 0.1f);
    }
}
