using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MaterialRandomizer : MonoBehaviour
{

    [SerializeField] private Material[] materials;
    // Start is called before the first frame update
    void Start()
    {

        if (materials.Length<1)
        {
            this.enabled = false;
            return;
        }

        Renderer r = GetComponent<Renderer>();

        Material m =
            materials[Random.Range(0, materials.Length)];

        r.material = m;

    }

   
}
