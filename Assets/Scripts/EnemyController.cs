using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCol;
    private Animator m_animator;
    private SpriteRenderer spriteRend;
    private float timeSinceFlip;

    public float walkTime = 5;
    public int walkSpeed = 5;
    public int walkDirection = -1;

    public GameObject respawn;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
        m_animator = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();

        rb.velocity = new Vector2(walkSpeed * walkDirection, rb.velocity.y);

        m_animator.SetInteger("Animate", 2);
    }

    void move()
    {
        if (timeSinceFlip > walkTime)
        {
            walkDirection *= -1;
            timeSinceFlip = 0f;
        }

        if (walkDirection < 0)
            spriteRend.flipX = true;
        else
            spriteRend.flipX = false;
        
        rb.velocity = new Vector2(walkSpeed * walkDirection, rb.velocity.y);
    }

    void hurtPlayer()
    {
        player.transform.position = new Vector2(respawn.transform.position.x, respawn.transform.position.y);
        Debug.Log("KILLED!!!");
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject != null)
        {
            timeSinceFlip += Time.deltaTime;

            move();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision!");
        if (collision.gameObject == player)
        {
            hurtPlayer();
        }
    }
}
