using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaracudaScript : BoidsAgent
{

    Transform target;

    float maxTargetEscapeRange = 30;

    [SerializeField]
    float hunger = 100;

    [SerializeField]
    float hungerDrainrate = 0.5f;

    private float Hunger
    {
        get
        {
            return hunger;
        }
        set
        {
            hunger = value;
            if (hunger < 0)
                hunger = 0;

            if (hunger > 100)
                hunger = 100;
        }
    }

    private bool IsHungry
    {
        get
        {
            return Hunger < 30;
        }
    }

    protected override void ExtraBehaviour(bool headingForCollision)
    {
        base.ExtraBehaviour(headingForCollision);
    }

    private void Update()
    {
        Hunger -= hungerDrainrate * Time.deltaTime;
    }

    protected override bool PriorityBehaviour(List<BoidsAgent> boidsInRange)
    {


        if (IsHungry)
        {

            if (target != null)
            {
                if (Vector3.Distance(this.transform.position, target.position) > maxTargetEscapeRange)
                {
                    target = null;

                }
                else
                {
                    MoveTowards(target.position, 1);
                    return false;
                }

            }

            foreach (BoidsAgent boid in boidsInRange)
            {
                if (boid is ChromieScript)
                {
                    target = boid.transform;
                    return false;
                }
            }
        }

        return true;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Chromie"))
        {
            if (target != null && other.transform == target)
            {
                Hunger += 100;

                Destroy(other.gameObject);
            }
        }
    }


}
