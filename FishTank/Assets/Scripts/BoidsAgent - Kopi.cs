using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoidStats))]
public class BoidsAgentold : MonoBehaviour
{
    BoidStats stats;

#if DEBUG
    private Vector3 flockCenterMassPosition = Vector3.zero;
#endif

    public float cohesionFactor = 1, alignmentFactor = 1, avoidanceFactor = 1;

    private float obstacleTS;
    private float recoverCD = 1f;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<BoidStats>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CalculateDirection(List<BoidsAgent> otherBoids)
    {
        List<BoidsAgent> boidsInRange = new List<BoidsAgent>();

        foreach (BoidsAgent boid in otherBoids)
        {
            if (boid == this)
                continue;
            if (Vector3.Distance(transform.position, boid.transform.position) <= stats.otherBoidsDetectionRange)
                boidsInRange.Add(boid);
        }

        if (!ObstacleDetection())
        {//check ray left and right

            //if timestamp rdy
            if (obstacleTS + recoverCD < Time.time)//if not avoiding obstacles
            {
                stats.rotationSpeed = stats.initialRrotationSpeed;
            }
            else
            {
                stats.rotationSpeed = stats.initialRrotationSpeed * 3;
            }

            if (otherBoids.Count < 1)
                return;

            // Avoidance(boidsInRange);
            //Cohesion(boidsInRange);
            Alignment(boidsInRange);
        }


    }



    private void Alignment(List<BoidsAgent> boidsInRange)
    {
        Vector3 averageRotation = Vector3.zero;

        //add all the rotations together
        foreach (BoidsAgent boid in boidsInRange)
        {
            averageRotation += boid.transform.rotation.eulerAngles;
        }

        if (averageRotation == Vector3.zero)
            return;

        //divide to get average
        averageRotation = averageRotation / boidsInRange.Count;

        //calculate speed
        float speed = (stats.rotationSpeed * alignmentFactor) * Time.deltaTime;

        //rotate towards the average rotation of the boids, lerping by speed
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(averageRotation), speed);

    }

    private void Cohesion(List<BoidsAgent> boidsInRange)
    {

        if (boidsInRange.Count < 1)
            return;

        Vector3 centerPoint = transform.position;

        //add all the positions together
        foreach (BoidsAgent boid in boidsInRange)
        {
            centerPoint += boid.transform.position;
        }

        if (centerPoint == Vector3.zero)
            return;

        //divide to get average
        centerPoint = centerPoint / (boidsInRange.Count + 1);
        /**/

#if DEBUG
        flockCenterMassPosition = centerPoint;
#endif

        float step = (stats.rotationSpeed * cohesionFactor) * Time.deltaTime;
        var targetRotation = Quaternion.LookRotation(centerPoint);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);

    }


    private void Avoidance(List<BoidsAgent> boidsInRange)
    {
        float desiredSeperation = 20f;
        Vector3 target = Vector3.zero;

        foreach (BoidsAgent boid in boidsInRange)
        {
            float d = Vector3.Distance(transform.position, boid.transform.position);

            if ((d > 0) && (d < desiredSeperation))
            {

            }

        }



        /*
        float step = (stats.speed * stats.speedToRotionSpeedFactor) * Time.deltaTime;
        var targetRotation = Quaternion.LookRotation(steer - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);
        */

    }


    private bool ObstacleDetection()
    {
        Ray[] rayArray = new Ray[]{
        new Ray(transform.position, transform.right)
        /*,
        new Ray(transform.position, transform.forward),
        new Ray(transform.position, transform.forward)*/
        };

        bool obstacleDetected = false;

        RaycastHit hit;
        foreach (Ray ray in rayArray)
        {

            if (Physics.Raycast(ray, out hit))
            {


                if (hit.distance <= stats.obstacleDetectionRange)
                {
                    float step = (stats.rotationSpeed) * Time.deltaTime;
                    var targetRotation = 
                        Quaternion.LookRotation(transform.right * -1, Vector3.up);
                    // transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);



                    //over time
                    transform.rotation =
                        Quaternion.Slerp(transform.rotation,
                        targetRotation, stats.rotationSpeed * Time.deltaTime);




                    obstacleTS = Time.time;
#if DEBUG
                    Debug.DrawLine(transform.position, (transform.position + transform.right * stats.obstacleDetectionRange));
                    Debug.DrawLine(transform.position, (transform.position + transform.forward * stats.obstacleDetectionRange));
                    Debug.DrawLine(transform.position, (transform.position + -transform.forward * stats.obstacleDetectionRange));
#endif

                    obstacleDetected = true;
                }
            }
        }

        return obstacleDetected;

    }

    private void OnDrawGizmos()
    {
        if (stats == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, (transform.position + transform.right * stats.obstacleDetectionRange));
    }

    private void OnDrawGizmosSelected()
    {
#if DEBUG

        if (stats == null)
            return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stats.otherBoidsDetectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(flockCenterMassPosition, .3f);
#endif
    }


}
