using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider healthBar;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI killCountText;

    private int killCount = 0;

    public static UIManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public void UpdateHealth(int current, int max)
    {
        healthBar.maxValue = max;
        healthBar.value = current;
    }

    public void UpdateTimer(float timeRemaining)
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void AddKill()
    {
        killCount++;
        killCountText.text = "Kills: " + killCount;
    }
}