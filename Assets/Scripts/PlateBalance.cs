using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateBalance : MonoBehaviour
{
    public Camera playerCamera;
    private Rigidbody plateRigidbody;

    public float raycastDistance = 20f;
    private bool holdingPlate = false;
    public Transform playerHand;

    public float angleStability = 0.25f;
    private Vector3 initialDirection;
    public float maxTiltAngle = 90f; 
    public float tiltDifficulty = 0.09f; 
    public float tiltInitial = 0.5f; 
    public float inputStrength = 1f;
    private bool tiltBias; 
    private bool pickedUp = false;
    private float randomTilt;

    void Start()
    {
        plateRigidbody = GetComponent<Rigidbody>();
        playerCamera = Camera.main;

        plateRigidbody.isKinematic = true;
        plateRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

        GameObject handFood = GameObject.Find("PlayerHand");
        playerHand = handFood.transform;

        initialDirection = transform.up;
        tiltBias = Random.value > 0.5f;
    }

    void Update()
    {
        if (holdingPlate)
        {
            PlayerInput();
            PlateTilt();
            plateRigidbody.position = playerHand.position;
            CheckPlateTilt();
        }
        if (!holdingPlate)
        {
            Raycast();
        }
    }

    private void Raycast()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.green);

            if (hit.collider.GetComponent<PlateBalance>() && !hit.collider.GetComponent<PlateBalance>().pickedUp && Input.GetKeyDown(KeyCode.Space))
            {
                hit.collider.GetComponent<PlateBalance>().PickUpPlate();
            }
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.red);
        }
    }

    private void PickUpPlate()
    {
        if (pickedUp) return;

        plateRigidbody.isKinematic = false;
        plateRigidbody.useGravity = false;
        holdingPlate = true;
        pickedUp = true;
        randomTilt = Random.Range(-1f, 1f);

    }

    private void PlayerInput()
    {
        if (Input.GetMouseButton(0))
        {
            plateRigidbody.AddTorque(Vector3.forward * inputStrength, ForceMode.Force);
        }
        if (Input.GetMouseButton(1)) 
        {
            plateRigidbody.AddTorque(Vector3.back * inputStrength, ForceMode.Force);
        }
    }

    private void PlateTilt()
    {
        float angle = Vector3.Angle(transform.up, initialDirection);

        if (angle > 0f && angle < 0.5f)
        {
            plateRigidbody.AddTorque(Vector3.forward * randomTilt * tiltInitial, ForceMode.Force);
        }

        Vector3 tiltDirection = tiltBias ? Vector3.forward : Vector3.back;
        plateRigidbody.AddTorque(tiltDirection * tiltDifficulty, ForceMode.Force);

        if (angle < angleStability)
        {
            tiltBias = !tiltBias;
        }
    }

    private void CheckPlateTilt()
    {
        float angle = Vector3.Angle(transform.up, initialDirection);
        Debug.Log(angle);
        if (angle > maxTiltAngle)
        {

            DropPlate();
            Debug.Log("Plate fell");
        }
    }

    public void DropPlate()
    {
        plateRigidbody.mass = 0.5f;
        holdingPlate = false;
        plateRigidbody.useGravity = true;
        plateRigidbody.isKinematic = false;
        Debug.Log("Plate dropped");
    }

    


}
