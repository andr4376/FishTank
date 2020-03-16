
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsManager : MonoBehaviour
{

    private List<BoidsAgent> boids;

    [SerializeField]
    [Header("The individual boid prefab & chance for spawn")]
    List<BoidAndSpawnChance> boidsToSpawn;

    [Header("Position and Rotation for spawnpoints:")]
    [SerializeField]
    Transform[] spawnPoints;

    [Header("Set in inspector:")]

    [SerializeField]
    [Range(2,50)]
    int spawnAmount;
    [SerializeField]
    [Range(3, 30)]
    float spawnRadius;

    [Header("Stats for transparency:")]
    [SerializeField]
    int boidsCount;

    


    public static BoidsManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        boids = new List<BoidsAgent>();



        SpawnBoids();
    }

    private void SpawnBoids()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            GameObject boid = null;

            //get random spawn point
            Transform spawnPosition;            
            spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Length)];

            int spawnNumber = Random.Range(0, 101);

            //sort (lowest spawn chance first)
            boidsToSpawn.Sort(BoidAndSpawnChance.SortByChance);

            foreach (BoidAndSpawnChance b in boidsToSpawn)
            {
                if (spawnNumber<=b.chanceOfSpawning)
                {
                    boid = b.boid;
                    break;
                }
            }

            if (boid == null) //select the boid with highest chance in case of error
                boid = boidsToSpawn[boidsToSpawn.Count - 1].boid;




            boid = GameObject.Instantiate(boid,spawnPosition.position,spawnPosition.rotation);
            boid.transform.position += new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                Random.Range(-spawnRadius, spawnRadius),
                Random.Range(-spawnRadius, spawnRadius));

            boid.name = "Boid#" + (i + 1);

            BoidsAgent agent = boid.GetComponent<BoidsAgent>();

            boids.Add(agent);


        }

#if DEBUG
        //for inspector
        boidsCount = boids.Count;
        Debug.Log(boidsCount + " Bois have been adeed to the fish tank");
#endif

    }

    // Update is called once per frame
    void Update()
    {

        foreach (BoidsAgent boid in boids)
        {
            boid.CalculateDirection(boids);
        }

    }

    public static void AddBoid(BoidsAgent b)
    {
        Instance.boids.Add(b);
    }

    public static Transform GetRandomSpawnPoint()
    {
        //get random spawn point

      
        Transform spawnPoint;
        spawnPoint = 
            Instance.spawnPoints
            [Random.Range(0, Instance.spawnPoints.Length)];

        return spawnPoint;
    }
}

[System.Serializable]
struct BoidAndSpawnChance
{
    public GameObject boid;

    [Range(0,100)]
    public float chanceOfSpawning;

    public static int SortByChance(BoidAndSpawnChance p1, BoidAndSpawnChance p2)
    {
        return p1.chanceOfSpawning.CompareTo(p2.chanceOfSpawning);
    }
}
