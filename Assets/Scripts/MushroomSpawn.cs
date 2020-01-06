using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomSpawn : MonoBehaviour
{
    public int minX = 1;
    public int maxX = 8;
    public int minY = 3;
    public int maxY = 8;
    public int maxMushroom = 7;
    public GameObject objectToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        RandomLocationGen();
    }

    void RandomLocationGen()
    {
        int count = 0;
        while (count < maxMushroom)
        {
            int x = Random.Range(minX, maxX);
            int y = Random.Range(minY, maxY);
            Instantiate(objectToSpawn, new Vector3(x, y, 0), Quaternion.identity);
            count++;
        }
    }
}
