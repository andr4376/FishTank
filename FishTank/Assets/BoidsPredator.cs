using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsPredator : BoidsAgent
{

    static private Transform _boidAnchor;
    static Transform BoidAnchor
    {
        get
        {
            if (_boidAnchor == null)
            {
                GameObject go = new GameObject("Predators");
                _boidAnchor = go.transform;
            }
            return _boidAnchor;
        }
    }
    public override void CalculateDirection(List<BoidsAgent> otherBoids)
    {
        base.CalculateDirection(otherBoids);
    }

    protected override void Alignment(List<BoidsAgent> boidsInRange)
    {
        base.Alignment(boidsInRange);
    }

    protected override void Avoidance(List<BoidsAgent> boidsInRange)
    {
        base.Avoidance(boidsInRange);
    }

    protected override void Cohesion(List<BoidsAgent> boidsInRange, bool headingForColision = false)
    {
        base.Cohesion(boidsInRange, headingForColision);
    }

    protected override void Init()
    {
        //For Hierarchy management
        //All boids will appear under the a game object
        transform.SetParent(BoidAnchor, true);
    }

    protected override bool ObstacleDetection()
    {
        return base.ObstacleDetection();
    }

    
}
