using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookScript : MonoBehaviour
{

    public float sensitivity = 100;

    float xRot = 0;

    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float x = Input.GetAxis("Mouse X") * sensitivity 
            * Time.deltaTime;

        float y = Input.GetAxis("Mouse Y") * sensitivity
            * Time.deltaTime;

        xRot -= y;

        xRot = Mathf.Clamp(xRot, -90, 90);

        transform.localRotation = Quaternion.Euler(xRot, 0, 0);
        playerTransform.Rotate(Vector3.up * x);

        
    }
}