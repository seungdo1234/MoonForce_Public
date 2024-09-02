using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraShake : MonoBehaviour
{

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin noise;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {


    }

    public void ShakeCamera(float shakeDuration, float amplitude , float frequency )
    {
        noise.m_AmplitudeGain = amplitude;
        noise.m_FrequencyGain = frequency;
        // 지정된 시간 후에 노이즈를 비활성화합니다.
        StartCoroutine(StopShake(shakeDuration));
    }

    private IEnumerator StopShake(float duration)
    {
        yield return new WaitForSeconds(duration);
        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;
    }
}
