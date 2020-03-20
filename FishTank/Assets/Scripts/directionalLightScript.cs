using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class directionalLightScript : MonoBehaviour
{

    public float speed = 0.3f;
    public float maxRotation = 15f;

    private Vector3 startRotation;
    private Vector3 startPosition;

    void Update()
    {
        float rotateFactor = maxRotation *
            Mathf.Sin(Time.time * speed);

        transform.rotation = Quaternion.Euler(
            startRotation.x- rotateFactor, startRotation.y
            - rotateFactor, startRotation.z);

        transform.position = new Vector3(startPosition.x - rotateFactor,
            startPosition.y - rotateFactor, startPosition.z);
    }
    // Start is called before the first frame update
    void Start()
    {
        startRotation = transform.rotation.eulerAngles;
        startPosition = transform.position;
    }
}
