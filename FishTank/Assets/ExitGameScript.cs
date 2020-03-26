using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGameScript : MonoBehaviour
{
    public void ExitGame()
    {
        SaveManager.SaveGame();
        Application.Quit();
    }
}
