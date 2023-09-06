using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CClaw : MonoBehaviour
{
    SpriteRenderer m_spriteRenderer; //TRASH
    private int m_opening = 0; // etat de l'ouverture de griffe
    private float m_vSpeed = 5.0f;
    private float m_hSpeed = 5.0f;
    private float m_verticalBound = 3;
    private float m_horizontalBound = 10;

    private GameObject go_grabHitbox;
    private BoxCollider2D bc_grabHitbox;
    // Start is called before the first frame update
    void Start()
    {
        go_grabHitbox = GameObject.Find("Hitbox");
        bc_grabHitbox = go_grabHitbox.GetComponent<BoxCollider2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>(); // TRASH
    }
    // Update is called once per frame
    void Update()
    {
        ChgStateClaw();
        MovementClaw();

    }  
    public void ChgStateClaw()
    {
        if (Input.GetKeyDown(KeyCode.O) && m_opening != 1)
        {
            bc_grabHitbox.enabled = false;
            m_opening = 1;
            m_spriteRenderer.color = Color.blue; //T
        }
        if (Input.GetKeyDown(KeyCode.C) && m_opening != -1)
        {
            bc_grabHitbox.enabled = true;
            
            m_opening = -1;
            m_spriteRenderer.color = Color.red;//T  
        }
        if (Input.GetKeyDown(KeyCode.N) && m_opening != 0)
        {
            bc_grabHitbox.enabled = false;
            m_opening = 0;
            m_spriteRenderer.color = Color.green;//T
        }
    }

    public void MovementClaw()
    {
        // Todo change to float values
        if (Input.GetKey(KeyCode.RightArrow) && transform.position.x < m_horizontalBound)
        {
            transform.position += new Vector3(m_hSpeed * Time.deltaTime, 0, 0); 
        }   
        if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x > -m_horizontalBound)
        {
            transform.position += new Vector3(-m_hSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.UpArrow) && transform.position.y < m_verticalBound)
        {
            transform.position += new Vector3(0, m_vSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow) && transform.position.y > -m_verticalBound)
        {
            transform.position += new Vector3(0, -m_vSpeed * Time.deltaTime, 0);
        }
    }

}
