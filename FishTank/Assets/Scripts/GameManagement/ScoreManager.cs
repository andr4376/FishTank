using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreManager
{
    public delegate void OnScoreChange();
    public delegate void OnScoreTick();
    public static OnScoreChange onScoreChange;
    public static OnScoreTick onScoreTick;

    private static float tickInterval = 1;
    private static float tickTS;

    /// <summary>
    /// A dictionary that dictates how many points the player get based on fish type
    /// </summary>
    private static Dictionary<FISH, float> fishPassivePoints;


    private static float score;
    public static float Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            if (score < 0)
                score = 0;

            onScoreChange?.Invoke();
        }
    }

    static ScoreManager()
    {
        if (SaveManager.ValidSave)
            Score = SaveManager.Save.score;
        else
            Score = 0;


        fishPassivePoints = new Dictionary<FISH, float>() {
            {FISH.CHROMIE,0.05f },
            {FISH.MOLA,3f },
            {FISH.EEL,0.08f },
            {FISH.BARRACUDA,0}
        };

    }

    /// <summary>
    /// every tick the player gets rewarded with a set amount of points
    /// depending on the quantity of fish and the types of fish
    /// The rewards is then multiplied with an upgradable multiplier that
    /// rewards extra points
    /// the tick is called in boidsmanager
    /// </summary>
    public static void Tick()
    {
        if (Time.time >= tickTS + tickInterval)
        {
            float sum = 0;
            sum += BoidsManager.ChromisCount *
                (fishPassivePoints[FISH.CHROMIE] *
                Upgrades.GetUpgradeModifier(FISH.CHROMIE));

            sum += BoidsManager.EelCount *
                (fishPassivePoints[FISH.EEL] *
                Upgrades.GetUpgradeModifier(FISH.EEL));

            sum += BoidsManager.MolaCount *
                (fishPassivePoints[FISH.MOLA] *
                Upgrades.GetUpgradeModifier(FISH.MOLA));

            Score += sum;


            tickTS = Time.time;

            onScoreTick?.Invoke();
        }
    }

    



}
