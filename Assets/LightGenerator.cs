using UnityEngine;

public class LightGenerator : MonoBehaviour
{
    public int lightCount = 20;
    public float spacing = 2.0f;
    public float intensity = 1.0f;
    public Color lightColor = Color.white;

    [ContextMenu("Generate Lights")]
    public void GenerateLights()
    {
        // Clear existing lights first to avoid duplicates
        while (transform.childCount > 0) {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        for (int i = 0; i < lightCount; i++)
        {
            GameObject lightGameObject = new GameObject("Point Light " + i);
            lightGameObject.transform.parent = this.transform;
            
            // Grid layout: 5 lights per row
            float x = (i % 5) * spacing;
            float z = (i / 5) * spacing;
            lightGameObject.transform.localPosition = new Vector3(x, 0, z);

            Light lightComponent = lightGameObject.AddComponent<Light>();
            lightComponent.type = LightType.Point;
            lightComponent.intensity = intensity;
            lightComponent.color = lightColor;
            lightComponent.range = 10f;
        }
    }
}