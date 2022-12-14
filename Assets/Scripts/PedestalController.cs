using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestalController : MonoBehaviour
{
    public Sprite swordReturnedSprite;
    public event GameState GameWin;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {
            GetComponent<SpriteRenderer>().sprite = swordReturnedSprite;
            FindObjectOfType<SwordController>().gameObject.SetActive(false);
            GameWin();
        }
    }
}
