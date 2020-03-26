using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSurfaceScript : MonoBehaviour
{

    public Color color;

    private Material mat;
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        mat.SetColor("_Color", color);
    }
}
