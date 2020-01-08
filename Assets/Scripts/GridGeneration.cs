using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGeneration : MonoBehaviour
{
    // Grid
    public int numGrid = 30;                    // Grid dimension
    public GameObject cam;                      // Main camera object
    private float limitRatio = 0.15f;           // The height limit the player can move up to from the top

    // Mushroom generation
    public int maxMushroom = 30;                // Max number of mushrooms
    public GameObject objectToSpawn;            // Mushroom prefab to spawn

    // Start is called before the first frame update
    void Start()
    {
        // Change the position of the camera and background by the grid dimension
        float newPos = numGrid / 2f - 0.5f;
        Vector2 verticalOffset = new Vector2(newPos + 1, 0);
        Vector2 verticalSize = new Vector2(1, numGrid + 2);
        Vector2 horizontalOffset = new Vector2(0, newPos + 1);
        Vector2 horizontalSize = new Vector2(numGrid + 2, 1);

        CameraSettings(newPos);
        WallSettings(newPos, "Top", horizontalOffset, horizontalSize);
        WallSettings(newPos, "Bottom", -horizontalOffset, horizontalSize);
        WallSettings(newPos, "Left", -verticalOffset, verticalSize);
        WallSettings(newPos, "Right", verticalOffset, verticalSize);
        MushroomGen();
    }

    void CameraSettings(float newPos)
    {
        transform.position = new Vector3(newPos, newPos, 1);
        cam.transform.position = new Vector3(newPos, newPos, -10);
        // Resize the background by the grid dimension
        transform.localScale = new Vector3(numGrid / 10f, numGrid / 10f, numGrid / 10f);
        // Resize the camera by the grid dimension with padding 1 unit each size
        Camera camComponent = cam.GetComponent<Camera>();
        camComponent.orthographicSize = numGrid / 2f;
    }

    void WallSettings(float newPos, string name, Vector2 offset, Vector2 size)
    {
        GameObject wall = GameObject.Find(name);
        wall.transform.position = new Vector3(newPos, newPos, 0);
        BoxCollider2D collider = wall.AddComponent<BoxCollider2D>();
        collider.offset = offset;
        collider.size = size;
    }

    // Generate the mushrooms in random positions
    void MushroomGen()
    {
        int count = 0;
        while (count < maxMushroom)
        {
            int x = Random.Range(1, numGrid);
            // Leave 2 bottom rows for space to shoot
            int y = Random.Range(2, numGrid);
            Instantiate(objectToSpawn, new Vector3(x, y, 0), Quaternion.identity);
            count++;
        }
    }
}
