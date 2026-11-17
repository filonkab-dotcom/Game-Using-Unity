
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureManager : MonoBehaviour
{
    public GameObject[] treasurePrefabs; // Τα Prefabs των θησαυρών (κεράσια, πορτοκάλια, λεμόνια)
    public float spawnInterval = 5f; // Χρόνος μεταξύ των εμφανίσεων
    public float treasureLifetime = 10f; // Χρόνος που παραμένει κάθε θησαυρός
    public Transform mazeBounds; // Αντικείμενο που καθορίζει τα όρια του λαβυρίνθου
    public LayerMask wallLayer; // Layer των τοίχων για έλεγχο συγκρούσεων

    private GameObject currentTreasure; // Ο τρέχων θησαυρός

    void Start()
    {
        StartCoroutine(SpawnTreasure());
    }

    public IEnumerator SpawnTreasure()
    {
        while (true)
        {
            // Αν υπάρχει ήδη θησαυρός, τον διαγράφουμε
            if (currentTreasure != null)
            {
                Destroy(currentTreasure);
            }

            // Προσπάθεια εύρεσης τυχαίας θέσης
            Vector3 spawnPosition = GetValidRandomPosition();

            // Δημιουργία θησαυρού
            int index = Random.Range(0, treasurePrefabs.Length);
            currentTreasure = Instantiate(treasurePrefabs[index], spawnPosition, Quaternion.identity);

            // Διαγραφή του θησαυρού μετά από τον χρόνο ζωής
            StartCoroutine(DestroyTreasureAfterTime(currentTreasure, treasureLifetime));

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public Vector3 GetValidRandomPosition()
    {
        Debug.Log("Αναζήτηση έγκυρης θέσης για τον θησαυρό...");
        Vector3 randomPosition = Vector3.zero;
        bool validPositionFound = false;

        int maxAttempts = 50; // Μέγιστος αριθμός προσπαθειών για εύρεση θέσης
        int attempts = 0;

        while (!validPositionFound && attempts < maxAttempts)
        {
            float x = Random.Range(mazeBounds.position.x - mazeBounds.localScale.x / 2 + 1,
                                   mazeBounds.position.x + mazeBounds.localScale.x / 2 - 1);
            float z = Random.Range(mazeBounds.position.z - mazeBounds.localScale.z / 2 + 1,
                                   mazeBounds.position.z + mazeBounds.localScale.z / 2 - 1);

            randomPosition = new Vector3(x, 2.5f, z);

            if (!Physics.CheckSphere(randomPosition, 2.5f, LayerMask.GetMask("Wall")))
            {
                validPositionFound = true; // Η θέση είναι έγκυρη
                Debug.Log($"Έγκυρη θέση: {randomPosition}");
            }

            attempts++;
        }

        if (!validPositionFound)
        {
            Debug.LogWarning("Δεν βρέθηκε έγκυρη θέση για τον θησαυρό!");
        }

        return randomPosition;
    }


    public IEnumerator DestroyTreasureAfterTime(GameObject treasure, float time)
    {
        yield return new WaitForSeconds(time);
        if (treasure != null)
        {
            Destroy(treasure);
        }
    }
}
