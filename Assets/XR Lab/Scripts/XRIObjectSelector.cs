using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

namespace XRLab
{
    /// <summary>
    /// uses the raycast from the XRRayInteractor and 
    /// sends a reference for any objects the ray hits 
    /// to the component manager 
    /// </summary>

    [AddComponentMenu("XRLab/XR Editor/Object Selector")]
    [RequireComponent(typeof(XRRayInteractor))]
    public class XRIObjectSelector : MonoBehaviour
    {
        [Tooltip("The control that the script should listen for")]
        [SerializeField] private InputActionReference m_selectObjectControl;

        [Tooltip("The ray interactor attached to the same gameobject as this script, if left unnassigned will attempt to locate the component automatically")]
        [SerializeField] private XRRayInteractor m_ray;

        //serialized only for debugging
        [Tooltip("The script attached to the UI menu, if left unnassigned will attempt to locate the script automatically")]
        [SerializeField] private XRIObjectComponentManager[] m_componentManagerUI;

        /// <summary>
        /// instantiate the ray and component manager
        /// variables if they were not explicitly 
        /// assigned through the inspector
        /// </summary>
        void Awake()
        {
            if (m_ray == null)
                m_ray = gameObject.GetComponent<XRRayInteractor>();

            //if (m_componentManagerUI == null)
#if UNITY_2023_1_OR_NEWER
            m_componentManagerUI = FindObjectsByType<XRIObjectComponentManager>(FindObjectsSortMode.None);
#else
            m_componentManagerUI = FindObjectsOfType<XRIObjectComponentManager>();
#endif
        }

        public void SetControl(InputActionReference value)
        {
            m_selectObjectControl = value; //assigning the value

            OnEnable(); //running onEnable to activate the callbacks
        }

        /// <summary>
        /// gets the raycast hit from the XRI ray interactor
        /// and sends a reference to what the ray hit over to 
        /// the UI
        /// </summary>
        /// <param name="context"> The callback used to run this function </param>
        private void SelectObject(InputAction.CallbackContext context)
        {
            RaycastHit rayHit; //the container for the raycast information

            foreach (XRIObjectComponentManager cMan in m_componentManagerUI)
            {
                if (m_ray.TryGetCurrent3DRaycastHit(out rayHit)) //assigning the rayhit variable and if it is not null, run this line
                    cMan.SetSelectedGameObject(rayHit.transform.gameObject); //sending the reference to the UI to do extra changes
            }

        }

        /// <summary>
        /// assign the callbacks to and required
        /// functions once this script is enabled
        /// </summary>
        private void OnEnable()
        {
            if (m_selectObjectControl == null) //check for null references
                return;

            //binding the control for selecting an object
            m_selectObjectControl.action.started += SelectObject;
            m_selectObjectControl.action.canceled += SelectObject;
        }

        /// <summary>
        /// disable and callbacks to this script
        /// once it has been disabled 
        /// </summary>
        private void OnDisable()
        {
            if (m_selectObjectControl == null) //check for null references
                return;

            //unbinding the control for selecting an object
            m_selectObjectControl.action.started -= SelectObject;
            m_selectObjectControl.action.canceled -= SelectObject;
        }
    }

}