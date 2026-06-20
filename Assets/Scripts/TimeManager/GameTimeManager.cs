using UnityEngine;
using System;

public class GameTimeManager : MonoBehaviour
{
    public static GameTimeManager Instance;

    [Header("Current Time")]
    [SerializeField] private int currentDay = 1;
    [Range(0f, 24f)]
    [SerializeField] private float currentHour = 0f;

    [Header("Time Settings")]
    [Tooltip("How many REALTIME seconds for a full in-game day (24 hours).")]
    [SerializeField] private float realSecondsPerDay = 60f;

    public int CurrentDay => currentDay;
    public float CurrentHour => currentHour;
    public bool IsDayTime => currentHour < 12f;
    public bool IsNightTime => !IsDayTime;

    public event Action<int> OnDayChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        AdvanceTime();
    }

    private void AdvanceTime()
    {
        float hoursPerSecond = 24f / realSecondsPerDay;
        currentHour += hoursPerSecond * Time.deltaTime;

        if (currentHour >= 24f)
        {
            currentHour -= 24f;
            currentDay++;
            OnDayChanged?.Invoke(currentDay);
        }
    }

    // Military time, e.g. "00:00", "13:45", "23:59"
    public string GetFormattedTime()
    {
        int hours24 = Mathf.FloorToInt(currentHour);
        int minutes = Mathf.FloorToInt((currentHour - hours24) * 60f);
        return $"{hours24:00}:{minutes:00}";
    }
}