using UnityEngine;
using UnityEngine.UI; // Required for Text UI
using System.Collections;
using TMPro;

public class Counter : MonoBehaviour
{
    public GameObject platePrefab; // Prefab for the plate
    public Transform spawnPoint; // Position where the plate spawns
    public float cookingTime; // Time it takes to prepare a new plate
    public float raycastDistance = 6f; // Distance for the raycast to check for the plate
    public LayerMask plateLayer; // Layer for the plates
    public TMP_Text statusText; // UI text to display the status

    private bool isCooking = false; // Flag to prevent multiple spawns

    private void Start()
    {
        UpdateStatusText("Ready!", Color.green); // Set initial status to ready
    }

    private void Update()
    {
        IsPlatePresent();
        // Check if there is no plate at the spawn point using a raycast
        if (!isCooking && !IsPlatePresent())
        {
            StartCoroutine(SpawnPlateAfterDelay());
        }
    }

    private bool IsPlatePresent()
    {
        // Perform a raycast to check for the presence of a plate in the upward direction
        RaycastHit hit;
        if (Physics.Raycast(spawnPoint.position, Vector3.up, out hit, raycastDistance, plateLayer))
        {
            Debug.DrawRay(spawnPoint.position, Vector3.up * raycastDistance, Color.green);
            return true;
        }

        Debug.DrawRay(spawnPoint.position, Vector3.up * raycastDistance, Color.red);
        return false;
    }

    private IEnumerator SpawnPlateAfterDelay()
    {
        isCooking = true;
        cookingTime = Random.Range(10f, 15f);
        UpdateStatusText("Cooking...", Color.red); // Update status to cooking
        Debug.Log("Cooking a new plate...");

        yield return new WaitForSeconds(cookingTime); // Simulate cooking time

        if (platePrefab != null && spawnPoint != null)
        {
            Instantiate(platePrefab, spawnPoint.position, spawnPoint.rotation);
            Debug.Log("New plate spawned at the counter.");
        }
        else
        {
            Debug.LogWarning("Plate prefab or spawn point not set.");
        }

        isCooking = false;
        UpdateStatusText("Ready!", Color.green); // Update status to ready
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
