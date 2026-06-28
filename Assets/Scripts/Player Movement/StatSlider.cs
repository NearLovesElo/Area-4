using UnityEngine;
using UnityEngine.UI;

public class StatSlider : MonoBehaviour
{
    private enum StatType { Health, Hunger, Thirst }

    [SerializeField] private Slider slider;
    [SerializeField] private StatType statType;

    private void Awake()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }
    }

    private void OnEnable()
    {
        // PlayerStats.Instance might not exist yet if this runs before
        // PlayerStats.Awake() - Start() is guaranteed to run after every
        // object's Awake(), so subscribe there instead of OnEnable.
        StartCoroutine(SubscribeWhenReady());
    }

    private System.Collections.IEnumerator SubscribeWhenReady()
    {
        while (PlayerStats.Instance == null)
        {
            yield return null;
        }

        PlayerStats.Instance.OnStatsChanged += Refresh;
        Refresh();
    }

    private void OnDisable()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.OnStatsChanged -= Refresh;
        }
    }

    private void Refresh()
    {
        float value = statType switch
        {
            StatType.Health => PlayerStats.Instance.Health,
            StatType.Hunger => PlayerStats.Instance.Hunger,
            StatType.Thirst => PlayerStats.Instance.Thirst,
            _ => 0f
        };

        slider.value = Mathf.Clamp(value, slider.minValue, slider.maxValue);
    }
}