using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CConveyorBelt : MonoBehaviour
{
    CPlayerMovements player;
    Rigidbody2D playerRb;
    float vectRight;
    float vectLeft;
    Vector2 vectInitial;
    bool isEnter = false;
    private float m_speed = 1.5f;
    public bool m_senseOfRotation = true; // true = right
    float playerSpeed;
    float playerSpeedInverse;
    public float DebutRotation;
    public float timeRepeatIteration;
    public float IncreasingSpeed;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("ChangeRotation", DebutRotation, timeRepeatIteration);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        IncreaseSpeed();

        if (!player)
            return;

        playerSpeed = vectInitial.x + m_speed;
        playerSpeedInverse = vectInitial.x - m_speed;
        if (isEnter == true && m_senseOfRotation == true)
        {
            player.Rbobj.velocity = new Vector2(vectRight, player.Rbobj.velocity.y * 1.4f);
        }
        if (isEnter == true && m_senseOfRotation == false)
        {
            player.Rbobj.velocity = new Vector2(vectLeft, player.Rbobj.velocity.y * 1.4f);
        }

    }
    private void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.TryGetComponent(out player))
        {
            
           // Debug.Log($"1 : {player.name}");
            playerRb = col.gameObject.GetComponent<Rigidbody2D>();
            vectInitial = playerRb.velocity;
            vectLeft = player.Rbobj.velocity.x;
            vectRight = player.Rbobj.velocity.x;
            InitialisePlayerVelocity();
            isEnter = true;
        }
        else if(col.transform.parent.TryGetComponent(out player))
        {
            
            //Debug.Log($"2 : {player.name}");
            playerRb = col.gameObject.GetComponent<Rigidbody2D>();
            vectInitial = playerRb.velocity;
            vectLeft = player.Rbobj.velocity.x;
            vectRight = player.Rbobj.velocity.x;
            InitialisePlayerVelocity();
            isEnter = true;
        }
        

    }
    bool InitialisePlayerVelocity()
    {
        vectLeft = playerSpeedInverse;
        vectRight = playerSpeed;
        return true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.gameObject.TryGetComponent(out player))
        {
            player.Rbobj.velocity = vectInitial;
            isEnter = false;
            player = null;
        }
        //else if (collision.transform.parent.TryGetComponent(out player))
        //{
        //    player.Rbobj.velocity = vectInitial;
        //    isEnter = false;
        //    player = null;
        //
        //
        //}
        
    }


    public void ChangeRotation()
    {
        m_senseOfRotation = !m_senseOfRotation;
        //Debug.Log("sens changer");
    }

    void IncreaseSpeed()
    {
        m_speed += Time.deltaTime * IncreasingSpeed / 100;
    }

}
