using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawVisual : MonoBehaviour
{
    public float shakeIntensity = 5F;
    public float shakePeriod = 4F;
    private float shakeTimer = 0F;
    public CClaw claw;

    // Update is called once per frame
    void Update()
    {
    //si val != 0 entre fonction
    if (claw.storedEnergy != 0)
        {
            DecrEnergy();
        }
    // baisser val chaque iteration
    }
    void DecrEnergy()
    {
        if (claw.storedEnergy > 0) {
        claw.storedEnergy -= 0.1F * Time.deltaTime;
            if (claw.storedEnergy <= 0)
                claw.storedEnergy = 0;
        } else
        {
        claw.storedEnergy += 0.1F* Time.deltaTime;
            if (claw.storedEnergy >= 0)
                claw.storedEnergy = 0;
        }
        shakeTimer += Time.deltaTime * shakePeriod;
        float storedSin = Mathf.Sin(shakeTimer * claw.storedEnergy);
        var rotation = new Quaternion();
        rotation.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, shakeIntensity * claw.storedEnergy);
        transform.rotation = rotation;
    }
}
