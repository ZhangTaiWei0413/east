using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerController player;
    public float speed = 0.75f;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, speed);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Bullet"), LayerMask.NameToLayer("the cube"), true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
            Debug.Log(collision.gameObject.name); 

            if (collision.gameObject.CompareTag("Ball"))
            {
                rb.velocity = new Vector2(0, speed);
                collision.rigidbody.AddForce(Vector2.up * 10.0f, ForceMode2D.Impulse);
            }

            if (collision.gameObject.CompareTag("Wall"))
            {
                player.bullet_exists = false;
                Destroy(gameObject);
            }
    }
}
