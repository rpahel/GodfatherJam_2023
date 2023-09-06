using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D c)
    {
        Debug.Log(c.name + " thing here");   
        if (c.gameObject.tag == "Obstacle") {
            Debug.Log("Obstacle in hitbox !");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}
