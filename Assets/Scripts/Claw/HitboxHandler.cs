using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxHandler : MonoBehaviour
{
    private GameObject go_claw;
    private Collider2D stored_colliderItem;
    public bool isInRange = false;
    // Start is called before the first frame update
    void Start()
    {
        go_claw = GameObject.Find("Claw");
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerExit2D(Collider2D c)
    {
        if (c.gameObject.tag == "Obstacle" || c.gameObject.tag == "Player")
        {
            isInRange = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D c)
    {
        Debug.Log(c.name + " thing here");
        if (c.gameObject.tag == "Obstacle" || c.gameObject.tag == "Player") {
            if (go_claw.GetComponent<CClaw>().IsClawOpen()) {
                stored_colliderItem = c;
                   isInRange = true;
            }
        }
    }
    public void SetInRange(bool toWhat)
    {
        isInRange = toWhat;
    }

    public bool IsInRange()
    {
        return isInRange;
    }
    public Collider2D getStoredCollider()
    {
        return stored_colliderItem;
    }

}