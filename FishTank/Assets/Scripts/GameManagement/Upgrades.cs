using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Upgrades
{
    public static Dictionary<FISH, float> pointModifiers;

    static Upgrades()
    {
        if (SaveManager.ValidSave)
        {

            //load
            pointModifiers = new Dictionary<FISH, float>() {
            {FISH.CHROMIE,SaveManager.Save.chromiePointModifier },
            {FISH.MOLA,SaveManager.Save.eelPointModifier  },
            {FISH.EEL,SaveManager.Save.molaPointModifier },
            {FISH.BARRACUDA,1}
        };
        }
        else
        {
            pointModifiers = new Dictionary<FISH, float>() {
            {FISH.CHROMIE,1 },
            {FISH.MOLA,1  },
            {FISH.EEL,1 },
            {FISH.BARRACUDA,1}
            };
        }
    }
}
