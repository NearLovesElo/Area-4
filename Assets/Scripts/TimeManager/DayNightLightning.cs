using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightLighting : MonoBehaviour
{
    [SerializeField] private Light2D globalLight;

    [Header("Lighting")]
    [SerializeField] private Gradient lightColorGradient;

    // Reset() runs once, only when the component is first added (or via the
    // right-click "Reset" in the Inspector) — NOT every time you hit Play.
    // This seeds a starting gradient so the field isn't blank, but won't
    // ever overwrite tweaks you make afterward in the Gradient Editor.
    private void Reset()
    {
        lightColorGradient = BuildDayNightGradient();
    }

    private void Update()
    {
        if (GameTimeManager.Instance == null) return;

        float normalizedTime = GameTimeManager.Instance.CurrentHour / 24f;
        globalLight.color = lightColorGradient.Evaluate(normalizedTime);
    }

    private Gradient BuildDayNightGradient()
    {
        Gradient g = new Gradient();

        GradientColorKey[] colorKeys = new GradientColorKey[]
        {
            new GradientColorKey(new Color(0.05f, 0.07f, 0.20f), 0f / 24f),   // 00:00 night
            new GradientColorKey(new Color(1.00f, 0.55f, 0.30f), 6f / 24f),   // 06:00 sunrise
            new GradientColorKey(new Color(1.00f, 0.95f, 0.85f), 9f / 24f),   // 09:00 morning
            new GradientColorKey(new Color(1.00f, 1.00f, 0.98f), 13f / 24f),  // 13:00 noon
            new GradientColorKey(new Color(1.00f, 0.85f, 0.65f), 17f / 24f),  // 17:00 afternoon
            new GradientColorKey(new Color(1.00f, 0.45f, 0.25f), 19f / 24f),  // 19:00 sunset
            new GradientColorKey(new Color(0.10f, 0.10f, 0.25f), 21f / 24f),  // 21:00 dusk
            new GradientColorKey(new Color(0.05f, 0.07f, 0.20f), 24f / 24f),  // 24:00 night
        };

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[]
        {
            new GradientAlphaKey(1f, 0f),
            new GradientAlphaKey(1f, 1f)
        };

        g.SetKeys(colorKeys, alphaKeys);
        return g;
    }
}