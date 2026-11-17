using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public GameObject wallPrefab; // Drag and drop the wall prefab here in Unity Inspector
    public Vector2 floorSize = new Vector2(100, 100); // Το μέγεθος του πατώματος

    void Start()
    {
        GenerateMaze();
    }

    void GenerateMaze()
    {
        int[,] mazeLayout = {
    {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
    {1, 0, 1, 0, 1, 1, 0, 0, 0, 1},
    {1, 0, 0, 0, 0, 0, 0, 1, 0, 0},
    {1, 0, 1, 1, 0, 1, 1, 1, 0, 1},
    {1, 0, 0, 0, 0, 1, 0, 0, 0, 1},
    {1, 0, 1, 0, 1, 1, 0, 1, 0, 1},
    {1, 0, 1, 0, 0, 0, 0, 1, 0, 1},
    {0, 0, 1, 1, 1, 1, 0, 1, 0, 1},
    {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
    {1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
    };


        // Υπολογισμός του μεγέθους κάθε κελιού ώστε να καλύπτει το πάτωμα
        int rows = mazeLayout.GetLength(0); // Αριθμός γραμμών
        int cols = mazeLayout.GetLength(1); // Αριθμός στηλών

        float cellWidth = floorSize.x / cols; // Πλάτος κάθε κελιού
        float cellHeight = floorSize.y / rows; // Ύψος κάθε κελιού

        // Αρχική θέση για να κεντράρουμε τον λαβύρινθο
        Vector3 startPosition = new Vector3(-floorSize.x / 2 + cellWidth / 2, 0, -floorSize.y / 2 + cellHeight / 2);

        // Δημιουργία τοίχων
        for (int z = 0; z < rows; z++)
        {
            for (int x = 0; x < cols; x++)
            {
                if (mazeLayout[z, x] == 1) // Αν είναι τοίχος
                {
                    Vector3 position = new Vector3(startPosition.x + x * cellWidth, startPosition.y, startPosition.z + z * cellHeight);
                    CreateWall(position.x, position.y, position.z, cellWidth, cellHeight);
                }
            }
        }
    }

    void CreateWall(float x, float y, float z, float width, float height)
    {
        // Δημιουργία τοίχου
        GameObject wall = Instantiate(wallPrefab, new Vector3(x, y + 0.1f, z), Quaternion.identity);
        wall.transform.localScale = new Vector3(width, 10, height); // Κλιμάκωση για να ταιριάζει στο κελί
        wall.GetComponent<Renderer>().material.color = Color.blue; // Ορισμός χρώματος
    }
}


