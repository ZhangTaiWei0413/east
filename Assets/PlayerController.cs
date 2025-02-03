using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool attack_flag = false;
    float attack_time = 0.5f;
    float attack_cnt = 0.0f;
    private SpriteRenderer spriteRenderer; // 存储 SpriteRenderer 组件
    private Color originalColor; // 记录原本颜色

    void Start()
    {
        attack_cnt = 0.0f;
        spriteRenderer = GetComponent<SpriteRenderer>(); // 获取 SpriteRenderer
        originalColor = spriteRenderer.color; // 记录初始颜色
    }

    void Update()
    {
        attack_cnt += Time.deltaTime;

        if (attack_cnt >= attack_time)
        {
            attack_flag = false;
            spriteRenderer.color = originalColor; // 恢复原本颜色
        }
    }

    private void FixedUpdate()
    {
        if (attack_flag == false)
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.localPosition += Vector3.left * 0.4f;
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.localPosition += Vector3.right * 0.4f;
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            attack_flag = true;
            attack_cnt = 0.0f;
            spriteRenderer.color = Color.magenta; // 攻击时变红
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            if (attack_flag)
            {
                collision.rigidbody.AddForce(Vector2.up * 10.0f, ForceMode2D.Impulse);
            }
        }
    }
}
