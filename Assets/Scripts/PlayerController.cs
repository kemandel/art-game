using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;

    bool canMove;

    Rigidbody2D myRigidbody;
    Animator myAnimator;

    SwordController sword;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        sword = GetComponentInChildren<SwordController>();
        sword.SwordCollide += SwordKnockback;
        canMove = true;
    }

    void Update()
    {
        if (Mathf.Abs(sword.transform.localPosition.x / sword.swordDistance) < .5)
        {
            if (sword.transform.localPosition.y < 0)
            {
                myAnimator.SetInteger("State", 0);

            }
            else if (sword.transform.localPosition.y > 0)
            {

                myAnimator.SetInteger("State", 1);

            }
        }
        else if (sword.transform.localPosition.x / sword.swordDistance < -.5)
        {
            myAnimator.SetInteger("State", 2);
        }
        else if (sword.transform.localPosition.x / sword.swordDistance > .5)
        {
            myAnimator.SetInteger("State", 3);
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            if (sword.activeCoroutine == null)
            {
                Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
                Vector2 velocity = direction * speed;
                myRigidbody.velocity = velocity;
            }
            else
            {
                myRigidbody.velocity = Vector2.zero;
            }
        }
    }

    void SwordKnockback(Vector3 position)
    {
        StartCoroutine(KnockBackCoroutine(3, .1f, -(position - transform.position)));
    }

    IEnumerator KnockBackCoroutine(float kbAmount, float duration, Vector2 direction)
    {
        canMove = false;
        myRigidbody.velocity = Vector2.zero;
        float startTime = Time.time;
        float passedTime = Time.time - startTime;
        do
        {
            myRigidbody.velocity = direction.normalized * kbAmount;
            passedTime = Time.time - startTime;
            yield return null;
        } while (passedTime < duration);
        canMove = true;
    }
}