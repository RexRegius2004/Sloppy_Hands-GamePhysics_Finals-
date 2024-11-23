using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Customer : MonoBehaviour
{
    public string orderTag; 
    public Transform plateTransform;
    public float eatingDuration; 

    public TMP_Text customerStatusText; 

    private bool isEating = false;

    public AudioClip pop; // Drag your sound effect here
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = pop;
        audioSource.volume = 0.2f;

        if (!isEating)
        {
            OrderNewPlate();
        }

        customerStatusText.color = Color.green;
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

            if (plate != null && plate.IsHeld())
            {
                plate.ServePlate();
            }

            if (plateRigidbody != null)
            {
                isEating = true;
                plateRigidbody.isKinematic = true;
                plateRigidbody.useGravity = false;
                plateRigidbody.MovePosition(plateTransform.position);
                plateRigidbody.MoveRotation(plateTransform.rotation);

                StartCoroutine(EatFood(other.gameObject));
            }
        }
    }

    private IEnumerator EatFood(GameObject plate)
    {
        customerStatusText.color = Color.red;
        eatingDuration = Random.Range(15f, 25f);
        UpdateUI();
        Debug.Log("Customer is eating...");

        yield return new WaitForSeconds(eatingDuration);

        Debug.Log("Customer finished eating!");
        Destroy(plate); 
        OrderNewPlate(); 

        isEating = false;
        customerStatusText.color = Color.green;
        UpdateUI();
    }

    private void OrderNewPlate()
    {
       
        audioSource.PlayOneShot(pop);
        string[] possibleTags = { "PanWaf", "EggnBac", "Burgy" };
        orderTag = possibleTags[Random.Range(0, possibleTags.Length)];

        Debug.Log($"{transform.name} now wants: {orderTag}");
        UpdateUI();
    }

    private void UpdateUI()
    {
        customerStatusText.text = isEating
            ? $"Wants: {orderTag}\nStatus: Eating"
            : $"Wants: {orderTag}\nStatus: Waiting for food";
    }
}
