using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraMovement : MonoBehaviour
{
    [Header("References")] 
    public Transform orientation;
    public Transform body;

    public float rotationSpeed;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void FixedUpdate()
    {
        //rotate orientation
        Vector3 viewDirection = body.position - new Vector3(transform.position.x , body.position.y, transform.position.z);
        orientation.forward = viewDirection;
        //rotate characterObj
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
       
        

    }
}
