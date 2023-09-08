using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CClaw : MonoBehaviour
{
    [SerializeField, Tooltip("Etat d'ouverture de la griffe (-1 = fermé, 0 = neutre et 1 = ouvert)")]
    private int m_opening = 0;
    [SerializeField, Tooltip("Vitesse de mouvement vertical de la griffe")]
    public float m_vSpeed = 5.0f;
    public float m_hSpeedScale = 5.0f;
    public float m_verticalBound = 3;
    public float m_horizontalBound = 10;
    private float offsetHeld = -1;
    public float storedEnergy = 0F;
    public float clawMaxHealth = 30;
    private float clawHealth;

    private GameObject go_grabHitbox;
    private GameObject go_heldItem;
    private GameObject go_impact;

    // Start is called before the first frame update
    void Start()
    {
        go_grabHitbox = GameObject.Find("Hitbox");
        go_impact = GameObject.Find("Virtual Camera");
    }
    // Update is called once per frame
    void Update()
    {
        ChgStateClaw();
        MovementClaw();
        if (go_heldItem != null && go_heldItem.gameObject.layer == LayerMask.NameToLayer("Player"))
            PlayerMash();
    }

    private void PlayerMash()
    {
        if (clawHealth <= 0) {
            go_impact.GetComponent<ImpactScript>().callImpact();
            Release_Item();
            m_opening = 1;
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.R))
        {
            go_impact.GetComponent<ImpactScript>().callShake();
            Debug.Log("MASHIN" + clawHealth);
            clawHealth -= 1;
        }
    }
    public bool IsClawOpen()
    {
        if (m_opening != -1)
            return true;
        return false;
    }
    public void ChgStateClaw()
    {
        if (Input.GetKeyDown(KeyCode.L) && m_opening != 1)
        {
            // if i'm holding && claw open, release
            if (go_heldItem != null)
            {
               go_impact.GetComponent<ImpactScript>().callImpact();
               Release_Item();
            }
            m_opening = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6) && m_opening != -1)
        {
            if (go_grabHitbox.GetComponent<HitboxHandler>().IsInRange() && go_grabHitbox.GetComponent<HitboxHandler>().getStoredCollider() != null)
            {
                go_heldItem = go_grabHitbox.GetComponent<HitboxHandler>().getStoredCollider().gameObject;
                if (go_heldItem.TryGetComponent(out CPlayerHoldReleaseManager cPlayerHRM))
                {
                    go_impact.GetComponent<ImpactScript>().callImpact();
                    go_heldItem = cPlayerHRM.GrabCharacter(transform).gameObject;
                    Catch_Item(false);
                } else {
                    Catch_Item(true);
                }
                clawHealth = clawMaxHealth;
                go_grabHitbox.GetComponent<HitboxHandler>().SetInRange(false);
            }
            m_opening = -1;
        }
        if (Input.GetKeyDown(KeyCode.N) && m_opening != 0)
        {
            if (go_heldItem != null)
            {
                Release_Item();
            }
            m_opening = 0;
        }
    }

    //public void OnUpDown(InputAction.CallbackContext context)
    //{
    //    float value = context.ReadValue<float>();
    //    if (value < 0 && transform.position.y > -m_verticalBound) {
    //        transform.position += new Vector3(0, -m_vSpeed * Time.deltaTime, 0);
    //    }
    //    else if (value > 0 && transform.position.y < m_verticalBound) {
    //        transform.position += new Vector3(0, m_vSpeed * Time.deltaTime, 0);
    //    }
    //}

    public void MovementClaw()
    {
        //if (Input.GetKey(KeyCode.W) && transform.position.x < m_horizontalBound)
        //{
        //    storedEnergy -= 0.2F * Time.deltaTime;
        //    transform.position += new Vector3(m_hSpeed * Time.deltaTime, 0, 0);
        //    if (storedEnergy <= -1F)
        //        storedEnergy = -1F;
        //}   
        //if (Input.GetKey(KeyCode.Q) && transform.position.x > -m_horizontalBound)
        //{
        //    storedEnergy += 0.2F * Time.deltaTime;
        //    transform.position += new Vector3(-m_hSpeed * Time.deltaTime, 0, 0);
        //    if (storedEnergy >= 1F)
        //        storedEnergy = 1F;
        //}
        if (Input.GetKey(KeyCode.J) && transform.position.y < m_verticalBound)
        {
            transform.position += new Vector3(0, m_vSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.V)      && transform.position.y > -m_verticalBound)
        {
            transform.position += new Vector3(0, -m_vSpeed * Time.deltaTime, 0);
        }
    }

    public void OnClawMove(InputAction.CallbackContext context)
    {
        float delta = context.ReadValue<float>();
        Debug.Log(delta);


        storedEnergy += (delta / Mathf.Abs(delta)) * 0.2F * Time.deltaTime;
        storedEnergy = Mathf.Clamp(storedEnergy, -1, 1);
        transform.position += new Vector3(delta * m_hSpeedScale * Time.deltaTime, 0, 0);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -m_horizontalBound, m_horizontalBound), transform.position.y, 0);
    }

    void Catch_Item(bool setParent)
    {
        // snap held item to my pos + offset
        go_heldItem.transform.position = new Vector3(transform.position.x, transform.position.y + offsetHeld, transform.position.z);
        // disable rigidbody component of held item
        go_heldItem.GetComponent<Rigidbody2D>().isKinematic = true;
        // put held item as child
        if (setParent)
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
