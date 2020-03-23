
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum FISH { CHROMIE, EEL, MOLA, BARRACUDA }
public class BoidsManager : MonoBehaviour
{

    private List<BoidsAgent> boids;

    [SerializeField]
    [Header("The individual boid prefab, their spawn rates, and their fish type")]
    List<BoidType> boidsToSpawn;

    [Header("Position and Rotation for spawnpoints:")]
    [SerializeField]
    Transform[] spawnPoints;

    [Header("Set in inspector:")]


    [SerializeField]
    [Range(3, 30)]
    float spawnRadius;

    public static int chromieCount;
    public static int eelCount;
    public static int molaCount;
    public static int barracudaCount;

    private List<BoidsAgent> boidsToRemove = new List<BoidsAgent>();
    private List<BoidsAgent> boidsToAdd = new List<BoidsAgent>();

    public static BoidsManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;

        boids = new List<BoidsAgent>();


    }
    // Start is called before the first frame update
    void Start()
    {
        if (SaveManager.ValidSave)
        {
            SpawnBoidsFromSave();
            Debug.Log("Boidsmanager loaded from save file!");
        }
        else
        {
            SpawnBoids();
            Debug.LogWarning("No save file found, boidsmanager spawned on its own!");

        }



    }

    private void SpawnBoidsFromSave()
    {
        Transform spawnPosition;
        GameObject prefab;


        //Spawn Chromies
        prefab = 
            boidsToSpawn.Where(x => x.type == FISH.CHROMIE).ElementAt(0).boid;

        for (int i = 0; i < SaveManager.Save.chromieCount; i++)
        {
            GameObject boid;

            spawnPosition = spawnPoints[
            i % spawnPoints.Length];

            boid = GameObject.Instantiate(prefab,
                spawnPosition.position, spawnPosition.rotation);

            boid.transform.position += new Vector3(
                UnityEngine.Random.Range(-spawnRadius, spawnRadius),
                UnityEngine.Random.Range(-spawnRadius, spawnRadius),
                UnityEngine.Random.Range(-spawnRadius, spawnRadius));

            boid.name =  "Chromie #" + (i + 1);
        }


        //Spawn Eels
        prefab =
            boidsToSpawn.Where(x => x.type == FISH.EEL).ElementAt(0).boid;

        for (int i = 0; i < SaveManager.Save.eelCount; i++)
        {
            GameObject boid;

            spawnPosition = spawnPoints[i % spawnPoints.Length];

            boid = GameObject.Instantiate(prefab,
                spawnPosition.position, spawnPosition.rotation);

            boid.transform.position += new Vector3(
                UnityEngine.Random.Range(-spawnRadius, spawnRadius),
                UnityEngine.Random.Range(-spawnRadius, spawnRadius),
                UnityEngine.Random.Range(-spawnRadius, spawnRadius));

            boid.name = "Eel #" + (i + 1);
        }

        //Spawn Molas
        prefab =
            boidsToSpawn.Where(x => x.type == FISH.MOLA).ElementAt(0).boid;
        for (int i = 0; i < SaveManager.Save.molaCount; i++)
        {
            GameObject boid;

            spawnPosition = spawnPoints[i % spawnPoints.Length];

            boid = GameObject.Instantiate(prefab,
                spawnPosition.position, spawnPosition.rotation);

            boid.transform.position += new Vector3(
                UnityEngine.Random.Range(-spawnRadius, spawnRadius),
                UnityEngine.Random.Range(-spawnRadius, spawnRadius),
                UnityEngine.Random.Range(-spawnRadius, spawnRadius));

            boid.name = "Mola #" + (i + 1);
        }

        //Spawn Barracudas
        prefab =
            boidsToSpawn.Where(x => x.type == FISH.BARRACUDA).ElementAt(0).boid;
        for (int i = 0; i < SaveManager.Save.barracudaCount; i++)
        {
            GameObject boid;

            spawnPosition = spawnPoints[i % spawnPoints.Length];

            boid = GameObject.Instantiate(prefab,
                spawnPosition.position, spawnPosition.rotation);

            boid.transform.position += new Vector3(
                UnityEngine.Random.Range(-spawnRadius, spawnRadius),
                UnityEngine.Random.Range(-spawnRadius, spawnRadius),
                UnityEngine.Random.Range(-spawnRadius, spawnRadius));

            boid.name = "Barracuda #" + (i + 1);
        }

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
                i%spawnPoints.Length];

                boid = GameObject.Instantiate(boiType.boid,
                    spawnPosition.position, spawnPosition.rotation);

                boid.transform.position += new Vector3(
                    UnityEngine.Random.Range(-spawnRadius, spawnRadius),
                    UnityEngine.Random.Range(-spawnRadius, spawnRadius),
                    UnityEngine.Random.Range(-spawnRadius, spawnRadius));

                boid.name = boiType.type.ToString() + " " + (i + 1);


            }

        }



    }

    // Update is called once per frame
    void Update()
    {
        AddBoids();

        foreach (BoidsAgent boid in boids)
        {
            boid.CalculateDirection(boids);
        }

        RemoveBoids();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Chromie " + chromieCount);
            Debug.Log("Eel " + eelCount);
            Debug.Log("Mola  " + molaCount);
            Debug.Log("Barracuda " + barracudaCount);
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SaveManager.SaveGame();
        }
        if (Input.GetKeyDown(KeyCode.Delete))
            SaveManager.DeleteSave();

    }

    private void RemoveBoids()
    {
        foreach (BoidsAgent item in boidsToRemove)
        {
            boids.Remove(item);

            Destroy(item.gameObject);
        }
        boidsToRemove.Clear();
    }

    private void FixedUpdate()
    {
        ScoreManager.Tick();
    }

    private void AddBoids()
    {
        foreach (BoidsAgent item in boidsToAdd)
        {
            boids.Add(item);
        }
        boidsToAdd.Clear();
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
            [UnityEngine.Random.Range(0, Instance.spawnPoints.Length)];

        return spawnPoint;
    }

    public static void RemoveBoid(BoidsAgent boidsAgent)
    {
        Instance.boidsToRemove.Add(boidsAgent);
    }

    private void _Spawn(FISH fish)
    {
        BoidType boidType = boidsToSpawn.Where(x => x.type == fish).ElementAt(0);

        Transform spawn = GetRandomSpawnPoint();

        GameObject go = Instantiate(boidType.boid, spawn.position, spawn.rotation);

        boidsToAdd.Add(go.GetComponent<BoidsAgent>());
    }



    public static void Spawn(FISH fish)
    {
        Instance._Spawn(fish);
    }
}

[System.Serializable]
struct BoidType
{

    public FISH type;
    public GameObject boid;

    [Range(0, 100)]
    public int spawnCount;

}
