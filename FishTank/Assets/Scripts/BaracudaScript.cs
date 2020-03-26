using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaracudaScript : BoidsAgent
{

    Transform target;

    float maxTargetEscapeRange = 30;

    [SerializeField]
    float hunger = 100;

    [SerializeField]
    float hungerDrainrate = 0.5f;


    /// <summary>
    /// If a barracuda kills enough targets, it'll invite more barracudas
    /// </summary>
    [SerializeField]
    int summonMoreBarracudasKC = 10;

    [Header("Show In inspector:")]
    [SerializeField] public int killCount = 0;


    [HideInInspector]
    public float Hunger
    {
        get
        {
            return hunger;
        }
        set
        {
            hunger = value;
            if (hunger < 0)
                hunger = 0;

            if (hunger > 100)
                hunger = 100;
        }
    }

    private bool IsHungry
    {
        get
        {
            return Hunger < 30;
        }
    }

    private int KillCount
    {
        get
        {
            return killCount;
        }
        set
        {
            killCount = value;

            
            if (killCount % summonMoreBarracudasKC == 0)
                BoidsManager.Spawn(FISH.BARRACUDA);
        }
    }

    protected override void Init()
    {
        BoidsManager.BarracudaCount++;
        base.Init();
    }
    private void OnDestroy()
    {
        BoidsManager.BarracudaCount--;

    }

    protected override void ExtraBehaviour(bool headingForCollision)
    {
        base.ExtraBehaviour(headingForCollision);
    }

    private void Update()
    {
        if(!SaveManager.Settings.peaceMode)
        Hunger -= hungerDrainrate * Time.deltaTime;
    }

    protected override bool PriorityBehaviour(List<BoidsAgent> boidsInRange)
    {
        if (SaveManager.Settings.peaceMode)
            return true;

        if (IsHungry)
        {

            if (target != null)
            {
                if (Vector3.Distance(this.transform.position, target.position) > maxTargetEscapeRange)
                {
                    target = null;

                }
                else
                {
                    MoveTowards(target.position, 1);
                    return false;
                }

            }

            foreach (BoidsAgent boid in boidsInRange)
            {
                if (boid is ChromieScript)
                {
                    target = boid.transform;
                    return false;
                }
            }
        }
        else
        {
            target = null;
        }


        return true;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Chromie"))
        {
            if (target != null && other.transform == target)
            {
                KillTarget();

            }
        }
    }

 

    private void KillTarget()
    {
        Hunger += 100;

        SoundManager.PlayAudio(SOUNDS.EAT_SOUND, 0.2f);

        KillCount++;

        BoidsManager.RemoveBoid(target.GetComponent<BoidsAgent>());

        target = null;
    }
}
