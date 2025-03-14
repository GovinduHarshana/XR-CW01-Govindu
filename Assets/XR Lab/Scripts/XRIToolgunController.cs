using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.UI;
/// <summary>
/// changes the current toolgun mode to allow
/// new functionality by enabling and disabling 
/// certain actions on the controller depending 
/// on the mode 
/// </summary>
[AddComponentMenu("XRLab/Generic VR/XR Toolgun Controller")]
public class XRIToolgunController : MonoBehaviour
{
    public enum ToolgunMode
    {
        inactive,
        transform,
        duplicator,
        custom,
    }

    [Tooltip("The XR controller script for the hand the script should target")]
    [SerializeField] private ActionBasedController m_xrController;
    [Tooltip("The input action to toggle the custom tool on and off")]
    [SerializeField] private InputActionReference m_resetToolgunControl;
    [Tooltip("An array of scripts that contain additional functionality for the 'custom' tool mode")]
    [SerializeField] MonoBehaviour[] m_customToolScripts;

    [Tooltip("The label that states the mode of the toolgun")]
    [SerializeField] private Text m_toolmodeLabel;

    //public MonoBehaviour m_currentCustomTool;

    //determines which tool will be activated when the player activates a custom tool
    public MonoBehaviour CurrentCustomTool { get; set; }

    // the internal variable for keeping track of the hands current state
    private ToolgunMode m_toolMode;

    private void Awake()
    {
        InputAction.CallbackContext context = new InputAction.CallbackContext();
        
        m_toolMode = ToolgunMode.custom; //setting this so toolmode toggles to idle
        ToggleCustomTool(context);
    }

    private void ToggleCustomTool(InputAction.CallbackContext context)
    {
        if (m_toolMode != ToolgunMode.custom)
        {
            m_toolMode = ToolgunMode.custom;

            if (m_xrController != null)
            {
                m_xrController.enableInputActions = false;
            }

            //disabling all tools
            foreach (MonoBehaviour script in m_customToolScripts)
            {
                script.enabled = false;
            }

            //enabling the selected tool
            CurrentCustomTool.enabled = true;

            if (m_toolmodeLabel != null)
                m_toolmodeLabel.text = "Custom - " + CurrentCustomTool.GetType().Name;
        }
        else
        {
            m_toolMode = ToolgunMode.inactive;

            if (m_xrController != null)
            {
                m_xrController.enableInputActions = true;
            }

            foreach (MonoBehaviour script in m_customToolScripts)
            {
                script.enabled = false;
            }

            if (m_toolmodeLabel)
                m_toolmodeLabel.text = "Idle";
        }
    }

    private void OnEnable()
    {
        m_resetToolgunControl.action.started += ToggleCustomTool;
    }

    private void OnDisable()
    {
        m_resetToolgunControl.action.started -= ToggleCustomTool;
    }
}
