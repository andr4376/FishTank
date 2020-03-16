﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{
    //Inspector Settings
    [Header("Points that the Camera should move between on input")]
    [SerializeField]
    Transform[] pointsToMoveBetween;

    [Header("Movement settings")]
    [SerializeField]
    private float movementSpeed = 1;
    [SerializeField]
    private float rotationSpeed = 1;

    [SerializeField]
    [Tooltip("The radius at which the camera accept it has arrived at its goal")]
    private float goalPrecision = 10;

    [Header("For Debugging: ")]
    [SerializeField]
    private Transform nextPoint;

    [SerializeField]
    private Transform previousPoint;


    //Privates
    private float inputXDir;

    private int pointIndex;

    // Start is called before the first frame update
    void Start()
    {
        if (pointsToMoveBetween.Length < 2)
        {
            this.enabled = false; return;
        }


        previousPoint = pointsToMoveBetween[0];
        nextPoint = pointsToMoveBetween[1];

        pointIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        inputXDir = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        MoveToPoint(inputXDir);
    }

    private void MoveToPoint(float inputXDir)
    {
        if (inputXDir > 0)
        {
            if (Vector3.Distance
                (this.transform.position, nextPoint.position)
                <= goalPrecision)
            {

                if (pointIndex + 1 < pointsToMoveBetween.Length)
                {
                    pointIndex++;
                    previousPoint = nextPoint;
                    nextPoint = pointsToMoveBetween[pointIndex];
                }
                else
                {
                    return;
                }
            }
            if (nextPoint != null)
                MoveTowards(nextPoint);
        }

        if (inputXDir < 0)
        {
            if (Vector3.Distance
                (this.transform.position, previousPoint.position)
                <= goalPrecision)
            {

                if (pointIndex - 1 >= 0)
                {
                    pointIndex--;
                    nextPoint = previousPoint;
                    previousPoint = pointsToMoveBetween[pointIndex];
                }
                else
                {
                    return;
                }
            }
            if (nextPoint != null)
                MoveTowards(previousPoint);
        }
    }

    private void MoveTowards(Transform t)
    {
        float distance = Vector3.Distance(previousPoint.position,
            nextPoint.position);


        this.transform.position =
            Vector3.MoveTowards(transform.position, t.position,
            (movementSpeed * Time.deltaTime) );


        transform.rotation = 
            Quaternion.Lerp(
                transform.rotation,
            t.rotation, 
             (rotationSpeed*Time.deltaTime));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, goalPrecision);


        foreach (Transform t in pointsToMoveBetween)
        {
            Gizmos.DrawSphere(t.position, goalPrecision);
        }

        if (nextPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(this.transform.position, nextPoint.transform.position);
        }

        if (previousPoint != null)
        {

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(this.transform.position, previousPoint.transform.position);
        }

    }
}
