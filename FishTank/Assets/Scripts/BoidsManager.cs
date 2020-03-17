
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsManager : MonoBehaviour
{

    private List<BoidsAgent> boids;

    [SerializeField]
    [Header("The individual boid prefab & chance for spawn")]
    List<BoidType> boidsToSpawn;

    [Header("Position and Rotation for spawnpoints:")]
    [SerializeField]
    Transform[] spawnPoints;

    [Header("Set in inspector:")]

   
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
        Transform spawnPosition;

        foreach (BoidType boiType in boidsToSpawn)
        {
            for (int i = 0; i < boiType.spawnCount; i++)
            {
                GameObject boid;

                spawnPosition = spawnPoints[
                Random.Range(0, spawnPoints.Length)];

                boid = GameObject.Instantiate(boiType.boid,
                    spawnPosition.position, spawnPosition.rotation);

                boid.transform.position += new Vector3(
                    Random.Range(-spawnRadius, spawnRadius),
                    Random.Range(-spawnRadius, spawnRadius),
                    Random.Range(-spawnRadius, spawnRadius));

                boid.name = "Boid#" + (i + 1);

                BoidsAgent agent = boid.GetComponent<BoidsAgent>();

                boids.Add(agent);

            }

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
struct BoidType
{
    public GameObject boid;

    [Range(0,100)]
    public int spawnCount;

}
