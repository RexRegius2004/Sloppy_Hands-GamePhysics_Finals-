using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class Customer : MonoBehaviour
{
    public string orderTag; 
    public Transform plateSpawn; 
    public float eatingDuration;

    public TMP_Text customerStatusText; 

    private bool isEating = false;

    private void Start()
    {
        if (!isEating)
        {
            OrderNewPlate();
        }

        UpdateUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isEating) return;

        if (other.CompareTag(orderTag))
        {
            PlateBalance plate = other.GetComponent<PlateBalance>();
            Rigidbody plateRigidbody = other.GetComponent<Rigidbody>();
            GameUI.Instance.AddScore(10);


            if (plateRigidbody != null)
            {
                // Place the plate and start eating
                isEating = true;
                plateRigidbody.isKinematic = true;
                plateRigidbody.useGravity = false;
                plateRigidbody.MovePosition(plateSpawn.position);
                plateRigidbody.MoveRotation(plateSpawn.rotation);

                customerStatusText.color = Color.red;
                StartCoroutine(EatFood(other.gameObject));
            }
        }
    }

    private IEnumerator EatFood(GameObject plate)
    {
        
        eatingDuration = Random.Range(15f, 25f);
        UpdateUI();
        Debug.Log("Customer is eating...");

        // Simulate eating time
        yield return new WaitForSeconds(eatingDuration);

        Debug.Log("Customer finished eating!");
        Destroy(plate); // Remove the plate after eating
        OrderNewPlate(); // Trigger a new order

        isEating = false;
        
        UpdateUI(); // Update the status UI to show the customer is not eating
    }

    private void OrderNewPlate()
    {
        customerStatusText.color = Color.green;
        // Randomly assign a new desired plate tag
        string[] possibleTags = { "PanWaf", "EggnBac", "Burgy" };
        orderTag = possibleTags[Random.Range(0, possibleTags.Length)];

        Debug.Log($"{transform.name} now wants: {orderTag}");
        UpdateUI(); // Update the desired plate UI
    }

    private void UpdateUI()
    {
        customerStatusText.text = isEating
            ? $"Wants: {orderTag}\nStatus: Eating"
            : $"Wants: {orderTag}\nStatus: Waiting for food";
    }
}
