using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float cellSize = 1f;
    public float bulletSpeed = 5f;      // Speed that a bullet can travel in unit cells per second
    public float fireRate = 0.25f;      // Fire rate for the bullet in seconds
    public int lives = 3;               // Lives given for the player
    public GameObject bulletPrefab;     // Prefab of a bullet

    private float nextFire;             // Time the next bullet can be fired
    private Rigidbody2D rb;             // Rigidbody of the player object

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        // If no fire rate
        if (fireRate == 0 && Input.GetButtonDown("Fire1"))
        {
            FireBullet();
        }
        // Fire rate specified
        else
        {
            if (Input.GetButton("Fire1") && Time.time > nextFire && fireRate > 0)
            {
                nextFire = Time.time + fireRate;
                FireBullet();
            }
        }
        CheckGameOver();
    }

    // Movement control function
    void Movement()
    {
        if (Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") > 0)
        {
            transform.position += Vector3.up * cellSize;
        }
        else if (Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") < 0)
        {
            transform.position += Vector3.down * cellSize;
        }
        else if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") < 0)
        {
            transform.position += Vector3.left * cellSize;
        }
        else if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") > 0)
        {
            transform.position += Vector3.right * cellSize;
        }
    }

    // Fire a bullet from player
    void FireBullet()
    {
        // Instantiate new bullet to fire from player
        GameObject newBullet = Instantiate(bulletPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        // Get Rigidbidy2D of the bullet to fire in up direction
        Rigidbody2D bulletRb = newBullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = Vector2.up * bulletSpeed;
    }

    // Detect collision with another Rigidbody2D
    void OnCollisionEnter2D(Collision2D col)
    {
        // Compare tag to detect whether the player collides with the centipede
        if (col.gameObject.CompareTag("Centipede"))
        {
            lives--;
        }
    }

    // Check the conditions whether it is game over
    void CheckGameOver()
    {
        if (lives <= 0)
        {

        }
    }
}
