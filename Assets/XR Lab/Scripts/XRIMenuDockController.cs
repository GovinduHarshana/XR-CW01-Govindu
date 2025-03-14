using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("XRLab/XR Editor/Menu Dock Controller")]
public class XRIMenuDockController : MonoBehaviour
{
    [Tooltip("The button to toggle object attach")]
    [SerializeField] private Button m_toggleDockedButton; 
    [Tooltip("The transform the attached object to snap to")]
    [SerializeField] private Transform m_dockPosition;

    //used to activate and deactivate dock mode on the attached object
    [SerializeField] private bool m_isDocked = false;
    
    public bool IsDocked 
    {
        get => m_isDocked;
        set => IsDocked = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (m_dockPosition == null)
            Debug.LogWarning("m_dockPosition is missing a reference. Script will not run correctly until it's assigned");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_dockPosition == null) //if there is a missing reference for the dock transform then stop running to prevent errors
            return;

        if (m_isDocked) //if the object is docked move the UI to the position of the dockPosition 
        {
            gameObject.transform.root.position = m_dockPosition.position;
            gameObject.transform.root.rotation = m_dockPosition.rotation;
        }
    }

    /// <summary>
    /// Toggles the isDocked property
    /// </summary>
    private void ToggleIsDocked()
    {
        m_isDocked = !m_isDocked;
    }

    private void OnEnable()
    {
        m_toggleDockedButton.onClick.AddListener(() => ToggleIsDocked());
    }

    private void OnDisable()
    {
        m_toggleDockedButton.onClick.RemoveAllListeners();
    }
}
