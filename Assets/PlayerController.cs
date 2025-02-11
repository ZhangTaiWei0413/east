﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject BOOM;
    private BallBehavior ballScript;
    public bool attack_flag = false;
    public bool slide_attack_flag = false;
    public bool bullet_flag = false;
    public bool boom_flag = false;
    public bool slide_l_or_r;
    public bool bullet_exists = false;

    float attack_time = 0.3f;
    float slide_attack_time = 0.15f;
    float bullet_attack_time = 0.3f;
    float boom_attack_time = 0.3f;
    float boom_attack_cool = 5.0f;
    float attack_cnt = 0.0f;
    float slide_attack_cnt = 0.0f;
    float bullet_attack_cnt = 0.0f;
    float boom_attack_cnt = 0.0f;

    public float playermove = 9.0f;
    public float playerslide = 27.0f;
    public int score;
    public int life;
    private Rigidbody2D rb;

    public Sprite idleSprite;
    public Sprite[] walkSprites;   
    public Sprite[] attackSprites;  
    public Sprite[] bulletSprites; 
    public Sprite[] slideSprites;   

    public Vector2 idleSize = Vector2.one;
    public Vector2 walkSize = Vector2.one;
    public Vector2 attackSize = Vector2.one;
    public Vector2 slideSize = Vector2.one;
    public Vector2 bulletSize = Vector2.one;

    public Vector2 idleOffset = Vector2.zero;
    public Vector2 walkOffset = Vector2.zero;
    public Vector2 attackOffset = Vector2.zero;
    public Vector2 slideOffset = Vector2.zero;
    public Vector2 bulletOffset = Vector2.zero;

    private float walkTimer = 0.0f;
    public float walkswitch = 0.25f;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        attack_cnt = 0.0f;
        slide_attack_cnt = 0.0f;
        bullet_attack_cnt = 0.0f;
        boom_attack_cnt = 0.0f;
        life = 20;
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        spriteRenderer = transform.Find("SpriteContainer").GetComponent<SpriteRenderer>();
        SetSprite(idleSprite, idleSize, idleOffset);
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        if (ball != null)
        {
            ballScript = ball.GetComponent<BallBehavior>();
        }
    }

    void Update()
    {
        attack_cnt += Time.deltaTime;
        slide_attack_cnt += Time.deltaTime;
        bullet_attack_cnt += Time.deltaTime;
        boom_attack_cnt += Time.deltaTime;
        bool hasLaunched = ballScript != null && ballScript.hasLaunched;

        if (attack_cnt >= attack_time)
        {
            attack_flag = false;
        }
        if (slide_attack_cnt >= slide_attack_time)
        {
            slide_attack_flag = false;
        }
        if (bullet_attack_cnt >= bullet_attack_time)
        {
            bullet_flag = false;
        }
        if (boom_attack_cnt >= boom_attack_cool)
        {
            boom_flag = false;
        }

        //子彈 > 滑行 > 拍球 > 移動
        if (!bullet_flag)
        {
            if (hasLaunched && Input.GetKey(KeyCode.E) && !bullet_exists)
            {
                rb.velocity = Vector2.zero;
                attack_flag = false;
                slide_attack_flag = false;
                bullet_flag = true;
                bullet_exists = true;
                bullet_attack_cnt = 0.0f;
                if (bulletPrefab != null)
                {
                    Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                }
            }
            else if (!slide_attack_flag)
            {
                if (Input.GetKey(KeyCode.A))
                {
                    if (hasLaunched && Input.GetKey(KeyCode.LeftShift))
                    {
                        attack_flag = false;
                        slide_l_or_r = true;
                        slide_attack_flag = true;
                        slide_attack_cnt = 0.0f;
                        spriteRenderer.flipX = true;
                    }
                    else if (!attack_flag)
                    {
                        if (Input.GetKeyDown(KeyCode.A))
                        {
                            walkTimer = 0;
                        }
                        walkTimer += Time.deltaTime;
                        spriteRenderer.flipX = true;
                        rb.velocity = new Vector2(-playermove, rb.velocity.y);
                    }
                }

                if (Input.GetKey(KeyCode.D))
                {
                    if (hasLaunched && Input.GetKey(KeyCode.LeftShift))
                    {
                        attack_flag = false;
                        slide_l_or_r = false;
                        slide_attack_flag = true;
                        slide_attack_cnt = 0.0f;
                        spriteRenderer.flipX = false;
                    }
                    else if (!attack_flag)
                    {
                        if (Input.GetKeyDown(KeyCode.D))
                        {
                            walkTimer = 0;
                        }
                        walkTimer += Time.deltaTime;
                        spriteRenderer.flipX = false;
                        rb.velocity = new Vector2(playermove, rb.velocity.y);
                    }
                }

                if (Input.GetKey(KeyCode.Space) && !attack_flag)
                {
                    rb.velocity = Vector2.zero;
                    attack_flag = true;
                    attack_cnt = 0.0f;
                    GameObject ball = GameObject.FindWithTag("Ball");
                    if (ball != null)
                    {
                        float playerX = transform.position.x;
                        float ballX = ball.transform.position.x;
                        spriteRenderer.flipX = playerX > ballX;
                    }
                }
            }
        }

        if (slide_attack_flag)
        {
            if (slide_l_or_r)
            {
                rb.velocity = new Vector2(-playerslide, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(playerslide, rb.velocity.y);
            }
        }

        if (hasLaunched && Input.GetKeyDown(KeyCode.Q) && !boom_flag)
        {
            boom_flag = true;
            boom_attack_cnt = 0.0f;
            GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
            foreach (GameObject ball in balls)
            {
                if (BOOM != null)
                {
                    GameObject tempBall = Instantiate(BOOM, ball.transform.position, Quaternion.identity);
                    Destroy(tempBall, boom_attack_time);
                }
            }
        }

        if (!slide_attack_flag && !attack_flag && !bullet_flag)
        {
            if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                walkTimer = 0;
                rb.velocity = Vector2.zero;
            }
        }

        if (bullet_flag)
        {
            int frame = Mathf.FloorToInt((bullet_attack_cnt / bullet_attack_time) * bulletSprites.Length);
            if (frame >= bulletSprites.Length) frame = bulletSprites.Length - 1;
            SetSprite(bulletSprites[frame], bulletSize, bulletOffset);
        }
        else if (slide_attack_flag)
        {
            int frame = Mathf.FloorToInt((slide_attack_cnt / slide_attack_time) * slideSprites.Length);
            if (frame >= slideSprites.Length) frame = slideSprites.Length - 1;
            SetSprite(slideSprites[frame], slideSize, slideOffset);
        }
        else if (attack_flag)
        {
            int frame = Mathf.FloorToInt((attack_cnt / attack_time) * attackSprites.Length);
            if (frame >= attackSprites.Length) frame = attackSprites.Length - 1;
            SetSprite(attackSprites[frame], attackSize, attackOffset);
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            int cycle = ((int)(walkTimer / walkswitch)) % 4;
            int index = (cycle == 3) ? 1 : cycle;
            SetSprite(walkSprites[index], walkSize, walkOffset);
        }
        else
        {
            SetSprite(idleSprite, idleSize, idleOffset);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool hasLaunched = ballScript != null && ballScript.hasLaunched;
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody2D ballRb = collision.rigidbody;
            if (attack_flag || slide_attack_flag)
            {
                Vector2 collisionPoint = collision.contacts[0].point;
                Vector2 playerPosition = transform.position;
                Vector2 collisionDirection = (collisionPoint - playerPosition).normalized;

                float angle = Mathf.Atan2(collisionDirection.y, collisionDirection.x) * Mathf.Rad2Deg;
                float horizontalFactor = Mathf.Abs(Mathf.Sin(angle * Mathf.Deg2Rad));
                float verticalFactor = Mathf.Abs(Mathf.Cos(angle * Mathf.Deg2Rad));
                float horizontalStrength = Mathf.Lerp(1.0f, 3.0f, horizontalFactor);
                float verticalStrength = Mathf.Lerp(3.0f, 1.0f, verticalFactor);

                Vector2 forceDirection = new Vector2(collisionDirection.x * horizontalStrength, verticalStrength).normalized;

                float ballSpeed = ballRb.velocity.magnitude;
                float speedMultiplier = Mathf.Clamp(ballSpeed / 5.0f, 0.7f, 1.6f);

                if (ballSpeed < 2.0f)
                {
                    speedMultiplier = 1.2f;
                }

                float normalizedRotationFactor = Mathf.Abs(Mathf.Sin(angle * Mathf.Deg2Rad));
                float rotationSpeed = Mathf.Lerp(10f, 80f, normalizedRotationFactor);
                float rotationMultiplier = 8.0f;
                float angularForce = rotationSpeed * rotationMultiplier * -Mathf.Sign(collisionDirection.x);

                ballRb.velocity = Vector2.zero;
                ballRb.AddForce(forceDirection * 11.0f * speedMultiplier, ForceMode2D.Impulse);

                ballRb.angularVelocity = angularForce * 2.0f;
            }
            if (hasLaunched && !attack_flag && !slide_attack_flag)
            {
                LoseLife();
                if (ballRb.velocity.magnitude > 11.0f)
                {
                    ballRb.velocity *= 0.6f;
                    ballRb.angularVelocity *= 0.6f;
                }
            }
        }
    }

    void SetSprite(Sprite newSprite, Vector2 size, Vector2 offset)
    {
        spriteRenderer.sprite = newSprite;
        spriteRenderer.transform.localScale = size;
        spriteRenderer.transform.localPosition = (Vector3)offset;
    }

    public void LoseLife()
    {
        life -= 1;
        Debug.Log(life);
        if (life <= 0)
        {
            Debug.Log("die");
        }
    }
}
