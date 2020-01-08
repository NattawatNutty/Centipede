using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Player properties
    public int lives = 3;                       // Lives given for the player
    public float playerSpeed = 5f;              // Speed of the player in cells per second
    private float limitRatio = 0.85f;           // The height limit the player can move up from the bottom
    private float nextMove = 0;                 // Time the player can move to the next cell
    private Rigidbody2D rb;                     // Rigidbody of the player object
    private Vector3 direction;

    // Gun properties
    public float bulletSpeed = 10f;             // Speed that a bullet can travel in cells per second
    public float fireRate = 0.25f;              // Fire rate for the bullet in seconds
    public GameObject bulletPrefab;             // Prefab of a bullet
    private float nextFire;                     // Time the next bullet can be fired

    // Start is called before the first frame update
    void Start()
    {
        // Get Rigidbody2D of the player
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        // If no fire rate
        if (fireRate == 0 && Input.GetButtonDown("Jump"))
        {
            FireBullet();
        }
        // Fire rate specified
        else
        {
            if (Input.GetButton("Jump") && Time.time > nextFire && fireRate > 0)
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
        // Move up
        if (CheckMovable(Vector3.up) && Input.GetButton("Vertical") && Input.GetAxisRaw("Vertical") > 0)
        {
            direction = Vector3.up;
        }
        // Move down
        else if (CheckMovable(Vector3.down) && Input.GetButton("Vertical") && Input.GetAxisRaw("Vertical") < 0)
        {
            direction = Vector3.down;
        }
        // Move left
        else if (CheckMovable(Vector3.left) && Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") < 0)
        {
            direction = Vector3.left;
        }
        // Move right
        else if (CheckMovable(Vector3.right) && Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") > 0)
        {
            direction = Vector3.right;
        }
        else
        {
            direction = Vector3.zero;
        }

        // Move the centipede by the speed (units / s)
        if (Time.time > nextMove)
        {
            nextMove = Time.time + 1f / playerSpeed;
            rb.MovePosition(transform.position + direction);
        }
    }

    // Check if the player can move to the selected direction
    bool CheckMovable(Vector3 direction)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, 1);
        foreach (RaycastHit2D h in hits)
        {
            // Ignore its collider
            if (!h.collider.gameObject.CompareTag("Player"))
            {
                return false;
            }
        }
        return true;
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
