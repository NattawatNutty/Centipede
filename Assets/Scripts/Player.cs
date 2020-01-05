using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float cellSize = 0.5f;
    public float bulletSpeed;       // Speed that a bullet can travel in cells per second
    public int lives = 3;           // Lives given for the player (default = 3)

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") > 0)
        {
            transform.position += Vector3.up * cellSize;
        }
        else if(Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") < 0)
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

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("COL");
    }
}
