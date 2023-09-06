using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CClaw : MonoBehaviour
{
    SpriteRenderer m_spriteRenderer; //TRASH
    private int m_opening = 0; // etat de l'ouverture de griffe
    private float m_speed = 5.0f;
    private float m_verticalBound = 3;
    private float m_horizontalBound = 10;

    // Start is called before the first frame update
    void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
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
            m_opening = 1;
            m_spriteRenderer.color = Color.blue;
        }
        if (Input.GetKeyDown(KeyCode.C) && m_opening != -1)
        {
            m_opening = -1;
            m_spriteRenderer.color = Color.red;
        }
        if (Input.GetKeyDown(KeyCode.N) && m_opening != 0)
        {
            m_opening = 0;
            m_spriteRenderer.color = Color.green;
        }
    }

    public void MovementClaw()
    {
        // Todo change to float values
        if (Input.GetKey(KeyCode.RightArrow) && transform.position.x < m_horizontalBound)
        {
            transform.position += new Vector3(m_speed * Time.deltaTime, 0, 0); 
        }   
        if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x > -m_horizontalBound)
        {
            transform.position += new Vector3(-m_speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.UpArrow) && transform.position.y < m_verticalBound)
        {
            transform.position += new Vector3(0, m_speed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow) && transform.position.y > -m_verticalBound)
        {
            transform.position += new Vector3(0, -m_speed * Time.deltaTime, 0);
        }
    }

}
