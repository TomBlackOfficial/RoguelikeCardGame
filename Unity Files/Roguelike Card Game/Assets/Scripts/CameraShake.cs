using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    private CinemachineVirtualCamera CinemachineVirtualCamera;

    private float timer;
    private CinemachineBasicMultiChannelPerlin _cbmcp;

    void Awake()
    {
        instance = this;

        CinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        _cbmcp = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Start()
    {
        StopShake();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                StopShake();
            }
        }
    }

    public void ShakeCamera(float shakeIntensity, float shakeFrequency, float shakeTime)
    {
        _cbmcp.m_AmplitudeGain = shakeIntensity;
        _cbmcp.m_FrequencyGain = shakeFrequency;
        timer = shakeTime;
    }

    private void StopShake()
    {
        CinemachineBasicMultiChannelPerlin _cbmcp = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cbmcp.m_AmplitudeGain = 0;
    }
}
