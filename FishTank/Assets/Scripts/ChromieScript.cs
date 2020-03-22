using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChromieScript : BoidsAgent
{
   

    protected override void ExtraBehaviour(bool headingForCollision)
    {
        base.ExtraBehaviour(headingForCollision);
    }

    protected override bool PriorityBehaviour(List<BoidsAgent> boidsInRange)
    {
        //If it sees a baracuda, run away, and do not
        // attempt to do flock behaviour

        foreach (BoidsAgent boid in boidsInRange)
        {
            if (boid is BaracudaScript)
            {
                SteerInDirection((transform.position - boid.transform.position)
                    .normalized, avoidanceFactor);

                return false;
            }
        }

        return true;
    }
}
