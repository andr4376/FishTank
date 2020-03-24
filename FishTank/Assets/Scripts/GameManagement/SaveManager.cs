using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static SaveData save;

    private static string filepath = "/SaveData.save";

    private static int tickAutosaveFrequency = 30; //seconds at the moment
    private static int tickCount = 0;

    public delegate void OnSettingsChanged();
    public static OnSettingsChanged onSettingsChanged;

    public static SaveData Save
    {
        get { return save; }
    }

    public static Settings Settings
    {
        get { return Save.settings; }
    }

    public static bool ValidSave
    {
        get
        {
            return save != null && save.valid;
        }
    }

    static SaveManager()
    {
        filepath = Application.persistentDataPath + filepath;

        save = LoadSave();

        if (save == null)
        {
            save = CreateSave();
        }

        if (save != null)
        {
            ScoreManager.onScoreTick += delegate ()
             {
                 AutoSave();
             };
        }

    }


    public static void SettingsHasChanged()
    {
        onSettingsChanged?.Invoke();
    }

    public static void DeleteSave()
    {
        if (File.Exists(filepath))
        {
            File.Delete(filepath);
            Debug.LogWarning("Save file deleted!");
        }
    }

    private static SaveData CreateSave()
    {

        // File.Create(filepath);

        save = new SaveData();

        string jsonSaveFile = JsonUtility.ToJson(save, true);

        File.WriteAllText(filepath, jsonSaveFile);



        return save;
    }

    private static SaveData LoadSave()
    {
        if (File.Exists(filepath))
        {
            string saveFileContent = File.ReadAllText(filepath);

            Debug.Log(saveFileContent);

            save = JsonUtility.FromJson<SaveData>(saveFileContent);

        }

        return save;

    }

    public static bool SaveGame()
    {
        save.Update();

        string jsonSaveFile = JsonUtility.ToJson(save, true);

        try
        {
            File.WriteAllText(filepath, jsonSaveFile);
        }
        catch (Exception)
        {
            Debug.LogWarning("Failed saving game!");

            return false;
        }

        return true;
    }


    private static void AutoSave()
    {
        tickCount++;

        if (tickCount % tickAutosaveFrequency == 0)
        {
            SaveGame();
            Debug.Log("Auto saved!");

        }
    }


}

[System.Serializable]
public class SaveData
{
    public int chromieCount = 0;
    public int eelCount = 0;
    public int molaCount = 0;
    public int barracudaCount = 0;

    public float chromiePointModifier = 0;
    public float eelPointModifier = 0;
    public float molaPointModifier = 0;

    public float score = 0;

    public Settings settings = new Settings();

    public bool valid = false;

    public int[] barracudasHunger;
    public int[] barracudasKC;

    public void Update()
    {
        chromieCount = BoidsManager.ChromisCount;
        eelCount = BoidsManager.EelCount;
        molaCount = BoidsManager.MolaCount;
        barracudaCount = BoidsManager.BarracudaCount;

        chromiePointModifier = Upgrades.PointModifiers[FISH.CHROMIE];
        eelPointModifier = Upgrades.PointModifiers[FISH.EEL];
        molaPointModifier = Upgrades.PointModifiers[FISH.MOLA];

        score = ScoreManager.Score;

        SaveBarracudas();

        valid = true;
    }

    private void SaveBarracudas()
    {
        
        List<BoidsAgent> bCudaAgents = BoidsManager.GetBarracudas();

        barracudasHunger = new int[bCudaAgents.Count];
        barracudasKC = new int[bCudaAgents.Count];


        for (int i = 0; i < bCudaAgents.Count; i++)
        {
            BaracudaScript cuda = bCudaAgents[i] as BaracudaScript;

            barracudasHunger[i] = (int)cuda.Hunger;
            barracudasKC[i] = cuda.killCount;

        }
        
    }
}
