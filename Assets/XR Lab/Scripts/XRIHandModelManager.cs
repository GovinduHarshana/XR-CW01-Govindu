using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace XRLab
{
    public class XRIHandModelManager : MonoBehaviour
    {
        public enum HandMode
        {
            idle,
            hidden,
            grabbing,
            rayActive,
            rayTransform,
        }

        public HandMode m_mode;

        [Header("Model references")]
        [SerializeField] GameObject m_idleHandModel;
        [SerializeField] GameObject m_grabHandModel;
        [SerializeField] GameObject m_rayActiveModel;

        [Header("Script references")]
        [SerializeField] XRDirectInteractor m_dirInteract;
        [SerializeField] XRIRayToggle m_rayToggleManager;
        [SerializeField] XRIMenuDockController m_menuDockController;

        /// <summary>
        /// Updates the current hand model based on the passed mode
        /// </summary>
        /// <param name="mode">The current mode the hand is in</param>
        public void UpdateHandModel(HandMode mode)
        {
            m_idleHandModel.SetActive(false);
            m_grabHandModel.SetActive(false);
            m_rayActiveModel.SetActive(false);

            m_mode = mode;

            switch (mode)
            {
                case HandMode.hidden:
                    return;
                //m_idleHandModel.SetActive(false);
                //m_grabHandModel.SetActive(false);
                //m_rayActiveModel.SetActive(false);
                //break;
                case HandMode.idle:
                    m_idleHandModel.SetActive(true);
                    break;
                case HandMode.grabbing:
                    m_grabHandModel.SetActive(true);
                    break;
                case HandMode.rayActive:
                    m_rayActiveModel.SetActive(true);
                    break;
                case HandMode.rayTransform:
                    m_rayActiveModel.SetActive(true);
                    break;
                default:
                    break;
            }


        }

        /// <summary>
        /// The current mode of the hand, used to change model 
        /// </summary>
        public HandMode CurrentHandMode
        {
            get => m_mode;
            set => m_mode = value;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        /// <summary>
        /// runs a state machine that will check the state
        /// of the hands and change them based on what the hands 
        /// are currently doing
        /// </summary>
        void FixedUpdate()
        {
            //note, the further up the list the handmode statement is, the higher it's priority        
            if (m_rayToggleManager.IsRayEnabled) //if the player is using the ray
            {
                m_mode = HandMode.rayActive;
            }
            else if (m_dirInteract.isSelectActive) //if the player activating the grab action
            {
                m_mode = HandMode.grabbing;
            }
            else
            {
                m_mode = HandMode.idle;
            }

            if (m_menuDockController != null)
            {
                if (m_menuDockController.IsDocked)
                {
                    m_mode = HandMode.hidden;
                }
            }

            //note move this out of the update to improve performance
            UpdateHandModel(m_mode);
        }
    }

}