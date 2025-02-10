using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject BOOM;
    public bool attack_flag = false;
    public bool slide_attack_flag = false;
    public bool bullet_flag = false;
    public bool boom_flag = false;
    public bool slide_l_or_r;
    public bool bullet_exists = false;
    float attack_time = 0.5f;
    float slide_attack_time = 0.3f;
    float bullet_attack_time = 0.3f;
    float boom_attack_time = 0.3f;
    float boom_attack_cool = 5.0f;
    float attack_cnt = 0.0f;
    float slide_attack_cnt = 0.0f;
    float bullet_attack_cnt = 0.0f;
    float boom_attack_cnt = 0.0f;
    //圖片控制
    public Sprite idleSprite;   
    public Sprite walkSprite1; 
    public Sprite walkSprite2; 
    public Sprite attackSprite;
    public Sprite slideSprite; 
    public Sprite bulletSprite; 

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
        spriteRenderer = transform.Find("SpriteContainer").GetComponent<SpriteRenderer>();
        SetSprite(idleSprite, idleSize, idleOffset);
    }

    void Update()//攻擊狀態
    {
        attack_cnt += Time.deltaTime;
        slide_attack_cnt += Time.deltaTime;
        bullet_attack_cnt += Time.deltaTime;
        boom_attack_cnt += Time.deltaTime;

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

        if (slide_attack_cnt >= slide_attack_time && attack_cnt >= attack_time && bullet_attack_cnt >= bullet_attack_time)
        {
        }

        if (Input.GetKeyDown(KeyCode.Q) && boom_flag == false)
        {
            boom_flag = true;
            boom_attack_cnt = 0.0f;
            CreateTemporaryBall();
        }

        void CreateTemporaryBall()
        {
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
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                walkTimer += Time.deltaTime;
                if (walkTimer >= walkswitch)
                {
                    walkTimer = 0;
                    SetSprite((spriteRenderer.sprite == walkSprite1) ? walkSprite2 : walkSprite1, walkSize, walkOffset);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    spriteRenderer.flipX = false;
                }
                else
                {
                    spriteRenderer.flipX = true;
                }
            }
            else
            {
                SetSprite(idleSprite, idleSize, idleOffset);
            }     
        }
    }

    private void FixedUpdate()//移動&攻擊指令
    {
        if (bullet_flag == false) 
        {
            if (Input.GetKey(KeyCode.E) && bullet_exists == false)
            {
                attack_flag = false;
                slide_attack_flag = false;
                bullet_flag = true;
                bullet_exists = true;
                bullet_attack_cnt = 0.0f;
                SetSprite(bulletSprite, bulletSize, bulletOffset);
                InstantiateBullet();
                
            }
            else if (slide_attack_flag == false)
            {
                if (Input.GetKey(KeyCode.A))
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        attack_flag = false;
                        slide_l_or_r = true;
                        slide_attack_flag = true;
                        slide_attack_cnt = 0.0f;
                        SetSprite(slideSprite, slideSize, slideOffset);
                        spriteRenderer.flipX = true;
                    }
                    else if (attack_flag == false)
                    {
                        transform.localPosition += Vector3.left * 0.1f;
                    }
                }

                if (Input.GetKey(KeyCode.D))
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        attack_flag = false;
                        slide_l_or_r = false;
                        slide_attack_flag = true;
                        slide_attack_cnt = 0.0f;
                        SetSprite(slideSprite, slideSize, slideOffset);
                        spriteRenderer.flipX = false;
                    }
                    else if (attack_flag == false)
                    {
                        transform.localPosition += Vector3.right * 0.1f;
                    }

                }

                if (Input.GetKey(KeyCode.Space) && attack_flag == false)
                {
                    attack_flag = true;
                    attack_cnt = 0.0f;
                    GameObject ball = GameObject.FindWithTag("Ball");
                    if (ball != null)
                    {
                        float playerX = transform.position.x;
                        float ballX = ball.transform.position.x;
                        spriteRenderer.flipX = playerX > ballX;
                    }

                    SetSprite(attackSprite, attackSize, attackOffset);
                }
            }

        }

        if (slide_attack_flag)
        {
            if ((slide_l_or_r))
            {
                transform.localPosition += Vector3.left * 0.3f;
            }
            else
            {
                transform.localPosition += Vector3.right * 0.3f;
            }
        }

        void InstantiateBullet()
        {
            if (bulletPrefab != null)
            {
                Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)//攻擊效果
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (attack_flag || slide_attack_flag)
            {
                Rigidbody2D ballRb = collision.rigidbody;

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
                float speedMultiplier = Mathf.Clamp(ballSpeed / 5.0f, 0.8f, 1.5f);

                if (ballSpeed < 2.0f)
                {
                    speedMultiplier = 1.2f;
                }

                float normalizedRotationFactor = Mathf.Abs(Mathf.Sin(angle * Mathf.Deg2Rad));
                float rotationSpeed = Mathf.Lerp(10f, 80f, normalizedRotationFactor);
                float rotationMultiplier = 8.0f;
                float angularForce = rotationSpeed * rotationMultiplier * -Mathf.Sign(collisionDirection.x);

                ballRb.velocity = Vector2.zero;
                ballRb.AddForce(forceDirection * 10.0f * speedMultiplier, ForceMode2D.Impulse);
                ballRb.angularVelocity = angularForce;
            }
        }
    }

    void SetSprite(Sprite newSprite, Vector2 size, Vector2 offset)
    {
        spriteRenderer.sprite = newSprite;
        spriteRenderer.transform.localScale = size;
        spriteRenderer.transform.localPosition = (Vector3)offset; 
    }
}
