using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSettingsScript : MonoBehaviour
{

    public GameObject settingsGo;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Toggle();
        }
    }

    public void Toggle()
    {
         settingsGo.SetActive(!settingsGo.activeInHierarchy);
    }
}
