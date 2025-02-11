using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool hasLaunched = false;
    public float launchForce = 10f;    
    public float launchYPosition = 2f; 
    private GameObject player;        

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        GetComponent<SpriteRenderer>().enabled = false;
    }

    void Update()
    {
        if (!hasLaunched)
        {
            transform.position = new Vector2(player.transform.position.x, launchYPosition);
        }

        if (!hasLaunched && Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = new Vector2(player.transform.position.x, launchYPosition);
            GetComponent<SpriteRenderer>().enabled = true;
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * launchForce, ForceMode2D.Impulse);
            hasLaunched = true;
        }
    }
}