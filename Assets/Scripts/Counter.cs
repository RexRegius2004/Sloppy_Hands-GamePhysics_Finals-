using UnityEngine;
using UnityEngine.UI; 
using System.Collections;
using TMPro;

public class Counter : MonoBehaviour
{
    public GameObject platePrefab; 
    public Transform spawnPoint;
    public float cookingTime;
    public float raycastDistance = 6f;
    public LayerMask plateLayer;
    public TMP_Text statusText;
    private bool isCooking = false;

    public AudioClip cooking;
    public AudioClip ding;
    private AudioSource Ding;
    private AudioSource Cooking;


    private void Start()
    {
        Ding = gameObject.AddComponent<AudioSource>();
        Cooking = gameObject.AddComponent<AudioSource>();
        Cooking.clip = cooking;
        Ding.clip = ding;
        Cooking.loop = true;
        Cooking.volume = 0.5f;
        Ding.volume = 0.5f;

        UpdateStatusText("Ready!", Color.green);
    }

    private void Update()
    {
        PlateRaycast();
        if (!isCooking && !PlateRaycast())
        {
            StartCoroutine(CookFood());
        }
    }

    private bool PlateRaycast()
    {
        Vector3 rayOrigin = spawnPoint.position + Vector3.down * 0.1f;
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, Vector3.up, out hit, raycastDistance, plateLayer))
        {
            Debug.DrawRay(rayOrigin, Vector3.up * raycastDistance, Color.green);
            return true;
        }

        Debug.DrawRay(rayOrigin, Vector3.up * raycastDistance, Color.red);
        return false;
    }


    private IEnumerator CookFood()
    {
        Cooking.Play();
        isCooking = true;
        cookingTime = Random.Range(10f, 15f);
        UpdateStatusText("Cooking...", Color.red);
        Debug.Log("Cooking a new plate...");

        yield return new WaitForSeconds(cookingTime);

        Instantiate(platePrefab, spawnPoint.position, spawnPoint.rotation);
        Debug.Log("New plate spawned at the counter.");
       
        isCooking = false;
        Cooking.Pause();
        Ding.Play();
        UpdateStatusText("Ready!", Color.green);
    }

    private void UpdateStatusText(string message, Color color)
    {
        if (statusText != null)
        {
            statusText.text = message;
            statusText.color = color;
        }
        else
        {
            Debug.LogWarning("Status Text UI not assigned.");
        }
    }
}
