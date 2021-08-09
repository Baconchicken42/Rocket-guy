using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesController : MonoBehaviour
{
    public GameObject player;
    public GameObject respawn;

    private BoxCollider2D boxCol;

    // Start is called before the first frame update
    void Start()
    {
        boxCol = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision!");
        if (collision.gameObject == player)
        {
            hurtPlayer();
        }
    }

    void hurtPlayer()
    {
        player.transform.position = new Vector2(respawn.transform.position.x, respawn.transform.position.y);
        Debug.Log("KILLED!!!");
    }
}
