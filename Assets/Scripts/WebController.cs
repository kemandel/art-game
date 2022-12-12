using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebController : MonoBehaviour
{
    private void Update()
    {
        if (FindObjectOfType<PlayerController>().transform.position.y > transform.position.y)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 6;
        }
        else
        {
            GetComponent<SpriteRenderer>().sortingOrder = 2;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {
            GetComponent<Animator>().SetBool("Break", true);
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
