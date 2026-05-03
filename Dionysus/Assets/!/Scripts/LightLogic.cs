using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Light))]
public class LightLogic : MonoBehaviour
{
    [SerializeField] private float smoothTime = 0.125f;
    [SerializeField, Min(0.05f)]  private float flickerIntensity = 2f;
    [SerializeField] private GameObject flames;
    public Light light;
    private float initialIntensity;

    private float targetIntensity;
    private float refIntensity;
    

    private void Awake()
    {
        initialIntensity = light.intensity;
    }

    private void Update()
    {
        targetIntensity = Random.Range(initialIntensity / flickerIntensity, initialIntensity * flickerIntensity);
        light.intensity = Mathf.SmoothDamp(light.intensity, targetIntensity, ref refIntensity, smoothTime, Mathf.Infinity, Time.deltaTime);
    }
    
    public void TurnOn()
    {
        light.enabled = true;
        flames.SetActive(true);
    }
    
    public void TurnOff()
    {
        
       light.enabled = false;
        flames.SetActive(false);
    }
}