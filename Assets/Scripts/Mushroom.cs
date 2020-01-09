using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public int mushroomLife = 3;    // Life point of a mushroom
    public int scoreMushroom = 15;  // Score of each mushroom when the player shot down
    private Player playerInfo;      // Player information

    void Start()
    {
        playerInfo = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        // If HP <= 0, destroy the mushroom
        if (this.mushroomLife <= 0)
        {
            Destroy(gameObject);
            playerInfo.score += scoreMushroom;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // Hit by a bullet will decrease the health point of mushroom
        if (col.gameObject.CompareTag("Bullet"))
        {
            this.mushroomLife--;
        }
    }
}
