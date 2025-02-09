using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boombehavior : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("the cube"))
        {
            Destroy(collision.gameObject);
        }
    }

}
