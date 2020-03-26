using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNightScript : MonoBehaviour
{
    //Day, afternoon/morning, night
    public TimeSettings[] timeSettings;


    [Range(0,0.003f)]
    public float cycleSpeed = 0.003f;

    float closeEnough = 0.01f;

    [SerializeField]
    Light dirLight;

    Camera cam;

    [SerializeField]
    WaterSurfaceScript watersurface;

    [SerializeField]
    FogController fogController;

    [SerializeField]
    SetupOceanFloorScript oceanFloorScript;

    //lerp time factor
    float t = 0.0f;

    int currentIndex;
    TimeSettings currentTarget;

    //move from day => afternoon => night (true) or Night=>morning =>day (false)
    bool ascending = true;
    private bool shouldLerp = false;


    // Start is called before the first frame update
    void Start()
    {

        if (cam == null)
            cam = Camera.main;

        if (dirLight == null)
            dirLight = GameObject.Find("Directional Light").GetComponent<Light>();

        if (fogController == null)
            fogController = GameObject.Find("FogController").GetComponent<FogController>();

        if (oceanFloorScript == null)
            oceanFloorScript = GameObject.Find("OceanFloorLight").GetComponent<SetupOceanFloorScript>();

        if (watersurface == null)
            watersurface = GameObject.Find("WaterSurface").GetComponent<WaterSurfaceScript>();


        currentIndex = 1;
        currentTarget = timeSettings[currentIndex];

        StartCoroutine(StartInTime(30));

    }

    IEnumerator StartInTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        shouldLerp = true;

        Debug.Log("Starting day and night cycle");
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldLerp)
        {


        t += Time.deltaTime * cycleSpeed *
            currentTarget.timeModifier; //afternoons and mornings are quicker

        LerpTowardsTargetTime();
        }
    }

    private void LerpTowardsTargetTime()
    {
        //Fog distance
        fogController._fogEnd =
            Mathf.Lerp(fogController._fogEnd, currentTarget.fogEnd, t);
        //light color
        dirLight.color = Color.Lerp(dirLight.color,
            currentTarget.dirLightColor, t);
        //background / ocean and fog color
        cam.backgroundColor = Color.Lerp(cam.backgroundColor,
            currentTarget.cameraColor, t);
        //surface color
        watersurface.color = Color.Lerp(watersurface.color,
            currentTarget.waterSurfaceColor, t);
        //the light pattern on floor color
        oceanFloorScript.lightColor = Color.Lerp(oceanFloorScript.lightColor,
           currentTarget.oceanFloorColor, t);

        //If lerped close enough
        if (Mathf.Abs(currentTarget.fogEnd - fogController._fogEnd) < closeEnough)
        {
            //if we have reached the end, turn back
            if (ascending && currentIndex + 1 >= timeSettings.Length)
                ascending = false;

            if (!ascending && currentIndex - 1 < 0)
                ascending = true;

            //fx 1=>2 || 2=>1
            currentIndex += ascending ? 1 : -1;

            currentTarget = timeSettings[currentIndex];

            t = 0;
        }
    }


}


