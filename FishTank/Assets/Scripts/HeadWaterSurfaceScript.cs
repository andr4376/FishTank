using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadWaterSurfaceScript : MonoBehaviour
{

    public delegate void OnAboveWater();
    public  OnAboveWater onAboveWater;

    private static HeadWaterSurfaceScript s;

    private bool aboveWater = false;

    public LayerMask waterLayerMask;

    private void Awake()
    {
        s = this;
    }

    public static void AddOnAboveWaterEvent(OnAboveWater method)
    {
        if (s==null)
        {
            return;
        }
         s.onAboveWater+= method ;
    }

    private void Update()
    {
        bool tmp = !Physics.CheckSphere(
                                 transform.position,
                                 0.1f,
                                 waterLayerMask);

        if(tmp != aboveWater)
            onAboveWater?.Invoke();

    }

    private void Start()
    {
        onAboveWater += delegate ()
         {
             aboveWater = !aboveWater;
         };
    }

    public static bool AboveWater()
    {
        return s.aboveWater;
    }

   
}
