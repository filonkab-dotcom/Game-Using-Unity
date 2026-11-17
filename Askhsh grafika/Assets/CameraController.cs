using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10f;   // Ταχύτητα κίνησης της κάμερας
    public float rotationSpeed = 50f; // Ταχύτητα περιστροφής

    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        // Μετακίνηση στους άξονες X και Z (βελάκια)
        float moveX = Input.GetAxis("Horizontal"); // Δεξιά / Αριστερά (Arrow Keys)
        float moveZ = Input.GetAxis("Vertical");   // Μπροστά / Πίσω (Arrow Keys)

        // Μετακίνηση στον άξονα Y με + και -
        float moveY = 0f;
        if (Input.GetKey(KeyCode.Equals) || Input.GetKey(KeyCode.KeypadPlus)) // + Πλήκτρο
        {
            moveY = 1f;
        }
        else if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus)) // - Πλήκτρο
        {
            moveY = -1f;
        }

        // Υπολογισμός κίνησης
        Vector3 moveDirection = new Vector3(moveX, moveY, moveZ).normalized;

        // Εφαρμογή της κίνησης
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    void HandleRotation()
    {
        // Περιστροφή γύρω από τον Y άξονα με το R
        if (Input.GetKey(KeyCode.R))
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
}
