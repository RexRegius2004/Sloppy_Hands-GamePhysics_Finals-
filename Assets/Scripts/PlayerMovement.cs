using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 18f;      
    public float lookSpeed = 6f;       
    public Transform playerCamera;     
    private float xRotation = 0f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        MovePlayer();
        LookAround();
    }

    void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical");     

        Vector3 movement = transform.right * horizontal + transform.forward * vertical;
        Vector3 move = rb.position + movement.normalized * moveSpeed * Time.deltaTime;

        rb.MovePosition(move);
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;
        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);  

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);  
    }

    
}