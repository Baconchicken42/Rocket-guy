using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCol;
    private float timeSinceFlip;

    public float walkTime = 5;
    public int walkSpeed = 5;
    public int walkDirection = -1;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();

        rb.velocity = new Vector2(walkSpeed * walkDirection, rb.velocity.y);
    }

    void move()
    {
        if (timeSinceFlip > walkTime)
        {
            walkDirection *= -1;
            timeSinceFlip = 0f;
        }
        rb.velocity = new Vector2(walkSpeed * walkDirection, rb.velocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceFlip += Time.deltaTime;

        move();
    }
}
