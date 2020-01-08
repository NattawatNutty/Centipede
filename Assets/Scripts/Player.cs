using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Grid
    public int numGrid = 30;                    // Grid dimension
    public GameObject cam;                      // Main camera object
    public GameObject bg;                       // Background
    private int gridWithPadding;                 // Grid with padding each side
    private float limitRatio = 0.15f;           // The height limit the player can move up to from the top

    // Mushroom generation
    public int maxMushroom = 30;                // Max number of mushrooms
    public GameObject objectToSpawn;            // Mushroom prefab to spawn

    // Player properties
    public bool movable = true;                 // Decide whether the player can move against that direction
    public float bulletSpeed = 5f;              // Speed that a bullet can travel in cells per second
    public float fireRate = 0.25f;              // Fire rate for the bullet in seconds
    public int lives = 3;                       // Lives given for the player
    public GameObject bulletPrefab;             // Prefab of a bullet

    private float nextFire;                     // Time the next bullet can be fired
    private Rigidbody2D rb;                     // Rigidbody of the player object

    // Start is called before the first frame update
    void Start()
    {
        gridWithPadding = numGrid + 2;
        // Change the position of the camera and background by the grid dimension
        float newPos = numGrid / 2f + 0.5f;
        bg.transform.position = new Vector3(newPos, newPos, 1f);
        cam.transform.position = new Vector3(newPos, newPos, -10f);
        // Resize the background by the grid dimension
        bg.transform.localScale = new Vector3(gridWithPadding / 10f, gridWithPadding / 10f, gridWithPadding / 10f);
        // Resize the camera by the grid dimension with padding 1 unit each size
        Camera camComponent = cam.GetComponent<Camera>();
        camComponent.orthographicSize = gridWithPadding / 2f;
        // Get Rigidbody2D of the player
        rb = GetComponent<Rigidbody2D>();
        MushroomGen();
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

    // Generate the mushrooms in random positions
    void MushroomGen()
    {
        int count = 0;
        while (count < maxMushroom)
        {
            int x = Random.Range(0, numGrid);
            // Leave 2 bottom rows for space to shoot
            int y = Random.Range(2, numGrid);
            Instantiate(objectToSpawn, new Vector3(x, y, 0), Quaternion.identity);
            count++;
        }
    }

    // Movement control function
    void Movement()
    {
        if (CheckMovable(Vector3.up) && Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") > 0)
        {
            rb.MovePosition(transform.position + Vector3.up);
        }
        else if (CheckMovable(Vector3.down) && Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") < 0)
        {
            rb.MovePosition(transform.position + Vector3.down);
        }
        else if (CheckMovable(Vector3.left) && Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") < 0)
        {
            rb.MovePosition(transform.position + Vector3.left);
        }
        else if (CheckMovable(Vector3.right) && Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") > 0)
        {
            rb.MovePosition(transform.position + Vector3.right);
        }
    }

    bool CheckMovable(Vector3 direction)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, 1);
        foreach (RaycastHit2D h in hits)
        {
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
