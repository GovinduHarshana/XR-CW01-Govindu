using UnityEngine;
using UnityEngine.InputSystem;

namespace XRLab
{
    /// <summary>
    /// the base class for custom tool scripts
    /// that operate on the input device 
    /// constantly (not bound to grab object). 
    /// can only be instanced from child classes
    /// but has all callback binding prepared
    /// so new scripts only need to override 
    /// the functions.
    /// </summary>
    public abstract class BaseCustomTool : MonoBehaviour
    {
        [Header("Control Bindings")]
        [Tooltip("The input action for when the primary button is pressed")]
        [SerializeField] protected InputActionReference m_primaryButtonPressed;
        [Tooltip("The input action for when the secondary button is pressed")]
        [SerializeField] protected InputActionReference m_secondaryButtonPressed;
        [Tooltip("The input action for when the trigger button is pressed")]
        [SerializeField] protected InputActionReference m_triggerPressed;
        [Tooltip("The input action for when the grip button is pressed")]
        [SerializeField] protected InputActionReference m_gripPressed;
        [Tooltip("The input action for when the touchpad is touched")]
        [SerializeField] protected InputActionReference m_touchpadTouched;
        [Tooltip("The input action for when the touchpad is pressed")]
        [SerializeField] protected InputActionReference m_touchpadPressed;
        [Tooltip("The input action for when the thumbstick is pressed")]
        [SerializeField] protected InputActionReference m_thumbstickPressed;

        protected GameObject m_selectedGameObjectReference;

        public GameObject SelectedGameObject
        {
            get => m_selectedGameObjectReference;
            set => m_selectedGameObjectReference = value;
        }

        /// <summary>
        /// Called when the primary button is pressed
        /// </summary>
        /// <param name="context"></param>
        protected virtual void PrimaryButtonPressed(InputAction.CallbackContext context) { }

        /// <summary>
        /// Called when the secondary button is pressed
        /// </summary>
        /// <param name="context"></param>
        protected virtual void SecondaryButtonPressed(InputAction.CallbackContext context) { }

        /// <summary>
        /// Called when the trigger button is pressed
        /// </summary>
        /// <param name="context"></param>
        protected virtual void TriggerPressed(InputAction.CallbackContext context) { }

        /// <summary>
        /// Called when the grip button is pressed
        /// </summary>
        /// <param name="context"></param>
        protected virtual void GripPressed(InputAction.CallbackContext context) { }

        /// <summary>
        /// Called when the touchpad is touched
        /// </summary>
        /// <param name="context"></param>
        protected virtual void TouchpadTouched(InputAction.CallbackContext context) { }

        /// <summary>
        /// Called when the touchpad is pressed
        /// </summary>
        /// <param name="context"></param>
        protected virtual void TouchpadPressed(InputAction.CallbackContext context) { }

        /// <summary>
        /// Called when the primary button is released
        /// </summary>
        /// <param name="context"></param>

        protected virtual void PrimaryButtonReleased(InputAction.CallbackContext context) { }

        /// <summary>
        /// Called when the secondary button is released
        /// </summary>
        /// <param name="context"></param>
        protected virtual void SecondaryButtonReleased(InputAction.CallbackContext context) { }

        /// <summary>
        /// Called when the trigger button is released
        /// </summary>
        /// <param name="context"></param>
        protected virtual void TriggerReleased(InputAction.CallbackContext context) { }

        /// <summary>
        /// Called when the grip button is released
        /// </summary>
        /// <param name="context"></param>
        protected virtual void GripReleased(InputAction.CallbackContext context) { }

        /// <summary>
        /// Binds all the input actions to functions
        /// </summary>
        protected virtual void OnEnable()
        {
            //button pressed bindings
            if (m_primaryButtonPressed != null)
                m_primaryButtonPressed.action.started += PrimaryButtonPressed;

            if (m_secondaryButtonPressed != null)
                m_secondaryButtonPressed.action.started += SecondaryButtonPressed;

            if (m_triggerPressed != null)
                m_triggerPressed.action.started += TriggerPressed;

            if (m_gripPressed != null)
                m_gripPressed.action.started += GripPressed;

            if (m_touchpadTouched != null)
                m_touchpadTouched.action.started += TouchpadTouched;

            if (m_touchpadPressed != null)
                m_touchpadPressed.action.started += TouchpadPressed;

            //button release bindings
            if (m_primaryButtonPressed != null)
                m_primaryButtonPressed.action.canceled += PrimaryButtonReleased;

            if (m_secondaryButtonPressed != null)
                m_secondaryButtonPressed.action.canceled += SecondaryButtonReleased;

            if (m_triggerPressed != null)
                m_triggerPressed.action.canceled += TriggerReleased;

            if (m_gripPressed != null)
                m_gripPressed.action.canceled += GripReleased;
        }

        /// <summary>
        /// unbinds input actions from functions
        /// </summary>
        protected virtual void OnDisable()
        {
            //button pressed bindings
            if (m_primaryButtonPressed != null)
                m_primaryButtonPressed.action.started -= PrimaryButtonPressed;

            if (m_secondaryButtonPressed != null)
                m_secondaryButtonPressed.action.started -= SecondaryButtonPressed;

            if (m_triggerPressed != null)
                m_triggerPressed.action.started -= TriggerPressed;

            if (m_gripPressed != null)
                m_gripPressed.action.started -= GripPressed;

            if (m_touchpadTouched != null)
                m_touchpadTouched.action.started -= TouchpadTouched;

            if (m_touchpadPressed != null)
                m_touchpadPressed.action.started -= TouchpadPressed;

            //button release bindings
            if (m_primaryButtonPressed != null)
                m_primaryButtonPressed.action.canceled -= PrimaryButtonReleased;

            if (m_secondaryButtonPressed != null)
                m_secondaryButtonPressed.action.canceled -= SecondaryButtonReleased;

            if (m_triggerPressed != null)
                m_triggerPressed.action.canceled -= TriggerReleased;

            if (m_gripPressed != null)
                m_gripPressed.action.canceled -= GripReleased;
        }
    }
}
