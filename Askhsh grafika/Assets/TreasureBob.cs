using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBob : MonoBehaviour
{
    public float moveSpeed = 5f; // Ταχύτητα κίνησης
    public Transform mazeBounds; // Αντικείμενο που καθορίζει τα όρια του λαβυρίνθου
    public float detectionRadius = 3.5f; // Ακτίνα ανίχνευσης εμποδίων
    public LayerMask wallLayer; // Layer για τα τοιχώματα
    private Vector3 movementDirection; // Κατεύθυνση κίνησης
    public Vector3 minBounds; // Ελάχιστα όρια του λαβυρίνθου
    public Vector3 maxBounds; // Μέγιστα όρια του λαβυρίνθου
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        // Υπολογισμός ορίων από το mazeBounds
        Vector3 mazeCenter = mazeBounds.position;
        Vector3 mazeSize = mazeBounds.localScale;

        minBounds = new Vector3(mazeCenter.x - mazeSize.x / 2, transform.position.y, mazeCenter.z - mazeSize.z / 2);
        maxBounds = new Vector3(mazeCenter.x + mazeSize.x / 2, transform.position.y, mazeCenter.z + mazeSize.z / 2);
    }

    void Update()
    {
        HandleInput();
        MoveBob();
    }

    void HandleInput()
    {
        // Λήψη εισόδου για τον άξονα Χ και Ζ
        float moveX = 0f;
        float moveZ = 0f;

        if (Input.GetKey(KeyCode.J)) moveX = -1f; // Αριστερά
        if (Input.GetKey(KeyCode.L)) moveX = 1f;  // Δεξιά
        if (Input.GetKey(KeyCode.I)) moveZ = 1f;  // Εμπρός
        if (Input.GetKey(KeyCode.K)) moveZ = -1f; // Πίσω

        movementDirection = new Vector3(moveX, 0f, moveZ).normalized;
    }

    void MoveBob()
    {
        // Υπολογισμός νέας θέσης
        Vector3 newPosition = transform.position + movementDirection * moveSpeed * Time.deltaTime;

        // Περιορισμός στα όρια του λαβυρίνθου
        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
        newPosition.z = Mathf.Clamp(newPosition.z, minBounds.z, maxBounds.z);

        // Έλεγχος αν υπάρχει εμπόδιο στην κατεύθυνση κίνησης
        if (!IsWallInDirection(movementDirection))
        {
            // Εφαρμόζουμε τη νέα θέση μόνο αν δεν υπάρχει τοίχος
            transform.position = newPosition;
        }
    }


    bool IsWallInDirection(Vector3 direction)
    {
        if (direction == Vector3.zero) return true; // Αν δεν υπάρχει κίνηση, δεν ελέγχουμε
        // Χρήση SphereCast για ανίχνευση εμποδίων
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, detectionRadius, direction, out hit, moveSpeed * Time.deltaTime, wallLayer))
        {
            Debug.DrawRay(transform.position, direction * detectionRadius, Color.red); // Ορατότητα για testing
            return true; // Υπάρχει εμπόδιο
        }
        return false; // Δεν υπάρχει εμπόδιο
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something touched the trap: " + other.name); // Έλεγχος ονόματος αντικειμένου
        if (other.CompareTag("trap"))
        {
            Debug.Log("Game Over! Bob touched a death trap.");
            EndGame();
        }
    }


    public void EndGame()
    {
        // Τερματίζει το παιχνίδι παγώνοντας τον χρόνο
        Debug.Log("Game Over! The game is ending...");
        Time.timeScale = 0;
    }

        
}
