using System.Collections;
using UnityEngine;

public class Treasure : MonoBehaviour

{
    public AudioClip treasureSound; // Ο ήχος που παίζει όταν ο θησαυρός αγγίζεται
    private AudioSource audioSource;
    private ParticleSystem particleSystem; // Το Particle System για τα εφέ
    private bool shrinks = false;

    void Start()
    {
        // Προσθήκη AudioSource στο GameObject αν δεν υπάρχει ήδη
        if (!TryGetComponent<AudioSource>(out audioSource))
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = treasureSound;
        audioSource.playOnAwake = false; // Ο ήχος δεν παίζει αυτόματα
        audioSource.loop = false; // Ο ήχος δεν κάνει loop

        // Βρες το Particle System
        particleSystem = GetComponentInChildren<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.Stop(); // Βεβαιώσου ότι δεν παίζει αρχικά
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Treasure triggered by: {other.name}, Tag: {other.tag}");
        if (other.CompareTag("Player") && !shrinks)
        {
            audioSource.Play(); // Παίζει ο ήχος
                                // Παίξε το εφέ
            GetComponentInChildren<ParticleSystem>().Play();

            Debug.Log("Player touched the treasure!");
            StartCoroutine(ShrinkAndDestroy());
        }
    }


    public IEnumerator ShrinkAndDestroy()
    {
        Debug.Log("Shrinking the treasure...");
        shrinks = true;

        float shrinkDuration = 0.5f; // Χρόνος σμίκρυνσης
        float elapsed = 0; // Χρόνος που πέρασε
        Vector3 originalScale = transform.localScale; // Αρχικό μέγεθος του θησαυρού
        Vector3 targetScale = Vector3.zero; // Εξασφαλίζουμε ότι είναι στο μηδέν

        

        while (elapsed < shrinkDuration)
        {
            // Γραμμική σμίκρυνση από το αρχικό μέγεθος στο μηδέν
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / shrinkDuration);

            elapsed += Time.deltaTime; // Ενημέρωση του χρόνου
            yield return null; // Επιστροφή στο επόμενο frame
        }
        transform.localScale = targetScale;
        
        Debug.Log("Treasure destroyed!");

        Destroy(gameObject); // Καταστροφή του θησαυρού
    }
}
