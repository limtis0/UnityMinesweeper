using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI setTimerText;
    private static TextMeshProUGUI timerText;
    
    public static bool timerIsRunning = false;

    private static float startTime;
    private float timePassed;
    
    private string hours;
    private string minutes;
    private string seconds;

    void Awake()
    {
        timerText = setTimerText;
    }

    void Update()
    {
        if (!timerIsRunning) { return; }

        timePassed = Time.time - startTime;
        
        if (timePassed >= 3600)
        {
            hours = ((int) timePassed / 3600).ToString("00:");
        }
        else
        {
            hours = "";
        }

        minutes = ((int) timePassed % 3600 / 60).ToString("00:");
        seconds = (timePassed % 60).ToString("00.00");

        timerText.text = "Time: " + hours + minutes + seconds;
    }

    public static void startTimer()
    {
        startTime = Time.time;
        timerIsRunning = true;
    }

    public static void stopTimer()
    {
        timerIsRunning = false;
    }

    public static void resetTime()
    {
        timerText.text = "Time: 00:00.00";
    }
}
