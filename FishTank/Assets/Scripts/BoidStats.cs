using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidStats : MonoBehaviour
{
    [Range(0,0.8f)]
    public float scaleRange = 0;
    public float ingameScale = 0;

    public float speed;
    public float rotationSpeed;
    public float obstacleDetectionRange = 1;
    public float friendDetectionRange = 1;
    public float avoidanceRange = 1;

    [HideInInspector]
    public float initialSpeed;

    [HideInInspector]
    public float initialRrotationSpeed;
    [HideInInspector]
    public float initialObstacleDetectionRange = 1;
    [HideInInspector]
    public float initialGriendDetectionRange = 1;

    public float maxSpeed=2;
    public float minSpeed=5;
    public float verticalRotationMultiplier=1;

    private void Start()
    {
        float scale = Random.Range(-scaleRange, scaleRange);
        ingameScale = 1+scale;

        transform.localScale *= (1 + scale);

        speed = Random.Range(minSpeed, maxSpeed)* (1 + scale);;
        initialSpeed = speed;
        avoidanceRange *= (1 + scale);
        initialObstacleDetectionRange *= (1 + scale);


        initialRrotationSpeed = rotationSpeed; 
        initialObstacleDetectionRange = obstacleDetectionRange;
        initialGriendDetectionRange = friendDetectionRange;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, avoidanceRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, friendDetectionRange);
    }

}
