//#define DRAW
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoidStats))]
public class BoidsAgent : MonoBehaviour
{
    protected BoidStats stats;

#if DEBUG
    private Vector3 flockCenterMassPosition = Vector3.zero;
#endif

    public float cohesionFactor = 1, alignmentFactor = 1, avoidanceFactor = 1;

    public bool interactWithOtherBoids = true;

    [Range(0, 1)]
    public float obstacleIgnorance = 1;


    static private Transform _boidAnchor;

    static Transform BoidAnchor
    {
        get
        {
            if (_boidAnchor == null)
            {
                GameObject go = new GameObject("Boids");
                _boidAnchor = go.transform;
            }
            return _boidAnchor;
        }
    }

    [SerializeField]
    private LayerMask obstacleLayer;


    // Start is called before the first frame update
    void Start()
    {
        Init();

        GetComponent<FishMovementScript>().onOutOfBounds += delegate ()
        {
            OnOutOfBounds();
        };
    }

    

    protected virtual void Init()
    {
        //For Hierarchy management
        //All boids will appear under the a game object
        transform.SetParent(BoidAnchor, true);

        stats = GetComponent<BoidStats>();

        BoidsManager.AddBoid(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void CalculateDirection(List<BoidsAgent> otherBoids)
    {
        List<BoidsAgent> boidsInRange = new List<BoidsAgent>();

        if (interactWithOtherBoids)
        {
            foreach (BoidsAgent boid in otherBoids)
            {
                if (boid == this)
                    continue;
                if (Vector3.Distance(transform.position, boid.transform.position) <= stats.friendDetectionRange)
                    boidsInRange.Add(boid);
            }
        }

        bool headingForCollision = ObstacleDetection();


        if (!(boidsInRange.Count < 1 || headingForCollision))
        {
            Cohesion(
                boidsInRange,
                headingForCollision);


            Alignment(boidsInRange);

            Avoidance(boidsInRange);
        }

        ExtraBehaviour(headingForCollision);
    }

    protected virtual void ExtraBehaviour(bool headingForCollision)
    {

    }



    protected virtual void Alignment(List<BoidsAgent> boidsInRange)
    {
        if (alignmentFactor == 0)
            return;

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
        transform.rotation =
            Quaternion.Lerp(transform.rotation,
            Quaternion.Euler(averageRotation),
            speed);

    }

    protected virtual void Cohesion(List<BoidsAgent> boidsInRange,
        bool headingForColision = false)
    {

        if (boidsInRange.Count < 1 || cohesionFactor == 0)
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

#if DRAW
        flockCenterMassPosition = centerPoint;
        Debug.DrawLine(transform.position, centerPoint);
#endif
        float steerForce =
            headingForColision ? obstacleIgnorance : 1;

        SteerTowards(centerPoint, steerForce);
        //MoveTowards(centerPoint,cohesionFactor);

    }


    protected virtual bool Avoidance(List<BoidsAgent> boidsInRange)
    {

        if (avoidanceFactor == 0)
            return false;
        Vector3 othersCenterMass = Vector3.zero;
        int count = 0;

        foreach (BoidsAgent boid in boidsInRange)
        {
            float d = Vector3.Distance(transform.position,
                boid.transform.position);

            if ((d > 0) && (d < stats.avoidanceRange))
            {
                othersCenterMass += boid.transform.position;
                count++;
            }

        }

        if (count == 0)
            return false;

        othersCenterMass /= count;

        SteerInDirection((transform.position - othersCenterMass).normalized, avoidanceFactor);
        /*MoveTowards(
            transform.position+((transform.position-othersCenterMass).normalized *
            Vector3.Distance(transform.position,othersCenterMass)), avoidanceFactor);
*/
        return true;
    }


    protected virtual bool ObstacleDetection()
    {
        /*
        Ray[] rayArray = new Ray[]{
        new Ray(transform.position, transform.right),
        new Ray(transform.position, (transform.forward+transform.right)*0.75f),
        new Ray(transform.position, (((transform.forward*-1)+transform.right)*0.75f)),

        new Ray(transform.position, (transform.up+transform.right)*0.75f),
        new Ray(transform.position, (((transform.up*-1)+transform.right)*0.75f))
        };

        bool obstacleDetected = false;

        RaycastHit hit;
        foreach (Ray ray in rayArray)
        {
            if (Physics.Raycast(ray, out hit,obstacleLayer))
            {               

                if (hit.distance <= stats.obstacleDetectionRange)
                {
                    Debug.DrawLine(ray.origin, hit.point);

                    MoveTowards(transform.position+((transform.position - hit.point).normalized), 1f);

                    obstacleDetected = true;
                }
            }
        }
        return obstacleDetected;
        */
        
        bool obstacleDetected = false;
        RaycastHit hit;

        float angle = 0.5f;

        Vector3 detectionStartPos = transform.position;

        

        //Detecting straight ahead
        if (Physics.Raycast(
            detectionStartPos,
            transform.right,
            out hit, stats.obstacleDetectionRange * 0.75f,
            obstacleLayer))
        {

#if DRAW

            Debug.DrawLine(transform.position,
                (transform.position + transform.right *
                stats.obstacleDetectionRange * angle));
#endif

            //turn fast
            stats.rotationSpeed = stats.initialRrotationSpeed * 3;
            stats.speed = stats.initialSpeed * 2f;


            SteerInDirection(transform.right * -1);


        }
        else
        {
            stats.rotationSpeed = stats.initialRrotationSpeed;
            stats.speed = stats.initialSpeed;

        }
        //Detecting left

        if (Physics.Raycast(
            detectionStartPos,
            (transform.forward + transform.right) * angle,
            out hit, stats.obstacleDetectionRange,
            obstacleLayer))
        {

#if DRAW

            Debug.DrawLine(transform.position,
                (transform.position + ((transform.forward +
                transform.right) * angle) *
                stats.obstacleDetectionRange));
#endif

            SteerInDirection(transform.right);

            obstacleDetected = true;
        }
        else
        {


            //Detecting right
            if (Physics.Raycast(
               detectionStartPos,
               ((transform.forward * -1) + transform.right) * angle,
               out hit, stats.obstacleDetectionRange,
            obstacleLayer))
            {

#if DRAW

                Debug.DrawLine(transform.position,
                    (transform.position + (((transform.forward * -1)
                    + transform.right) * angle) *
                    stats.obstacleDetectionRange));
#endif

                SteerInDirection(transform.right * -1);


                obstacleDetected = true;

            }
        }

        //Detecting up
        if (Physics.Raycast(
           detectionStartPos,
           (transform.right + transform.up) * angle,
           out hit, stats.obstacleDetectionRange,
            obstacleLayer))
        {

#if DRAW

            Debug.DrawLine(transform.position,
                (transform.position +
                ((transform.right + transform.up) * angle) *
                stats.obstacleDetectionRange));
#endif

            transform.Rotate(
               new Vector3(0, 0, -1)
               * (stats.rotationSpeed
               * stats.verticalRotationMultiplier) * Time.deltaTime);
            obstacleDetected = true;

        }
        else
        {


            //Detecting down
            if (Physics.Raycast(
               detectionStartPos,
               (transform.right + (transform.up * -1)) * angle,
               out hit, stats.obstacleDetectionRange,
            obstacleLayer))
            {

#if DRAW

                Debug.DrawLine(transform.position,
                    (transform.position +
                    ((transform.right + (transform.up * -1)) * angle) *
                    stats.obstacleDetectionRange));
#endif


                transform.Rotate(
                    new Vector3(0, 0, 1)
                    * (stats.rotationSpeed
                    * stats.verticalRotationMultiplier) * Time.deltaTime);

                obstacleDetected = true;

            }
        }

        return obstacleDetected;
        
    }

    private void OnDrawGizmos()
    {
        if (stats == null)
            return;

        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(transform.position, (transform.position + transform.right * stats.obstacleDetectionRange));
    }

    private void OnDrawGizmosSelected()
    {
#if DRAW

        if (stats == null)
            return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stats.friendDetectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(flockCenterMassPosition, .3f);
#endif
    }


    //TEST
    protected void MoveTowards(Vector3 position, float modifier)
    {
        Vector3 direction = position - transform.position;

        if (direction != Vector3.zero)
        {
            transform.rotation =
                Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(direction) *
                Quaternion.Euler(new Vector3(0, -90, 0)),
                stats.rotationSpeed * modifier * Time.deltaTime);
        }
    }

    protected void SteerInDirection(Vector3 point, float modifier = 1)
    {
        if (Mathf.Approximately(modifier, 0))
            return;

        point.Normalize();

        transform.right = Vector3.Lerp(
            transform.right, point, stats.rotationSpeed * modifier * Time.deltaTime);




        /*
        float step = ((stats.rotationSpeed) * Time.deltaTime)*modifier;
        var targetRotation =
            Quaternion.LookRotation(
                point,
            transform.up);

        //over time
        transform.rotation =
            Quaternion.Slerp(transform.rotation,
            targetRotation, stats.rotationSpeed * Time.deltaTime);
            */
    }

    protected void SteerTowards(Vector3 point, float modifier = 1)
    {
        point = point - transform.position;
        point.Normalize();

        SteerInDirection(point, modifier);
    }


    protected virtual void OnOutOfBounds()
    {

    }

}

