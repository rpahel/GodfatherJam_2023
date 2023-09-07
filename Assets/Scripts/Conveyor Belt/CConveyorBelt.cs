using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CConveyorBelt : MonoBehaviour
{
    CPlayerMovements player;
    Vector2 vectRight;
    Vector2 vectLeft;
    Vector2 vectInitial;
    bool isEnter = false;
    private float m_speed = 1.5f;
    public bool m_senseOfRotation = true; // true = right
    float playerSpeed;
    float playerSpeedInverse;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("ChangeRotation", 10.0f, 5.0f);
        vectInitial = player.Rbobj.velocity;
        vectLeft = player.Rbobj.velocity;
        vectRight = player.Rbobj.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        IncreaseSpeed();
        playerSpeed = vectInitial.x + m_speed;
        playerSpeedInverse = vectInitial.x - m_speed;

        if (isEnter == true && m_senseOfRotation == true)
        {
            player.Rbobj.velocity = vectRight;
        }
        if (isEnter == true && m_senseOfRotation == false)
        {
            player.Rbobj.velocity = vectLeft;
        }
        Debug.Log(m_speed);

    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.TryGetComponent(out player))
        {
            InitialisePlayerVelocity();
            isEnter = true;
        }
    }
    private void OnCollisionStay2D(Collision2D stay)
    {
        if (stay.gameObject.TryGetComponent(out player))
        {
            InitialisePlayerVelocity();
        }
    }
    bool InitialisePlayerVelocity()
    {
        vectLeft = new Vector2(playerSpeedInverse, 0);
        vectRight = new Vector2(playerSpeed, 0);
        return true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out player))
        {
            player.Rbobj.velocity = vectInitial;
            isEnter = false;
        }
    }


    public void ChangeRotation()
    {
        m_senseOfRotation = !m_senseOfRotation;
        Debug.Log("sens changer");
    }

    void IncreaseSpeed()
    {
        m_speed = m_speed + Time.deltaTime/8;
    }

}
