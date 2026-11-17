using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrapManager : MonoBehaviour
{
    public GameObject trapPrefab; // Prefab της παγίδας (σφαίρα με υφή death.jpg)
    public Transform mazeBounds; // Τα όρια του λαβύρινθου
    public LayerMask wallLayer; // Layer των τοίχων για αποφυγή σύγκρουσης
    public float minSpawnInterval = 3f; // Ελάχιστος χρόνος μεταξύ παγίδων
    public float maxSpawnInterval = 8f; // Μέγιστος χρόνος μεταξύ παγίδων
    public float minTrapDuration = 2f; // Ελάχιστη διάρκεια παραμονής της παγίδας
    public float maxTrapDuration = 5f; // Μέγιστη διάρκεια παραμονής της παγίδας

    void Start()
    {
        StartCoroutine(SpawnTrap());
    }

    public IEnumerator SpawnTrap()
    {
        while (true)
        {
            float spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(spawnInterval);

            Vector3 trapPosition = GetValidRandomPosition();
            GameObject trap = Instantiate(trapPrefab, trapPosition, Quaternion.identity);
            Debug.Log($"Trap spawned at: {trapPosition}");

            float trapDuration = Random.Range(minTrapDuration, maxTrapDuration);
            yield return new WaitForSeconds(trapDuration);

            if (trap != null)
            {
                Debug.Log("Trap destroyed");
                Destroy(trap);
            }
        }
    }


    public Vector3 GetValidRandomPosition()
    {
        Vector3 randomPosition = Vector3.zero;
        bool validPositionFound = false;
        int attempts = 0;

        while (!validPositionFound && attempts < 50)
        {
            // Τυχαία θέση εντός ορίων του λαβυρίνθου
            float x = Random.Range(mazeBounds.position.x - mazeBounds.localScale.x / 2 + 1,
                                   mazeBounds.position.x + mazeBounds.localScale.x / 2 - 1);
            float z = Random.Range(mazeBounds.position.z - mazeBounds.localScale.z / 2 + 1,
                                   mazeBounds.position.z + mazeBounds.localScale.z / 2 - 1);
            randomPosition = new Vector3(x, 2.5f, z);

            // Έλεγχος αν η θέση είναι ελεύθερη (όχι τοίχος)
            if (!Physics.CheckSphere(randomPosition, 2.5f, LayerMask.GetMask("Wall")))
            {
                validPositionFound = true; // Η θέση είναι έγκυρη
            }
            attempts++;
        }

        return randomPosition;
    }
}
