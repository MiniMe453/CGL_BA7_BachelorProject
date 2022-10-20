using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rover.Arduino;
using Rover.OS;
using UnityTimer;
using Rover.Systems;
using Rover.Settings;

public class System_LIDAR : MonoBehaviour
{
    private ArduinoInput m_LidarButton;
    public int numOfLidarPoints;
    public Transform parentGameObject;
    public GameObject lidarPointPrefab;
    void Awake()
    {
        m_LidarButton = new ArduinoInput(InputType.Digital, 43, 9, "LIDAR Button");
        m_LidarButton.EOnButtonPressed += OnButtonPressed;
    }

    void OnButtonPressed(int pin)
    {
        if(System_MTR.RoverVelocity > 0.1)
        {
            UIManager.ShowMessageBox("STOP THE ROVER", Color.red, 2f);  
        }
        else
        {
            OperatingSystem.SetUserControl(false);
            UIManager.ShowMessageBox("TRANSMITTING...", Color.green, 2f);
            Timer.Register(2f, () => SpawnLidarPoints());
        }
    }

    void SpawnLidarPoints()
    {
        OperatingSystem.SetUserControl(true);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, GameSettings.LIDAR_SCAN_RANGE);

        foreach(Collider collider in hitColliders)
        {
            Debug.Log(collider.gameObject.name);
            collider.gameObject.layer = LayerMask.NameToLayer("NavCameraViewMode");
            
            for(int i = 0; i< collider.gameObject.transform.childCount; i++)
            {
                collider.gameObject.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("NavCameraViewMode");
            }
        }
    }
}
