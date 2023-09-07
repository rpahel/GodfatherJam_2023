using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CClaw : MonoBehaviour
{
    private int m_opening = 0; // etat de l'ouverture de griffe
    public float m_vSpeed = 5.0f;
    public float m_hSpeed = 5.0f;
    public float m_verticalBound = 3;
    public float m_horizontalBound = 10;
    private float offsetHeld = -1;
    public float storedEnergy = 0F;

    private GameObject go_grabHitbox;
    private GameObject go_heldItem;

    // Start is called before the first frame update
    void Start()
    {
        go_grabHitbox = GameObject.Find("Hitbox");
    }
    // Update is called once per frame
    void Update()
    {
        ChgStateClaw();
        MovementClaw();

    }

    public bool IsClawOpen()
    {
        if (m_opening != -1)
            return true;
        return false;
    }
    public void ChgStateClaw()
    {
        if (Input.GetKeyDown(KeyCode.R) && m_opening != 1)
        {
            // if i'm holding && claw open, release
            if (go_heldItem != null)
            {
               Release_Item();
            }
            m_opening = 1;
        }
        if (Input.GetKeyDown(KeyCode.V) && m_opening != -1)
        {
            if (go_grabHitbox.GetComponent<HitboxHandler>().IsInRange() && go_grabHitbox.GetComponent<HitboxHandler>().getStoredCollider() != null)
            {
                go_heldItem = go_grabHitbox.GetComponent<HitboxHandler>().getStoredCollider().gameObject;
                Catch_Item();
                go_grabHitbox.GetComponent<HitboxHandler>().SetInRange(false);
            }
            m_opening = -1;
        }
        if (Input.GetKeyDown(KeyCode.F) && m_opening != 0)
        {
            if (go_heldItem != null)
            {
                Release_Item();
            }
            m_opening = 0;
        }
    }

    public void MovementClaw()
    {
        if (Input.GetKey(KeyCode.S) && transform.position.x < m_horizontalBound)
        {
            storedEnergy += 0.2F * Time.deltaTime;
            transform.position += new Vector3(m_hSpeed * Time.deltaTime, 0, 0);
            if (storedEnergy >= 1F)
                storedEnergy = 1F;
        }   
        if (Input.GetKey(KeyCode.A) && transform.position.x > -m_horizontalBound)
        {
            storedEnergy -= 0.2F * Time.deltaTime;
            transform.position += new Vector3(-m_hSpeed * Time.deltaTime, 0, 0);
            if (storedEnergy <= -1F)
                storedEnergy = -1F;
        }
        if (Input.GetKey(KeyCode.E) && transform.position.y < m_verticalBound)
        {
            transform.position += new Vector3(0, m_vSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.D) && transform.position.y > -m_verticalBound)
        {
            transform.position += new Vector3(0, -m_vSpeed * Time.deltaTime, 0);
        }
    }

    void Catch_Item()
    {
        // snap held item to my pos + offset
        go_heldItem.transform.position = new Vector3(transform.position.x, transform.position.y + offsetHeld, transform.position.z);
        // disable rigidbody component of held item
        go_heldItem.GetComponent<Rigidbody2D>().isKinematic = true;
        // put held item as child
        go_heldItem.transform.SetParent(transform, true);
    }

    void Release_Item()
    {
        if (go_heldItem.TryGetComponent(out CPlayerHoldReleaseManager playerManager)) {
            Debug.Log("entering release character");
            playerManager.ReleaseCharacter();
        } else {
            // remove item as child
            go_heldItem.transform.parent = null;
        }
        // activate rigidbody
        go_heldItem.GetComponent<Rigidbody2D>().isKinematic = false;
        go_heldItem = null;
    }
}
