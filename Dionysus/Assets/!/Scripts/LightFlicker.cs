using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
    [SerializeField] private float smoothTime = 0.125f;
    [SerializeField, Min(0.05f)]  private float flickerIntensity = 2f;
    private new Light light;
    private float initialIntensity;

    private float targetIntensity;
    private float refIntensity;

    private void Awake()
    {
        light = GetComponent<Light>();
        initialIntensity = light.intensity;
    }

    private void Update()
    {
        targetIntensity = Random.Range(initialIntensity / flickerIntensity, initialIntensity * flickerIntensity);
        light.intensity = Mathf.SmoothDamp(light.intensity, targetIntensity, ref refIntensity, smoothTime, Mathf.Infinity, Time.deltaTime);
    }
}