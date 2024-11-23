using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateBalance : MonoBehaviour
{
    public Camera playerCamera;
    private Rigidbody plateRigidbody;

    public float raycastDistance = 20f;
    private bool isHoldingPlate = false;
    public Transform playerHand;

    private Vector3 initialDirection;
    public float maxTiltAngle = 90f; 
    public float tiltDifficulty = 0.5f;
    public float tiltInitial = 1f;
    public float playerInputStrength = 1f;

    private bool tiltBias; 
    public float stabilityLevel = 0.25f; 

    private bool pickedUp = false;
    private float randomTilt;

    public AudioClip splat;
    public AudioClip plateFall;
    private AudioSource PlateFall;
    private AudioSource Splat;

    void Start()
    {
        plateRigidbody = GetComponent<Rigidbody>();
        GameObject handObject = GameObject.Find("PlayerHand");

        PlateFall = gameObject.AddComponent<AudioSource>();
        Splat = gameObject.AddComponent<AudioSource>();
        Splat.volume = 0.5f;

        playerCamera = Camera.main;
        playerHand = handObject.transform;
        
        plateRigidbody.isKinematic = true;
        plateRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

        initialDirection = transform.up;
        tiltBias = Random.value > 0.5f;


    }

    void Update()
    {
        if (isHoldingPlate)
        {
            PlayerInput();
            PlateTilt();
            plateRigidbody.position = playerHand.position;
            CheckPlateTilt();
        }
        if (!isHoldingPlate)
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
        isHoldingPlate = true;
        pickedUp = true;
        randomTilt = Random.Range(-1f, 1f);
    }

    private void PlayerInput()
    {
        if (Input.GetMouseButton(0))
        {
            plateRigidbody.AddTorque(Vector3.forward * playerInputStrength, ForceMode.Force);
        }

        if (Input.GetMouseButton(1))
        {
            plateRigidbody.AddTorque(Vector3.back * playerInputStrength, ForceMode.Force);
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

        if (angle < stabilityLevel)
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
        }
    }

    public void ServePlate()
    {
        if (!isHoldingPlate) return;

        isHoldingPlate = false;
        plateRigidbody.useGravity = true;
        plateRigidbody.isKinematic = false;
        Debug.Log("Plate Served!");
    }

    public void DropPlate()
    {
        
        isHoldingPlate = false;
        plateRigidbody.useGravity = true;
        plateRigidbody.mass = 0.5f;
        Debug.Log("Plate dropped!");
        PlateFall.PlayOneShot(plateFall);
    }

    public bool IsHeld()
    {
        return isHoldingPlate;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            Splat.PlayOneShot(splat);
        }
    }

}
