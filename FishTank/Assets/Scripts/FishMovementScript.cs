#define DEBUGGING

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoidStats))]
public class FishMovementScript : MonoBehaviour
{


    [SerializeField]
    private float bounceSpeed;

    [SerializeField]
    private float bounceFrequency;

    private BoidStats stats;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<BoidStats>();
    }

    // Update is called once per frame
    void Update()
    {
        //Move in direction that the fish is pointing
        //BoidsAgent will change its rotation
        //So it will avoid obstacles and flock around its comrads
        transform.position +=
            transform.right * stats.speed * Time.deltaTime;

        float hoverMovement = Mathf.Sin(Time.time * bounceSpeed) * bounceFrequency;
        hoverMovement *= Time.deltaTime;
        transform.position = transform.position+
            new Vector3(0, hoverMovement,0);
    }



    private static int OobCount = 0;
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("bounds"))
        {


            Vector3 bounds = other.GetComponent<Collider>().bounds.size;
            Vector3 otherPos = other.transform.position;

            bool wrapped = false;

            //If exit right
            if (transform.position.x >
                otherPos.x + (bounds.x / 2))
            {
                transform.position =
                    new Vector3(otherPos.x - (bounds.x / 2),
                    transform.position.y,
                    transform.position.z);
                wrapped = true;
            }
            else
            {
                //if exit left
                if (transform.position.x <
                    otherPos.x - (bounds.x / 2))
                {
                    transform.position =
                        new Vector3(otherPos.x + (bounds.x / 2),
                        transform.position.y,
                        transform.position.z);
                    wrapped = true;

                }
            }

            if (transform.position.z >
                otherPos.z + (bounds.z / 2))
            {
                //if exit forward
                transform.position =
                    new Vector3(
                        transform.position.x,
                    transform.position.y,
                    otherPos.z - (bounds.z / 2));
                wrapped = true;

            }
            else
            {
                //if exit backwards
                if (transform.position.z <
                    otherPos.z - (bounds.z / 2))
                {
                    transform.position =
                    new Vector3(
                        transform.position.x,
                    transform.position.y,
                    otherPos.z + (bounds.z / 2));
                    wrapped = true;

                }
            }

            //if exited bounds but none of the above
            //(they steer up and down to avoid bottom and top, but just in case)
            if (!wrapped)
            {
                Transform sP = BoidsManager.GetRandomSpawnPoint();

                //Respawn
                this.transform.position = sP.position;
                this.transform.rotation = sP.rotation;
#if DEBUG
                OobCount++;
                Debug.LogWarning(
                    "Out of bound count: " + OobCount + 
                    " - total time: " + Time.time,gameObject);
#endif
            }





        }
    }


}
