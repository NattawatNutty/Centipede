using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Player properties
    public int lives;                           // Lives given for the player
    public GameObject remainingLifeText;        // Text object to show the remaining lives
    public float playerSpeed = 5f;              // Speed of the player in cells per second
    public int score = 0;                       // Score of the player
    public GameObject scoreText;                // Text object to show the remaining lives
    private float limitRatio = 0.85f;           // The height limit the player can move up from the bottom
    private float nextMove = 0;                 // Time the player can move to the next cell
    private Rigidbody2D rb;                     // Rigidbody of the player object
    private Vector3 direction;                  // Direction to move

    // Gun properties
    public float bulletSpeed = 10f;             // Speed that a bullet can travel in cells per second
    public float fireRate = 0.25f;              // Fire rate for the bullet in seconds
    public GameObject bulletPrefab;             // Prefab of a bullet
    private float nextFire;                     // Time the next bullet can be fired

    private GridGeneration gridInfo;
    private Scene currentScene;

    // Start is called before the first frame update
    void Start()
    {
        lives = PlayerPrefs.GetInt("PlayerLife", lives);
        currentScene = SceneManager.GetActiveScene();
        // Get Rigidbody2D of the player
        rb = GetComponent<Rigidbody2D>();
        gridInfo = GameObject.Find("Background").GetComponent<GridGeneration>();
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
        // Show the remaining lives
        Text lifeTxt = remainingLifeText.GetComponent<Text>();
        lifeTxt.text = lives.ToString();
        Text scoreTxt = scoreText.GetComponent<Text>();
        scoreTxt.text = score.ToString();
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
        // Check if the player faces with obstacle
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, 1);
        foreach (RaycastHit2D h in hits)
        {
            // Ignore its collider
            if (!h.collider.gameObject.CompareTag("Player"))
            {
                if (h.collider.gameObject.CompareTag("Centipede"))
                {
                    lives--;
                    PlayerPrefs.SetInt("PlayeLife", lives);
                    PlayerPrefs.Save();
                    SceneManager.LoadScene(currentScene.name);
                }
                return false;
            }
        }

        if(direction == Vector3.up && transform.position.y + 1 > Math.Floor(limitRatio * gridInfo.numGrid))
        {
            return false;
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
        Debug.Log(col.gameObject.tag);
        // Compare tag to detect whether the player collides with the centipede
        if (col.gameObject.CompareTag("Centipede"))
        {
            lives--;
            PlayerPrefs.SetInt("PlayeLife", lives);
            PlayerPrefs.Save();
            SceneManager.LoadScene(currentScene.name);
        }
    }

    // Check the conditions whether it is game over
    void CheckGameOver()
    {
        GameObject[] centipede = GameObject.FindGameObjectsWithTag("Centipede");
        if (lives <= 0 || centipede.Length == 0)
        {
            
        }
    }
}
