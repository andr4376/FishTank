using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreTextScript : MonoBehaviour
{

    private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();

        ScoreManager.onScoreChange += delegate ()
        {
            UpdateScoreText();
        };
    }

    private void UpdateScoreText()
    {
        text.text = ((int)ScoreManager.Score).ToString();
    }
}
