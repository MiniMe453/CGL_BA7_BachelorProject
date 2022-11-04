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
    public static event System.Action EOnObjectEnterRange;
    public static event System.Action EOnObjectLeaveRange;
    private Collider[] m_hitColliders = new Collider[] {};
    private bool m_OnLeaveEventFired = false;
    void Awake()
    {
        m_LidarButton = new ArduinoInput(InputType.Digital, 43, 9, "LIDAR Button");
        m_LidarButton.EOnButtonPressed += OnButtonPressed;

        Timer.Register(0.25f, () => CheckLidarArea(), isLooped: true);
    }

    void OnButtonPressed(int pin)
    {
        if(System_MTR.RoverVelocity > 0.1)
        {
            UIManager.ShowMessageBox("STOP THE ROVER", Color.red, 2f);  
            return;
        }
        else if (m_hitColliders.Length == 0)
        {
            UIManager.ShowMessageBox("NO OBJECTS IN RANGE", Color.red, 2f);
        }
        else
        {
            OperatingSystem.SetUserControl(false);
            UIManager.ShowMessageBox("SCANNING...", Color.green, 2f);
            Timer.Register(2f, () => SpawnLidarPoints());
        }
    }

    void SpawnLidarPoints()
    {
        OperatingSystem.SetUserControl(true);

        foreach(Collider collider in m_hitColliders)
        {
            Debug.Log(collider.gameObject.name);
            collider.gameObject.layer = LayerMask.NameToLayer("NavCameraViewMode");
            
            for(int i = 0; i< collider.gameObject.transform.childCount; i++)
            {
                collider.gameObject.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("NavCameraViewMode");
            }
        }
    }

    void CheckLidarArea()
    {
        m_hitColliders = Physics.OverlapSphere(transform.position, GameSettings.LIDAR_SCAN_RANGE, LayerMask.NameToLayer("NavCameraDetectionLayer"));

        if(m_hitColliders.Length == 0)
        {
            if(!m_OnLeaveEventFired)
            {
                EOnObjectLeaveRange?.Invoke();
                m_OnLeaveEventFired = true;
            }
            return;
        }
            
        m_OnLeaveEventFired = false;
        EOnObjectEnterRange?.Invoke();
    }
}
