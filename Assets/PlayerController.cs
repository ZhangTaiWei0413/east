using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    bool attack_flag = false;
    float attack_time = 1.0f;
    float attack_cnt = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        attack_cnt = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        attack_cnt += Time.deltaTime;

        if (attack_cnt >= attack_time)
        {
            attack_flag = false;
            //GetComponent<SpriteRenderer>().sprite = false;
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.localPosition += Vector3.left * 0.3f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.localPosition += Vector3.right * 0.3f;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            attack_flag = true;
            attack_cnt=0.0f;

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {/*
            collision.rigidbody.AddForce(Vector2.up * 10.0f,ForceMode2D.Impulse);*/
        }
    }
}


