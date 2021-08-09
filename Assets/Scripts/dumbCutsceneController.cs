using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public Camera cam0;
    public Camera cam1;
    public Camera main;

    public GameObject hero;
    public GameObject textDisplay1;
    public GameObject textDisplay2;

    private Animator m_animator;

    

    // Start is called before the first frame update
    void Start()
    {
        m_animator = hero.GetComponent<Animator>();
        textDisplay1.SetActive(false);
        textDisplay2.SetActive(false);
        main.enabled = false;
        cam1.enabled = false;

        StartCoroutine(animCoRoutine());
    }

    IEnumerator animCoRoutine()
    {
        Debug.Log("coroutine started at " + Time.time);
        yield return new WaitForSeconds(1);
        textDisplay1.SetActive(true);
        yield return new WaitForSeconds(2);

        m_animator.SetTrigger("SpazOut");
        yield return new WaitForSeconds(3);

        cam1.enabled = true;
        cam0.enabled = false;
        yield return new WaitForSeconds(.5f);

        textDisplay2.SetActive(true);
        yield return new WaitForSeconds(4);

        cam1.enabled = false;
        main.enabled = true;
        textDisplay2.SetActive(false);
        textDisplay1.SetActive(false);
        gameObject.SetActive(false);
        StopCoroutine(animCoRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
