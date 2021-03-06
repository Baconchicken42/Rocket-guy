using UnityEngine;
using System.Collections;

public class HeroKnight : MonoBehaviour {

    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;
    [SerializeField] float      m_rollForce = 6.0f;
    [SerializeField] bool       m_noBlood = false;
    [SerializeField] GameObject m_slideDust;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private SpriteRenderer      m_spriteRenderer;
    private Sensor_HeroKnight   m_groundSensor;
    private Sensor_HeroKnight   m_wallSensorR1;
    private Sensor_HeroKnight   m_wallSensorR2;
    private Sensor_HeroKnight   m_wallSensorL1;
    private Sensor_HeroKnight   m_wallSensorL2;
    private Sensor_HeroKnight   m_AttackSensor_R;
    private Sensor_HeroKnight   m_AttackSensor_L;
    private bool                m_grounded = false;
    private bool                m_rolling = false;
    private int                 m_facingDirection = 1;
    private int                 m_currentAttack = 0;
    private float               m_timeSinceAttack = 0.0f;
    private float               m_timeSinceAttackStarted = 0.0f;
    private bool                m_attacking = false;
    private float               m_delayToIdle = 0.0f;
    private bool                m_rocketing = false;


    // Use this for initialization
    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
        m_AttackSensor_L = transform.Find("AttackSensor_L").GetComponent<Sensor_HeroKnight>();
        m_AttackSensor_R = transform.Find("AttackSensor_R").GetComponent<Sensor_HeroKnight>();
    }

    // Update is called once per frame
    void Update()
    {
        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;
        

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            // m_spriteRenderer.flipX = false;
            this.gameObject.transform.localScale = new Vector3(1, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
            m_facingDirection = 1;
        }
            
        else if (inputX < 0)
        {
            // m_spriteRenderer.flipX = true;
            this.gameObject.transform.localScale = new Vector3(-1, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
            m_facingDirection = -1;
        }

        //Jump
        if (Input.GetKey("space") && m_grounded && !m_rolling)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + .1f);
            m_rocketing = true;
            if (Input.GetKey("w"))
            {
                m_animator.SetTrigger("Jump");
                m_grounded = false;
                m_animator.SetBool("Grounded", m_grounded);
                m_body2d.gravityScale = 0;
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
                m_groundSensor.Disable(0.2f);
            }
            if (Mathf.Abs(inputX) > Mathf.Epsilon)
            {
                m_animator.SetTrigger("Jump");
                m_grounded = false;
                m_animator.SetBool("Grounded", m_grounded);
                m_body2d.gravityScale = 0;
                m_body2d.velocity = new Vector2(m_jumpForce * (inputX / Mathf.Abs(inputX)), m_body2d.velocity.y);
                m_groundSensor.Disable(0.2f);
            }
        }
        if (Input.GetKeyUp("space") && m_rocketing)
        {
            m_rocketing = false;
            m_body2d.gravityScale = 1;
        }

        // Move
        if (!m_rolling && !m_rocketing)
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // -- Handle Animations --
        //Wall Slide
        //m_animator.SetBool("WallSlide", (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State()));

        //Death
        // if (Input.GetKeyDown("e") && !m_rolling)
        // {
        //     m_animator.SetBool("noBlood", m_noBlood);
        //     m_animator.SetTrigger("Death");
        // }
            
        //Hurt
        // else if (Input.GetKeyDown("q") && !m_rolling)
        //     m_animator.SetTrigger("Hurt");

        //check if attack is ongoing
        if (m_attacking)
        {
            m_timeSinceAttackStarted += Time.deltaTime;

            //check for a hit, hopefully
            // if (m_facingDirection < 0 && m_AttackSensor_L.getTarget().tag == "enemy")
            // {
            //     Debug.Log("Hit something on left side!");
            //     Destroy(m_AttackSensor_L.getTarget());
            //     Debug.Log("Left side target should be destroyed now");
            // }
            
            if (m_facingDirection > 0 && m_AttackSensor_R.getTarget().tag == "enemy")
            {
                Debug.Log("Hit something on right side!");
                Destroy(m_AttackSensor_R.getTarget());
                Debug.Log("Right side target should be destroyed now");
            }

            if (m_timeSinceAttackStarted > (5f / 60))
            {
                m_attacking = false;
                m_timeSinceAttackStarted = 0f;
            }
        }

        //Attack
        else if(Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling)
        {
            m_attacking = true;
            m_currentAttack++;

            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            m_animator.SetTrigger("Attack" + m_currentAttack);

            

            // Reset timer
            m_timeSinceAttack = 0.0f;
        }

        

        // Block
        // else if (Input.GetMouseButtonDown(1) && !m_rolling)
        // {
        //     m_animator.SetTrigger("Block");
        //     m_animator.SetBool("IdleBlock", true);
        // }

        // else if (Input.GetMouseButtonUp(1))
        //     m_animator.SetBool("IdleBlock", false);

        // // Roll
        // else if (Input.GetKeyDown("left shift") && !m_rolling)
        // {
        //     m_rolling = true;
        //     m_animator.SetTrigger("Roll");
        //     m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
        // }
            

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
                if(m_delayToIdle < 0)
                    m_animator.SetInteger("AnimState", 0);
        }
    }

    // Animation Events
    // Called in end of roll animation.
    void AE_ResetRoll()
    {
        m_rolling = false;
    }

    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }
}
