using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Destroy the bullet when it hits to reduce the memory
    void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(gameObject);
    }
}
