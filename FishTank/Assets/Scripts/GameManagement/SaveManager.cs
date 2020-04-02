using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


/// <summary>
/// A class that keeps track of saved data and settings.
/// It also handles loading and saving of the game.
/// </summary>
public static class SaveManager
{

    private static SaveData save;

    private static string filepath = "/SaveData.save";

    private static int tickAutosaveFrequency = 30; //seconds at the moment
    private static int tickCount = 0;



    public static SaveData Save
    {
        get { return save; }
    }

    public static Settings Settings
    {
        get { return Save.settings; }
    }

    /// <summary>
    /// Property that returns if the game is loaded correctly, and the existing save
    /// was valid
    /// </summary>
    public static bool ValidSave
    {
        get
        {
            return save != null && save.valid;
        }
    }

    /// <summary>
    /// Static constructor for save manager
    /// </summary>
    static SaveManager()
    {
        //Get filepath in %AppData% and add save file name
        filepath = Application.persistentDataPath + filepath;

        //attempt to load
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

            //Convert the json text into an instance of the serializable class SaveData
            save = JsonUtility.FromJson<SaveData>(saveFileContent);

        }

        return save;

    }

    public static bool SaveGame()
    {
        //Make sure the data is up to date.
        save.Update();

        //get save data as a json string (that is formatted with prettyPrint to make it more readable for humans)
        string jsonSaveFile = JsonUtility.ToJson(save, true);

        try
        {
            File.WriteAllText(filepath, jsonSaveFile);
        }
        catch (Exception)
        {
            Debug.LogError("Failed saving game!");

            return false;
        }

        return true;
    }

    /// <summary>
    /// Gets called every money tick (1 second interval),
    /// and if tickAutosaveFrequency = 30,
    /// saves automatically every 30th second
    /// </summary>
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

/// <summary>
/// A serializable class that condenses all the things we want to save into 
/// a single class of info.
/// </summary>
[System.Serializable]
public class SaveData
{
    //Fish counts
    public int chromieCount = 0;
    public int eelCount = 0;
    public int molaCount = 0;
    public int barracudaCount = 0;

    //Fish upgrades
    public float chromiePointModifier = 0;
    public float eelPointModifier = 0;
    public float molaPointModifier = 0;

    //Score / money
    public float score = 0;

    //Settings (fog, audio, volumetric light, ect.)
    public Settings settings = new Settings();

    //if a previous save file exists
    public bool valid = false;

    //Barracuda stats
    //An element is made for each existing barracuda.
    public int[] barracudasHunger;
    public int[] barracudasKC;
    //
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
