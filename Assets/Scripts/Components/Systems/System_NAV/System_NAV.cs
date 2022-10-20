using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rover.Systems;
using Rover.DateTime;
using TMPro;
using UnityTimer;

public class System_NAV : MonoBehaviour
{
    public TextMeshProUGUI roverHeading;
    public TextMeshProUGUI cameraHeading;
    public TextMeshProUGUI roverSpeed;
    public TextMeshProUGUI roverRoll;
    public TextMeshProUGUI roverPitch;
    public TextMeshProUGUI selectedCamera;
    public TextMeshProUGUI dateTime;
    public TextMeshProUGUI gpsCoords;

    void Start()
    {
        Timer.Register(1f, () => UpdateValues(), isLooped: true);
        TimeManager.EOnDateTimeUpdated += OnNewDateTime;
        System_CAM.EOnNewCameraSelected += OnNewCameraSelected;
    }

    void OnDisable()
    {
        TimeManager.EOnDateTimeUpdated -= OnNewDateTime;
        System_CAM.EOnNewCameraSelected -= OnNewCameraSelected;
    }

    void OnNewDateTime(DateTimeStruct newTime)
    {
        dateTime.text = TimeManager.TimeToStringFull(newTime);
    }

    void UpdateValues()
    {
        roverHeading.text = "HDNG: " + System_GPS.Heading.ToString("000");
        cameraHeading.text = "CAM HDNG: " + System_CAM.Heading.ToString("000");
        roverSpeed.text = "SPD: " + System_MTR.RoverVelocity.ToString("0.00") + "m/s";
        roverRoll.text = "RLL: " + System_MTR.RoverRoll.ToString("0.00");
        roverPitch.text = "PCH: " + System_MTR.RoverPitch.ToString("0.00");
        gpsCoords.text = "GPS: " + System_GPS.GPSCoordinates.x.ToString("00.00") + ", " + System_GPS.GPSCoordinates.y.ToString("00.00");
    }

    void OnNewCameraSelected(CameraMode newCamera)
    {
        selectedCamera.text = newCamera.ToString().ToUpper();
    }

}
