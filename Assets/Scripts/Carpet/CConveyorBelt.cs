using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCarpet : MonoBehaviour
{
    Coroutine verifieur;
    float seconds = 20f;
    private float m_speed = 10.0f;
    private bool m_senseOfRotation = true; // true = right
    // Start is called before the first frame update
    void Start()
    {
        verifieur = StartCoroutine(RotationCooldown());
    }

    // Update is called once per frame
    void Update()
    {
        if (verifieur == null)
        {
            verifieur = StartCoroutine(RotationCooldown());
        }

    }

    public void ImproveSpeed()
    {
        m_speed += Time.deltaTime;
    }
    public void ChangeRotation()
    {
        m_senseOfRotation = !m_senseOfRotation;
    }
    private IEnumerator RotationCooldown()
    {

        yield return new WaitForSecondsRealtime(seconds);
        ChangeRotation();
        seconds = accelerateRotationChanging(seconds);
        verifieur = null;
    }
    public float accelerateRotationChanging(float repeatTime)
    {
        return repeatTime / 1.4f;
    }
}
