using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The common stats all boids use in their behaviour
/// </summary>
public class BoidStats : MonoBehaviour
{
    /// <summary>
    /// how much should a boid be randomly scaled
    /// fx: Standard scale += random(-Range,Range)
    /// </summary>
    [Range(0, 0.8f)]
    public float scaleRange = 0;
    [HideInInspector]
    public float ingameScale = 0;

    /// <summary>
    /// Movement speed
    /// </summary>
    public float speed;

    /// <summary>
    /// Rotation speed, for when the boid agent adjusts its rotation
    /// </summary>
    public float rotationSpeed;

    //The boids generates its initial speed randomly, based on min and max speed
    public float maxSpeed = 5;
    public float minSpeed = 2;
    //

    /// <summary>
    /// used to tweak the boids dodging capabilities when it detects water surface
    /// or the ocean floor
    /// </summary>
    public float verticalRotationMultiplier = 1;
    /// <summary>
    /// The range of the raycasts, detecting obstacles
    /// </summary>
    public float obstacleDetectionRange = 1;

    /// <summary>
    /// The radius in which it can see other boids
    /// </summary>
    public float otherBoidsDetectionRange = 1;

    /// <summary>
    /// the radius in which the boid will avoid other boids
    /// </summary>
    public float avoidanceRange = 1;

    [HideInInspector]
    public float initialSpeed;
    [HideInInspector]
    public float initialRrotationSpeed;
    [HideInInspector]
    public float initialObstacleDetectionRange = 1;
    [HideInInspector]
    public float initialGriendDetectionRange = 1;


    private void Start()
    {
        //Randomize scaling
        float scale = Random.Range(-scaleRange, scaleRange);
        ingameScale = 1 + scale;

        transform.localScale *= (1 + scale);

        //Move faster or slower based on scale
        speed = Random.Range(minSpeed, maxSpeed) * (1 + scale);
        initialSpeed = speed;

        //vision based on scale
        avoidanceRange *= (1 + scale);
        initialObstacleDetectionRange *= (1 + scale);


        initialRrotationSpeed = rotationSpeed;
        initialObstacleDetectionRange = obstacleDetectionRange;
        initialGriendDetectionRange = otherBoidsDetectionRange;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, avoidanceRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, otherBoidsDetectionRange);
    }

}
