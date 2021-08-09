using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DiamondController : MonoBehaviour
{
    public GameObject player;

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision!");
        if (collision.gameObject == player)
        {
            Debug.Log("Victory!");
            SceneManager.LoadScene("End_Screen");
        }
    }
}
