using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void MyDel(Vector3 position);  // delegate
public delegate void GameState();
public class SwordController : MonoBehaviour
{
    public float swordDistance;
    public float rotateTime;
    public Sprite[] swordStateSprites;

    public event MyDel SwordCollide;
    public event GameState GameLose;

    public Coroutine activeCoroutine;

    Vector3 oldPosition;
    Vector3 oldAngles;
    float currentRotateTime;
    int swordHealth;
    bool gameOver = false;

    BoxCollider2D myCollisionCollider;
    SpriteRenderer mySpriteRenderer;

    void Start()
    {
        oldPosition = transform.localPosition;
        oldAngles = transform.eulerAngles;
        myCollisionCollider = GetComponents<BoxCollider2D>()[0];
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        swordHealth = swordStateSprites.Length - 1;
        if (swordHealth > 0)
        {
            mySpriteRenderer.sprite = swordStateSprites[swordStateSprites.Length - 1 - swordHealth];
        }
    }

    private void Update()
    {
        if (activeCoroutine == null)
        {
            myCollisionCollider.enabled = true;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                activeCoroutine = StartCoroutine(RotateLeftCoroutine());
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                activeCoroutine = StartCoroutine(RotateRightCoroutine());
            }
        }
        else
        {
            myCollisionCollider.enabled = false;
        }
        if (transform.localPosition.y >= 0)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 3;
        }
        else
        {
            GetComponent<SpriteRenderer>().sortingOrder = 5;
        }

        if (gameOver)
        {
            foreach (BoxCollider2D b in GetComponentsInChildren<BoxCollider2D>())
            {
                b.enabled = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            LoseHealth();
            SwordCollide(transform.position);
            if (activeCoroutine != null)
            {
                StopCoroutine(activeCoroutine);
                activeCoroutine = null;
                MoveBack();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            LoseHealth();
            // Left of player
            if (transform.position.x < GetComponentInParent<PlayerController>().transform.position.x)
            {
                // Above wall
                if (transform.position.y > other.contacts[0].point.y)
                {
                    activeCoroutine = StartCoroutine(RotateRightCoroutine());
                }
                else if (transform.position.y < other.contacts[0].point.y)
                {
                    activeCoroutine = StartCoroutine(RotateLeftCoroutine());
                }
            }
            // Right of player
            else if (transform.position.x > GetComponentInParent<PlayerController>().transform.position.x)
            {
                // Above wall
                if (transform.position.y > other.contacts[0].point.y)
                {
                    activeCoroutine = StartCoroutine(RotateLeftCoroutine());
                }
                else if (transform.position.y < other.contacts[0].point.y)
                {
                    activeCoroutine = StartCoroutine(RotateRightCoroutine());
                }
            }
            // Above player
            else if (transform.position.y > GetComponentInParent<PlayerController>().transform.position.y)
            {
                // Left of wall
                if (transform.position.x < other.contacts[0].point.x)
                {
                    activeCoroutine = StartCoroutine(RotateLeftCoroutine());
                }
                else if (transform.position.x > other.contacts[0].point.x)
                {
                    activeCoroutine = StartCoroutine(RotateRightCoroutine());
                }
                else
                {
                    SwordCollide(other.contacts[0].point);
                }
            }
            // Below player
            else if (transform.position.y < GetComponentInParent<PlayerController>().transform.position.y)
            {
                // Left of wall
                if (transform.position.x < other.contacts[0].point.x)
                {
                    activeCoroutine = StartCoroutine(RotateRightCoroutine());
                }
                else if (transform.position.x > other.contacts[0].point.x)
                {
                    activeCoroutine = StartCoroutine(RotateLeftCoroutine());
                }
                else
                {
                    SwordCollide(other.contacts[0].point);
                }
            }
        }
    }

    void LoseHealth()
    {
        swordHealth--;
        if (swordHealth > 0)
        {
            mySpriteRenderer.sprite = swordStateSprites[swordStateSprites.Length - 1 - swordHealth];
        }
        else
        {
            mySpriteRenderer.sprite = swordStateSprites[swordStateSprites.Length - 1 - swordHealth];
            GameLose();
            gameOver = true;
        }
    }

    IEnumerator RotateLeftCoroutine()
    {
        Vector2 positionPolar = ToPolar(transform.localPosition);

        float startingTheta = positionPolar.y;
        float startTime = Time.time;
        float passedTime = Time.time - startTime;
        float initialAngle = transform.eulerAngles.z;
        do
        {
            passedTime = Time.time - startTime;
            currentRotateTime = passedTime;
            float lerpRatio = Mathf.Clamp01(passedTime / rotateTime);
            positionPolar.y = Mathf.Lerp(startingTheta, startingTheta + Mathf.PI / 2, lerpRatio);
            transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(initialAngle, initialAngle + 90, lerpRatio));
            transform.localPosition = ToCartesian(positionPolar);
            yield return null;
        } while (passedTime < rotateTime);
        transform.localPosition = new Vector2(Mathf.Round(transform.localPosition.x) * swordDistance, Mathf.Round(transform.localPosition.y) * swordDistance);
        transform.eulerAngles = new Vector3(0, 0, Mathf.Round(transform.transform.eulerAngles.z));
        oldAngles = transform.eulerAngles;
        oldPosition = transform.localPosition;
        activeCoroutine = null;
    }

    IEnumerator RotateRightCoroutine()
    {
        Vector2 positionPolar = ToPolar(transform.localPosition);

        float startingTheta = positionPolar.y;
        float startTime = Time.time;
        float passedTime = Time.time - startTime;
        float initialAngle = transform.eulerAngles.z;
        do
        {
            passedTime = Time.time - startTime;
            currentRotateTime = passedTime;
            float lerpRatio = Mathf.Clamp01(passedTime / rotateTime);
            positionPolar.y = Mathf.Lerp(startingTheta, startingTheta - Mathf.PI / 2, lerpRatio);
            transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(initialAngle, initialAngle - 90, lerpRatio));
            transform.localPosition = ToCartesian(positionPolar);
            yield return null;
        } while (passedTime < rotateTime);
        transform.localPosition = new Vector2(Mathf.Round(transform.localPosition.x) * swordDistance, Mathf.RoundToInt(transform.localPosition.y) * swordDistance);
        transform.eulerAngles = new Vector3(0, 0, Mathf.Round(transform.transform.eulerAngles.z / 10) * 10);
        oldAngles = transform.eulerAngles;
        oldPosition = transform.localPosition;
        activeCoroutine = null;
    }

    void MoveBack()
    {
        transform.localPosition = oldPosition;
        transform.eulerAngles = oldAngles;
        /*
        Vector2 positionPolar = ToPolar(transform.localPosition);
        float startingTheta = positionPolar.y;
        float startTime = Time.time;
        float passedTime = Time.time - startTime;
        float initialAngle = transform.eulerAngles.z;
        do
        {
            float lerpRatio = passedTime / newRotateTime;
            positionPolar.y = Mathf.Lerp(startingTheta, oldAngles.z * Mathf.Deg2Rad, lerpRatio);
            transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(initialAngle, oldAngles.z, lerpRatio));
            transform.localPosition = ToCartesian(positionPolar);
            passedTime = Time.time - startTime;
            yield return null;
        } while (passedTime <= newRotateTime);
        transform.localPosition = new Vector2(Mathf.Round(transform.localPosition.x) * swordDistance, Mathf.RoundToInt(transform.localPosition.y) * swordDistance);
        transform.eulerAngles = new Vector3(0, 0, Mathf.Round(transform.transform.eulerAngles.z));
        oldAngles = transform.eulerAngles;
        oldPosition = transform.localPosition;
        activeCoroutine = null;
        yield return null;
        */
    }

    Vector2 ToPolar(Vector2 positionCartesian)
    {
        float x = positionCartesian.x;
        float y = positionCartesian.y;
        float r = Mathf.Sqrt(((x * x) + (y * y)));
        float theta = Mathf.Atan2(y, x);
        return new Vector2(r, theta);
    }

    Vector2 ToCartesian(Vector2 positionPolar)
    {
        float r = positionPolar.x;
        float theta = positionPolar.y;
        float x = r * Mathf.Cos(theta);
        float y = r * Mathf.Sin(theta);
        return new Vector2(x, y);
    }
}
