using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrap : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something touched the trap: " + other.name); // Έλεγχος ονόματος αντικειμένου
        if (other.CompareTag("Trap"))
        {
            Debug.Log("Game Over! Bob touched a death trap.");
            EndGame();
        }
    }


    void EndGame()
    {
        // Τερματίζει το παιχνίδι παγώνοντας τον χρόνο
        Debug.Log("Game Over! The game is ending...");
        Time.timeScale = 0;

        // Προαιρετικά: Εμφάνιση μηνύματος στην οθόνη
        // (θα χρειαστεί σύστημα UI για να εμφανιστεί μήνυμα "Game Over").
    }
}

