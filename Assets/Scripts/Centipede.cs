using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centipede : MonoBehaviour
{
    public bool isCollide;                              // Boolean determining whether the centipede hits something
    public bool isDown;                                 // Boolean determining the direction (Top -> Bottom OR Bottom -> Top)
    public bool isRight;                                // Boolean determining the direction (Left -> Right OR Right -> Left)
    public int order;
    public float centipedeSpeed = 3f;                   // Speed that the centipede can travel in cells per second
    public Vector3 direction;                           // Current direction of the centipede

    private float nextMove = 0;                         // Time the centipede can move to the next cell
    private Rigidbody2D rb;                             // Rigidbody of the centipede object
    private GridGeneration gridInfo;

    // Start is called before the first frame update
    void Start()
    {
        // Set the boolean properties
        isCollide = false;
        isDown = true;
        isRight = true;
        rb = GetComponent<Rigidbody2D>();
        gridInfo = GameObject.Find("Background").GetComponent<GridGeneration>();
    }

    // Update is called once per frame
    void Update()
    {
        // Determine the direction of the centipede before moving
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

        // Move the centipede by the speed (units / s)
        if (Time.time > nextMove)
        {
            nextMove = Time.time + 1f / centipedeSpeed;
            rb.MovePosition(transform.position + direction);
            // After moving to the next row, immediately change to continue moving in the same row
            isCollide = false;
        }
    }

    // Check if the centipede can move to the selected direction
    void CheckMovable(Vector3 direction)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, 1);
        foreach (RaycastHit2D h in hits)
        {
            // Ignore the centipede itself
            if (!h.collider.gameObject.CompareTag("Centipede"))
            {
                // Change the horizontal direction between left and right
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

    void OnCollisionEnter2D(Collision2D col)
    {
        // Hit by a bullet will destroy this piece of centipede
        if (col.gameObject.CompareTag("Bullet"))
        {
            GameObject[] centipede = GameObject.FindGameObjectsWithTag("Centipede");
            for (int i = 0; i < centipede.Length; i++)
            {
                if (i < order)
                {
                    centipede[i].GetComponent<Centipede>().isRight = isRight == true ? false : true;
                }
            }
            Destroy(gameObject);
        }
    }
}
