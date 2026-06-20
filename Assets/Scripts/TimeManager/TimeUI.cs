using TMPro;
using UnityEngine;

public class TimeUI : MonoBehaviour
{
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text periodText;

    private void Update()
    {
        if (GameTimeManager.Instance == null) return;

        dayText.text = $"Day {GameTimeManager.Instance.CurrentDay}";
        timeText.text = GameTimeManager.Instance.GetFormattedTime();

        float hour = GameTimeManager.Instance.CurrentHour;

        if (hour >= 6f && hour < 12f) periodText.text = "SUNRISE";
        else if (hour >= 12f && hour < 18f) periodText.text = "NOON";
        else if (hour >= 18f && hour < 20f) periodText.text = "SUNSET";
        else periodText.text = "MIDNIGHT";
    }
}