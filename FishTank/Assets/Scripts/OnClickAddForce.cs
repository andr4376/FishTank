using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickAddForce : Clickable
{
    Rigidbody rb;
    Camera cam;

    public float force = 10;

    protected override void OnClick()
    {

        rb.isKinematic = false;

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        rb.AddForce((this.transform.position - mousePos).normalized * force, ForceMode.Force);

        base.OnClick();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
