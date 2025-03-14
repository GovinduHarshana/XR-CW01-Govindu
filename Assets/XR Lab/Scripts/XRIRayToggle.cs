using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace XRLab
{
    /// <summary>
    /// Toggles the ray on and off when a certain input
    /// is detected
    /// </summary>
    [AddComponentMenu("XRLab/Generic VR/XRI Ray Toggle")]
    [RequireComponent(typeof(XRRayInteractor))] //this will add the component to the attached game object if it is not already there
    public class XRIRayToggle : MonoBehaviour
    {
        #region Declarations
        [Tooltip("The input needed to activate this script")]
        [SerializeField] private InputActionReference m_control; //needed to recieve inputs
        [SerializeField] private XRRayInteractor m_ray; //a reference to the ray interactor used on the hand
        private bool m_isEnabled = false; //the boolean used to enable and disable the ray

        [Header("Simulated Settings")]
        [SerializeField] private InputAction m_simControl;
        #endregion

        /// <summary>
        /// Gets the isEnabled field
        /// </summary>
        public bool IsRayEnabled
        {
            get => m_isEnabled;
        }

        private void Awake()
        {
            m_ray = gameObject.GetComponent<XRRayInteractor>(); //getting the ray interactor. note if one was not added then one will be added automatically which could cause bugs if not setup correctly

            //adding the simulated controls into the stock input action
            foreach (InputBinding binding in m_simControl.bindings)
            {
                m_control.action.AddBinding(binding.path);
            }
        }

        /// <summary>
        /// called after each frame to prevent bug that prevented teleportation
        /// </summary>
        private void LateUpdate()
        {
            m_ray.enabled = m_isEnabled;
        }

        #region Enable and Disable
        /// <summary>
        /// binds the callbacks for the InputActionReference passed in and allows their
        /// callbacks to activate the Toggle function
        /// </summary>
        private void OnEnable()
        {
            m_control.action.started += Toggle; //the callback for when the player starts triggering the assigned event
            m_control.action.canceled += Toggle; //the callback for when the player stops triggering the assigned event
        }

        /// <summary>
        /// disables the callbacks when the object/script is disabled
        /// </summary>
        private void OnDisable()
        {
            m_control.action.started -= Toggle; //the callback for when the player starts triggering the assigned event
            m_control.action.canceled -= Toggle; //the callback for when the player stops triggering the assigned event
        }

        /// <summary>
        /// ran from callbacks by input actions, sets the isEnabled bool to 
        /// true or false depending on the input that is passed during the callback 
        /// </summary>
        /// <param name="context"></param>
        private void Toggle(InputAction.CallbackContext context)
        {
            m_isEnabled = context.control.IsPressed();
        }
        #endregion
    } 
}
