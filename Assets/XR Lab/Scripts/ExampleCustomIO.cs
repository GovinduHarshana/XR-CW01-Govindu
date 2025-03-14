using UnityEngine;
using UnityEngine.InputSystem;

namespace XRLab
{
    /// <summary>
    /// An example of a custom script that could be 
    /// added to the player within the game.
    /// 
    /// script inherits from BaseCustomTool and primarily
    /// uses the callbacks but overrides the output functions 
    /// which could have additional interactivity added
    /// </summary>
    public class ExampleCustomIO : BaseCustomTool
    {
        [SerializeField] GameObject m_duplicationTarget;
        //[SerializeField] LineRenderer m_lineRen;

        /// <summary>
        /// Called when the primary button is pressed
        /// </summary>
        /// <param name="context"></param>
        protected override void PrimaryButtonPressed(InputAction.CallbackContext context)
        {
            //m_lineRen.SetPosition(m_lineRen.positionCount, gameObject.transform.position);
            Debug.Log("adding point to " + transform.position);
        }

        /// <summary>
        /// Called when the trigger button is pressed
        /// </summary>
        /// <param name="context"></param>
        protected override void TriggerPressed(InputAction.CallbackContext context)
        {
            Debug.Log("trigger press");

            if (m_duplicationTarget == null)
                return;


            m_duplicationTarget = Instantiate(m_duplicationTarget);
            m_duplicationTarget.transform.position = gameObject.transform.position;


            //m_lineRen.SetPosition(m_lineRen.positionCount, gameObject.transform.position);
            //Debug.Log("adding point to " + transform.position);
        }

        /// <summary>
        /// Called when the touchpad is pressed
        /// </summary>
        /// <param name="context"></param>
        protected override void TouchpadPressed(InputAction.CallbackContext context)
        {
            Debug.Log("touchpad press");
        }
    }

}