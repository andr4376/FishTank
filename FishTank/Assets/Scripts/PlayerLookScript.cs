using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookScript : MonoBehaviour
{

    public float sensitivity = 100;

    float xRot = 0;
    float yRot = 0;

    private Transform playerTransform;

    [SerializeField] bool holdToLook = false;



    private void Start()
    {
        try
        {
        playerTransform = GameObject.Find("Player").transform;

        }
        catch (System.Exception)
        {
            this.enabled = false;
        }
       // Cursor.lockState = CursorLockMode.Locked;

        
    }

    private void Update()
    {

        if (holdToLook && !Input.GetMouseButton(1))
            return;
            

            float x = Input.GetAxis("Mouse X") * sensitivity 
            * Time.unscaledDeltaTime;

        float y = Input.GetAxis("Mouse Y") * sensitivity
            * Time.unscaledDeltaTime;

        xRot -= y;
       // yRot += x;

        xRot = Mathf.Clamp(xRot, -90, 90);

        transform.localRotation = Quaternion.Euler(xRot, 0, 0);

        playerTransform.Rotate(Vector3.up * x);
        }


}