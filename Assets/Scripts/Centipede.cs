using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centipede : MonoBehaviour
{
    public bool isCollide;                              // Boolean determining whether the centipede hits something
    public bool isDown;                                 // Boolean determining the direction (Top -> Bottom OR Bottom -> Top)
    public bool isRight;                                // Boolean determining the direction (Left -> Right OR Right -> Left)
    public int numOfCentipede = 15;                     // Number of the centipede length
    public float centipedeSpeed = 5f;                   // Speed that the centipede can travel in cells per second
    public Vector3 direction;

    private float nextMove = 0;
    private Vector3 initialPosition;
    private Rigidbody2D rb;                             // Rigidbody of the centipede object

    // Start is called before the first frame update
    void Start()
    {
        // Get grid dimension information
        GridGeneration gridInfo = GameObject.Find("Background").GetComponent<GridGeneration>();
        // Set the initial position for centipede's head
        initialPosition = new Vector3(numOfCentipede - 1, gridInfo.numGrid - 1, 0);
        transform.position = initialPosition;
        // Set the boolean properties
        isCollide = false;
        isDown = true;
        isRight = true;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the velocity of the centipede
        CheckMovable(direction);
        CentipedeMove();
    }

    void CentipedeMove()
    {
        // Move from left to right
        if (isRight)
        {
            direction = Vector3.right;
        }
        // Move from right to left
        else
        {
            direction = Vector3.left;
        }

        // Collide with something, need to change row
        if (isCollide)
        {
            // Move from top to bottom
            if (isDown)
            {
                direction = Vector3.down;
            }
            // Move from bottom to top
            else
            {
                direction = Vector3.up;
            }
        }

        if (Time.time > nextMove)
        {
            nextMove = Time.time + 1f / centipedeSpeed;
            rb.MovePosition(transform.position + direction);
            isCollide = false;
        }
    }

    void CheckMovable(Vector3 direction)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, 1);
        foreach (RaycastHit2D h in hits)
        {
            if (!h.collider.gameObject.CompareTag("Centipede"))
            {
                Debug.Log(h.collider.gameObject.name);
                if (h.collider.gameObject.CompareTag("Wall") || h.collider.gameObject.CompareTag("Mushroom"))
                {
                    isCollide = true;
                    isRight = isRight == true ? false : true;
                }
                // Change the vertical direction between up and down
                else if (h.collider.gameObject.name == "Top" || h.collider.gameObject.name == "Bottom")
                {
                    isDown = isDown == true ? false : true;
                }
            }
        }
    }
    /*
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col.gameObject.name);
        // Hit by a bullet will destroy this piece of centipede
        if (col.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
        // Change the horizontal direction between left and right for the next row
        else if (col.gameObject.CompareTag("Wall") || col.gameObject.CompareTag("Mushroom"))
        {
            isCollide = true;
            isRight = isRight == true ? false : true;
        }
        // Change the vertical direction between up and down
        else if (col.gameObject.name == "Top" || col.gameObject.name == "Bottom")
        {
            isDown = isDown == true ? false : true;
        }
    }*/
}
