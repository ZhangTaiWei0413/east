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
    private SpriteRenderer spriteRenderer; // 存储 SpriteRenderer 组件
    private Color originalColor; // 记录原本颜色

    void Start()
    {
        attack_cnt = 0.0f;
        slide_attack_cnt = 0.0f;
        bullet_attack_cnt = 0.0f;
        boom_attack_cnt = 0.0f;
        spriteRenderer = GetComponent<SpriteRenderer>(); // 获取 SpriteRenderer
        originalColor = spriteRenderer.color; // 记录初始颜色
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
            spriteRenderer.color = originalColor; // 恢复原本颜色
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
                spriteRenderer.color = new Color(1.0f, 0.5f, 0.0f);
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
                        spriteRenderer.color = Color.green;
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
                        spriteRenderer.color = Color.green;
                    }
                    else if (attack_flag == false)
                    {
                        transform.localPosition += Vector3.right * 0.1f;
                    }

                }

                if (Input.GetKey(KeyCode.Space))
                {
                    attack_flag = true;
                    attack_cnt = 0.0f;
                    spriteRenderer.color = Color.magenta; // 攻击时变红
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
                collision.rigidbody.AddForce(Vector2.up * 10.0f, ForceMode2D.Impulse);
            }
        }
    }
}
