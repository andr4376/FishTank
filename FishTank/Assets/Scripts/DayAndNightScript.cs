using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TIME { NIGHT, DAY }
public class DayAndNightScript : MonoBehaviour
{
    public TimeSettings day;
    public TimeSettings night;



    private TIME targetTime = TIME.NIGHT;

    private float cycleSpeed = 0.003f;

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

    float t = 0.0f;


    // Start is called before the first frame update
    void Start()
    {

        if(cam == null)
        cam = Camera.main;
              
        if(dirLight == null)
        dirLight = GameObject.Find("Directional Light").GetComponent<Light>();

        if(fogController==null)
        fogController = GameObject.Find("FogController").GetComponent<FogController>();

        if(oceanFloorScript==null)
        oceanFloorScript = GameObject.Find("OceanFloorLight").GetComponent<SetupOceanFloorScript>();
        
        if(watersurface==null)
        watersurface = GameObject.Find("WaterSurface").GetComponent<WaterSurfaceScript>();

        if (dirLight == null)
        {
            Debug.LogError("dirlight null");
        }
        else
        {
            Debug.Log("dirlight ok");
        }
        if (fogController == null)
        {
            Debug.LogError("fogController null");
        }
        else
        {
            Debug.Log("fogController ok");
        }
        if (oceanFloorScript == null)
        {
            Debug.LogError("oceanFloorScript null");
        }
        else
        {
            Debug.Log("oceanFloorScript ok");
        }
        if (watersurface == null)
        {
            Debug.LogError("watersurface null");
        }
        else
        {
            Debug.Log("watersurface ok");
        }

        if (day == null)
        {
            Debug.Log("day is null");

        }

        if (night == null)
        {
            Debug.Log("night is null");

        }
    }

    // Update is called once per frame
    void Update()
    {

        t += Time.deltaTime * cycleSpeed;

        LerpTowardsTargetTime();
    }

    private void LerpTowardsTargetTime()
    {


        if (targetTime == TIME.DAY)
        {


            fogController._fogEnd =
                Mathf.Lerp(fogController._fogEnd, day.fogEnd, t);

            dirLight.color = Color.Lerp(dirLight.color,
                day.dirLightColor, t);

            cam.backgroundColor = Color.Lerp(cam.backgroundColor,
                day.cameraColor, t);

            watersurface.color = Color.Lerp(watersurface.color,
                day.waterSurfaceColor, t);

            oceanFloorScript.lightColor = Color.Lerp(oceanFloorScript.lightColor,
               day.oceanFloorColor, t);


            if (Mathf.Abs(day.fogEnd - fogController._fogEnd) < closeEnough)
            {
                targetTime = TIME.NIGHT;

                t = 0;
            }
        }

        if (targetTime == TIME.NIGHT)
        {


            fogController._fogEnd =
                Mathf.Lerp(fogController._fogEnd, night.fogEnd, t);

            dirLight.color = Color.Lerp(dirLight.color,
                night.dirLightColor, t);

            cam.backgroundColor = Color.Lerp(cam.backgroundColor,
                night.cameraColor, t);

            watersurface.color = Color.Lerp(watersurface.color,
                night.waterSurfaceColor, t);

            oceanFloorScript.lightColor = Color.Lerp(oceanFloorScript.lightColor,
               night.oceanFloorColor, t);


            if (Mathf.Abs(night.fogEnd - fogController._fogEnd) < closeEnough)
            {
                targetTime = TIME.DAY;

                t = 0;
            }
        }
    }
}


