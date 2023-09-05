using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTapis : MonoBehaviour
{
    private float m_speed = 10.0f;
    private bool m_senseOfRotation = true;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ImproveSpeed()
    {
        m_speed += Time.deltaTime;
    }
    public void ChangeRotation()
    {
        m_senseOfRotation = !m_senseOfRotation;

    }

}
