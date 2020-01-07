using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public int mushroomLife = 3;    // Life point of a mushroom

    // Update is called once per frame
    void Update()
    {
        // If HP <= 0, destroy the mushroom
        if (this.mushroomLife <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Hit by a bullet will decrease the health point of mushroom
        if (col.gameObject.CompareTag("Bullet"))
        {
            this.mushroomLife--;
        }
    }
}
