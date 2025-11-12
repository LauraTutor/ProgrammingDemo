using UnityEngine;

public class FlickeringLightLS : MonoBehaviour
{
    public Light lightSource;
    public float minIntensity = 0.5f;  // Minimum light intensity
    public float maxIntensity = 2f;  // Maximum light intensity
    public float flickerSpeed = 0.1f;  // Speed of flickering

    private float targetIntensity;
    private float smoothFactor = 0.1f;  // Smoothing factor for flicker effect

    void Start()
    {
        if (lightSource == null)
        {
            lightSource = GetComponent<Light>();
        }
    }

    void Update()
    {
        FlickerLight();
    }

    void FlickerLight()
    {
        if (Random.value > 0.9)  // Randomly decide when to change the intensity
        {
            targetIntensity = Random.Range(minIntensity, maxIntensity);
        }

        // Smoothly transition the light intensity to the target value
        lightSource.intensity = Mathf.Lerp(lightSource.intensity, targetIntensity, smoothFactor * flickerSpeed);
    }
}
