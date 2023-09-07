using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Cinemachine;

public class ImpactScript : MonoBehaviour
{
    public CinemachineVirtualCamera m_camera;
    private CinemachineBasicMultiChannelPerlin m_multiChannelPerlin;

    private bool isImpactOn = false;
    public float shakeIntensity = 1.0f;
    public float Duration = 0.5f;
    public float remainingTime = -100;

    private void Awake()
    {
        m_camera = GetComponent<CinemachineVirtualCamera>();
    }
    private void Start()
    {
        if (m_camera)
        {
            m_multiChannelPerlin = m_camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            Debug.Log("camera is ok");
            StopShake();
        }
    }
    public void ShakeScreen()
    {
        m_multiChannelPerlin.m_AmplitudeGain = shakeIntensity;
    }

    public void StopShake()
    {
        m_multiChannelPerlin.m_AmplitudeGain = 0.0f;
    }

    public void PauseTime()
    {
        isImpactOn = true;
        remainingTime = Duration;
        Time.timeScale = 0.0F;
    }

    public void callImpact()
    {
        if (isImpactOn == false)
        {
            PauseTime();
            ShakeScreen();
        }
    }
    private void Update()
    {
        if (remainingTime > 0) {
            remainingTime -= Time.unscaledDeltaTime;
        } else if (remainingTime <= 0 && isImpactOn) {
            StopShake();
            //restart time
            remainingTime = 0;
            Time.timeScale = 1.0F;
            isImpactOn = false;
        }
    }
}
