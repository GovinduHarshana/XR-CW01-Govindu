using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Reads in controller inputs to try and detect the 
/// active XR device
/// </summary>
public class VRDeviceDetector : MonoBehaviour
{
    private InputAction m_indexAction, m_oculusAction, m_viveAction;

    public bool IndexDetected { get; private set; }
    public bool ViveWandDetected { get; private set; }
    public bool OculusDetected { get; private set; }

    /// <summary>
    /// Assigning the input actions in the start. Could use input
    /// action asset but will use hard coded version to make 
    /// portability of script easier. 
    /// 
    /// Trigger is currently the target action since all standard
    /// XR controllers have a trigger
    /// </summary>
    void Start()
    {
        m_indexAction = new InputAction(type:InputActionType.Button, binding: "<ValveIndexController>/triggerPressed", interactions:"press(behavior=1)");
        m_oculusAction = new InputAction(type:InputActionType.Button, binding: "<OculusTouchController>/triggerPressed", interactions: "press(behavior=1)");
        m_viveAction = new InputAction(type: InputActionType.Button, binding: "<ViveController>/triggerPressed", interactions: "press(behavior=1)");
        
        m_indexAction.Enable();
        m_oculusAction.Enable();
        m_viveAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_indexAction.triggered)
        {
            IndexDetected = true;
            Debug.Log("Index detected");
        }

        if (m_viveAction.triggered)
        {
            ViveWandDetected = true;
            Debug.Log("vive detected");
        }

        if (m_oculusAction.triggered)
        {
            OculusDetected = true;
            Debug.Log("oculus detected");
        }
    }

    private void OnEnable()
    {

    }
}
