using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorkScript : Clickable
{
    public Material liquidMat;

    public ParticleSystem particles;

    public float drainRate = 0.4f;

    public float liquidStartAmount = -0.63f;

    private bool open = false;

    private void Start()
    {
        liquidMat.SetFloat("_FillAmount", liquidStartAmount);
        particles = GetComponent<ParticleSystem>();
    }

    protected override void OnClick()
    {
        //Start Wine Script

        open = true;

        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;


        particles.Play();

        base.OnClick();
    }

    private void Update()
    {
        if (open)
        {

           float amount =  liquidMat.GetFloat("_FillAmount");

            if(amount>3)
            {
                this.enabled = false;
                return;
            }
            float newAmount = amount += drainRate * Time.deltaTime;
           liquidMat.SetFloat("_FillAmount", newAmount);
                    
            

        }

    }


}
