using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidStats : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public float obstacleDetectionRange = 1;
    public float friendDetectionRange = 1;
    public float avoidanceRange = 1;

    public float initialSpeed;
    public float initialRrotationSpeed;
    public float initialObstacleDetectionRange = 1;
    public float initialGriendDetectionRange = 1;

    public float maxSpeed=2;
    public float minSpeed=5;
    public float maxSteerForce=3;
    public float verticalRotationMultiplier=1;

    private void Start()
    {
        initialSpeed = speed;
        initialRrotationSpeed = rotationSpeed; ;
        initialObstacleDetectionRange = obstacleDetectionRange;
        initialGriendDetectionRange = friendDetectionRange;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, avoidanceRange);
    }

}
