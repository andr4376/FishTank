using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookScript : MonoBehaviour
{

    public float sensitivity = 100;

    float xRot = 0;
    float yRot = 0;

    private Transform playerTransform;

    private void Start()
    {
        //playerTransform = GameObject.Find("Player").transform;
       // Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {

        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;

            float x = Input.GetAxis("Mouse X") * sensitivity 
            * Time.unscaledDeltaTime;

        float y = Input.GetAxis("Mouse Y") * sensitivity
            * Time.unscaledDeltaTime;

        xRot -= y;
        yRot += x;

        xRot = Mathf.Clamp(xRot, -90, 90);

        transform.localRotation = Quaternion.Euler(xRot, yRot, 0);

        //playerTransform.Rotate(Vector3.up * x);
        }
        Cursor.lockState = CursorLockMode.None;


    }
}