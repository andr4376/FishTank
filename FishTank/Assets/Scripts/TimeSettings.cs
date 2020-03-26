using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeSettings",
    menuName = "ScriptableObjects/TimeSettings")]
public class TimeSettings : ScriptableObject
{
    public Color dirLightColor;
    public Color cameraColor;
    public Color waterSurfaceColor;
    public Color oceanFloorColor;
    public float fogEnd;
    public float timeModifier;
}
