using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsMola : BoidsAgent
{
    [SerializeField]
    [Range(0, 1)]
    float upPullFactor = 0.3f;



    protected override void ExtraBehaviour(bool headingForCollision)
    {
        if (!headingForCollision)
        {

            MoveTowards(transform.position + (transform.right + 
                Vector3.up) * 0.5f, upPullFactor);

            /*
            transform.Rotate(
                           new Vector3(0, 0, -1)
                           * (stats.rotationSpeed
                           * stats.verticalRotationMultiplier * downPullFactor) * Time.deltaTime);
             */

        }
    }

    protected override void Init()
    {
        BoidsManager.molaCount++;
        base.Init();
    }
    private void OnDestroy()
    {
        BoidsManager.molaCount--;

    }
}
