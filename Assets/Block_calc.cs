using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_calc : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            Destroy(gameObject);
        }
    }
}
