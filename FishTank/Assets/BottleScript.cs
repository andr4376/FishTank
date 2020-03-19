using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleScript : MonoBehaviour
{

    Material mat;

    Bounds bounds;
    // Start is called before the first frame update
    void Start()
    {
        this.mat = GetComponent<Renderer>().material;
        this.bounds = GetComponent<Renderer>().bounds;
    }

    // Update is called once per frame
    void Update()
    {
        //mat.SetFloat("_LiquidSurface",)
        
    }
}
