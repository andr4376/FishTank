using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class scorePerSecondTextScript : MonoBehaviour
{
    private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        this.text = GetComponent<TextMeshProUGUI>();

        ScoreManager.onScoreTick += delegate ()
        {
            UpdateText();
        };
        
    }

    private void UpdateText()
    {
        text.text = "+" + ScoreManager.lastSum.ToString("0") + "/s";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
