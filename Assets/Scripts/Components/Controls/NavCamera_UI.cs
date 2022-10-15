using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rover.DateTime;
using TMPro;
using Rover.Systems;

public class NavCamera_UI : MonoBehaviour
{
    [Header("UI Variables")]
    public TextMeshProUGUI dateTimeText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI rollText;
    public TextMeshProUGUI pitchText;

    void Start()
    {
        TimeManager.EOnDateTimeUpdated += OnDateTimeUpdated;
    }

    void OnDateTimeUpdated(DateTimeStruct newTime)
    {
        dateTimeText.text = TimeManager.TimeToStringFull(newTime);
    }

    void FixedUpdate()
    {
        speedText.text = "SPD: " + System_MTR.RoverVelocity.ToString("00.00") + "m/s";
        rollText.text = "RLL: " + System_MTR.RoverRoll.ToString("00.00");
        pitchText.text = "PTH: " + System_MTR.RoverPitch.ToString("00.00");
    }
}
