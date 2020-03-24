using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsEel : BoidsAgent
{
    [SerializeField]
    [Range(0, 1)]
    float downPullFactor = 0.3f;


    

    protected override void ExtraBehaviour(bool headingForCollision)
    {
        if (!headingForCollision)
        {

            MoveTowards(transform.position+(transform.right + Vector3.down)*0.5f, downPullFactor);

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
        BoidsManager.EelCount++;
        base.Init();
    }
    private void OnDestroy()
    {
        BoidsManager.EelCount--;

    }
}
