using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMenuScript : MonoBehaviour
{

    [SerializeField]
    GameObject menuGo;

    // Start is called before the first frame update
    void Start()
    {
        if (menuGo == null)
        {
            menuGo= GameObject.Find("Menu");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            Toggle();
    }

    public void Toggle()
    {
        menuGo.SetActive(!menuGo.activeInHierarchy);
    }
}
