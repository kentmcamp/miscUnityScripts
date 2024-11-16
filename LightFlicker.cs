using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public float minIntensity = 0.5f;
    public float maxIntensity = 1;
    public float flickerSpeed = 5f;

    private Light lightObject;
    private float randomOffset;

    void Start()
    {
        lightObject = GetComponent<Light>();
        randomOffset = Random.Range(0f, 100f);
    }

    // Update is called once per frame
    void Update()
    {
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, randomOffset);
        lightObject.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
    }
}
