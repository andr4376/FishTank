using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleMenuScript : MonoBehaviour
{

    [SerializeField]
    GameObject menuGo;


    [SerializeField]
    private Sprite openMenuButton;
    [SerializeField]
    private Sprite closeMenuButton;


    private Button button;
    // Start is called before the first frame update
    void Start()
    {
        if (menuGo == null)
        {
            menuGo = GameObject.Find("Menu");
        }
        button = GetComponent<Button>();
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

            if (menuGo.activeInHierarchy)
                button.GetComponent<Image>().sprite = closeMenuButton;

            if (!menuGo.activeInHierarchy)
                button.GetComponent<Image>().sprite = openMenuButton;

    }
}
