using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XRLab
{
    [AddComponentMenu("XRLab/XR Editor/Menu Rigidbody Manager")]
    public class ObjectRigidBodyManager : MonoBehaviour
    {
        #region Declarations
        [Header("Gravity UI Elements")]
        [Tooltip("The toggle UI object for changing gravity")]
        [SerializeField] private Toggle useGravityToggle;
        [Tooltip("The toggle UI object for changing the is kinematic property")]
        [SerializeField] private Toggle m_isKinematicToggle;

        [Header("Mass UI Elements")]
        [Tooltip("The readout for the current mass of the selected object")]
        [SerializeField] private Text m_massReadout;
        [Tooltip("the slider for changing the mass value of the selected object")]
        [SerializeField] private Slider m_massSlider;
        [Tooltip("the button to change the max value of the mass slider to 10")]
        [SerializeField] private Button m_maxMassButt10;
        [Tooltip("the button to change the max value of the mass slider to 100")]
        [SerializeField] private Button m_maxMassButt100;
        [Tooltip("the button to change the max value of the mass slider to 1000")]
        [SerializeField] private Button m_maxMassButt1000;

        [Header("Drag UI Elements")]
        [Tooltip("The readout for the current drag of the selected object")]
        [SerializeField] private Text m_dragReadout;
        [Tooltip("the slider for changing the drag value of the selected object")]
        [SerializeField] private Slider m_dragSlider;
        [Tooltip("the button to change the max value of the drag slider to 10")]
        [SerializeField] private Button m_maxDragButt10;
        [Tooltip("the button to change the max value of the drag slider to 100")]
        [SerializeField] private Button m_maxDragButt100;
        [Tooltip("the button to change the max value of the drag slider to 1000")]
        [SerializeField] private Button m_maxDragButt1000;

        [Header("Angular Drag UI Elements")]
        [Tooltip("The readout for the current angular drag of the selected object")]
        [SerializeField] private Text m_angDragReadout;
        [Tooltip("the slider for changing the angular drag value of the selected object")]
        [SerializeField] private Slider m_angDragSlider;
        [Tooltip("the button to change the max value of the angular drag slider to 0.1")]
        [SerializeField] private Button m_maxAngDragButt01;
        [Tooltip("the button to change the max value of the angular drag slider to 0.5")]
        [SerializeField] private Button m_maxAngDragButt05;
        [Tooltip("the button to change the max value of the angular drag slider to 1")]
        [SerializeField] private Button m_maxAngDragButt1;

        [Header("Freeze Constraint UI Elements")]
        [Header("Freeze Position Toggles")]
        [SerializeField] private Toggle m_freezeXPos;
        [SerializeField] private Toggle m_freezeYPos;
        [SerializeField] private Toggle m_freezeZPos;

        [Header("Freeze Rotation Toggles")]
        [SerializeField] private Toggle m_freezeXRot;
        [SerializeField] private Toggle m_freezeYRot;
        [SerializeField] private Toggle m_freezeZRot;

        public GameObject m_selectedGameObject; // the currently selected game object. can probably be changed to a local variable to improve RAM useage at the cost of 1 cpu cycle
        private Rigidbody m_selectedRigidbody; //the currently selected rigid body

        private bool[] m_freezePosHolder = new bool[3];
        private bool[] m_freezeRotHolder = new bool[3];

        private bool m_loadingFromSelectedObject; //boolean used to load in values into the UI from the rigid body when a new object is selecte
        #endregion
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //Physics.gravity = new Vector3(1, 0, 0);
        }

        private void ToggleAttribute(string attribute, bool value)
        {
            if (m_selectedRigidbody == null) //if there isn't an object selected, stop running
                return;

            if (attribute.Equals("gravity"))
            {
                m_selectedRigidbody.useGravity = value;
                Debug.Log("setting gravity to " + value);
            }
            else if (attribute.Equals("kinematic"))
            {
                m_selectedRigidbody.isKinematic = value;
            }
            else if (attribute.Equals("freezePosX"))
            {
                m_freezePosHolder[0] = value;
                UpdateConstraints();
            }
            else if (attribute.Equals("freezePosY"))
            {
                m_freezePosHolder[1] = value;
                UpdateConstraints();
            }
            else if (attribute.Equals("freezePosZ"))
            {
                m_freezePosHolder[2] = value;
                UpdateConstraints();
            }
            else if (attribute.Equals("freezeRotX"))
            {
                m_freezeRotHolder[0] = value;
                UpdateConstraints();
            }
            else if (attribute.Equals("freezeRotY"))
            {
                m_freezeRotHolder[1] = value;
                UpdateConstraints();
            }
            else if (attribute.Equals("freezeRotZ"))
            {
                m_freezeRotHolder[2] = value;
                UpdateConstraints();
            }
        }

        /// <summary>
        /// changes an attribute of the selected rigid body to a float value
        /// </summary>
        /// <param name="attribute">name of attribute to change</param>
        /// <param name="value">value to set on attribute</param>
        private void ChangeAttribute(string attribute, float value)
        {
            if (m_selectedRigidbody == null) //if there isn't an object selected, stop running
                return;

            if (attribute.Equals("mass"))
            {
                m_massReadout.text = "Mass: " + value;
                m_selectedRigidbody.mass = value;
            }
            else if (attribute.Equals("drag"))
            {
                m_dragReadout.text = "drag: " + value;
                m_selectedRigidbody.drag = value;
            }
            else if (attribute.Equals("angular drag"))
            {
                m_angDragReadout.text = "Angular Drag: " + value;
                m_selectedRigidbody.angularDrag = value;
            }
        }

        private void UpdateConstraints()
        {
            if (m_selectedRigidbody == null || m_loadingFromSelectedObject) //if there isn't an object selected, stop running
                return;

            m_selectedRigidbody.constraints = RigidbodyConstraints.None;

            if (m_freezePosHolder[0])
                m_selectedRigidbody.constraints |= RigidbodyConstraints.FreezePositionX;

            if (m_freezePosHolder[1])
                m_selectedRigidbody.constraints |= RigidbodyConstraints.FreezePositionY;

            if (m_freezePosHolder[2])
                m_selectedRigidbody.constraints |= RigidbodyConstraints.FreezePositionZ;

            if (m_freezeRotHolder[0])
                m_selectedRigidbody.constraints |= RigidbodyConstraints.FreezeRotationX;

            if (m_freezeRotHolder[1])
                m_selectedRigidbody.constraints |= RigidbodyConstraints.FreezeRotationY;

            if (m_freezeRotHolder[2])
                m_selectedRigidbody.constraints |= RigidbodyConstraints.FreezeRotationZ;
        }

        /// <summary>
        /// update the UI to have the current values of the selected rigid body
        /// </summary>
        private void UpdateUI()
        {
            //if there is no game object selected then do not attempt to update UI
            if (m_selectedGameObject == null)
                return;

            m_selectedRigidbody = m_selectedGameObject.GetComponent<Rigidbody>(); //getting all of the materials in the object

            if (m_loadingFromSelectedObject && m_selectedRigidbody != null) //note this is only ran when an object is initially loaded. running this constantly causes callback conflicts
            {
                //setting toggle values
                useGravityToggle.isOn = m_selectedRigidbody.useGravity;
                m_isKinematicToggle.isOn = m_selectedRigidbody.isKinematic;

                //setting slider values 
                m_massSlider.value = m_selectedRigidbody.mass;
                m_dragSlider.value = m_selectedRigidbody.drag;
                m_angDragSlider.value = m_selectedRigidbody.angularDrag;

                //checking which constraints the selected object has and updating ui
                m_freezeXPos.isOn = HasConstraint(m_selectedRigidbody.constraints, RigidbodyConstraints.FreezePositionX);
                m_freezeYPos.isOn = HasConstraint(m_selectedRigidbody.constraints, RigidbodyConstraints.FreezePositionY);
                m_freezeZPos.isOn = HasConstraint(m_selectedRigidbody.constraints, RigidbodyConstraints.FreezePositionZ);
                m_freezeXRot.isOn = HasConstraint(m_selectedRigidbody.constraints, RigidbodyConstraints.FreezeRotationX);
                m_freezeYRot.isOn = HasConstraint(m_selectedRigidbody.constraints, RigidbodyConstraints.FreezeRotationY);
                m_freezeZRot.isOn = HasConstraint(m_selectedRigidbody.constraints, RigidbodyConstraints.FreezeRotationZ);

                m_loadingFromSelectedObject = false;
            }
        }

        /// <summary>
        /// sets the maximum value of the slider passed to it
        /// </summary>
        /// <param name="slider">the target slider to change</param>
        /// <param name="maxValue">the maximum value for the slider</param>
        private void SetSliderMax(Slider slider, float maxValue)
        {
            slider.maxValue = maxValue;
        }

        /// <summary>
        /// sets the selected game object to the object passed to it. 
        /// </summary>
        /// <param name="obj">The new selected object</param>
        /// <param name="updateUI">If true, will update UI immediatly after setting new selected object</param>
        public void SetSelectedObject(GameObject obj, bool updateUI)
        {
            m_selectedGameObject = obj; //assigning the reference to the selected object

            //selectedRigidbody = selectedGameObject.GetComponent<Rigidbody>(); //probably not required 

            m_loadingFromSelectedObject = true;

            if (updateUI) //optionally update UI immediatly after getting sew target 
                UpdateUI();

        }

        /// <summary>
        /// Compares the target constraint to the expected constraint
        /// and returns true if they match
        /// </summary>
        /// <param name="targetRigidBodyConstraint"> The constraint to check</param>
        /// <param name="expectedConstraint"> The value to find</param>
        /// <returns>true if constraint is found, false if it is not</returns>
        private bool HasConstraint(RigidbodyConstraints targetRigidBodyConstraint, RigidbodyConstraints expectedConstraint)
        {
            //if ((targetRigidBodyConstraint & expectedConstraint) == expectedConstraint) //checks if constraint is this specific constraint

            if ((targetRigidBodyConstraint & expectedConstraint) != RigidbodyConstraints.None) //checks if constraints contain the target
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void OnEnable()
        {
            useGravityToggle.onValueChanged.AddListener((value) => ToggleAttribute("gravity", value));
            m_isKinematicToggle.onValueChanged.AddListener((value) => ToggleAttribute("kinematic", value));

            m_massSlider.onValueChanged.AddListener((value) => ChangeAttribute("mass", value));
            m_dragSlider.onValueChanged.AddListener((value) => ChangeAttribute("drag", value));
            m_angDragSlider.onValueChanged.AddListener((value) => ChangeAttribute("angular drag", value));

            m_maxMassButt10.onClick.AddListener(() => SetSliderMax(m_massSlider, 10f));
            m_maxMassButt100.onClick.AddListener(() => SetSliderMax(m_massSlider, 100f));
            m_maxMassButt1000.onClick.AddListener(() => SetSliderMax(m_massSlider, 1000f));

            m_maxDragButt10.onClick.AddListener(() => SetSliderMax(m_dragSlider, 10f));
            m_maxDragButt100.onClick.AddListener(() => SetSliderMax(m_dragSlider, 100f));
            m_maxDragButt1000.onClick.AddListener(() => SetSliderMax(m_dragSlider, 1000f));

            m_maxAngDragButt01.onClick.AddListener(() => SetSliderMax(m_angDragSlider, 0.1f));
            m_maxAngDragButt05.onClick.AddListener(() => SetSliderMax(m_angDragSlider, 0.5f));
            m_maxAngDragButt1.onClick.AddListener(() => SetSliderMax(m_angDragSlider, 1f));

            m_freezeXPos.onValueChanged.AddListener((value) => ToggleAttribute("freezePosX", value));
            m_freezeYPos.onValueChanged.AddListener((value) => ToggleAttribute("freezePosY", value));
            m_freezeZPos.onValueChanged.AddListener((value) => ToggleAttribute("freezePosZ", value));

            m_freezeXRot.onValueChanged.AddListener((value) => ToggleAttribute("freezeRotX", value));
            m_freezeYRot.onValueChanged.AddListener((value) => ToggleAttribute("freezeRotY", value));
            m_freezeZRot.onValueChanged.AddListener((value) => ToggleAttribute("freezeRotZ", value));
        }

        private void OnDisable()
        {
            useGravityToggle.onValueChanged.RemoveAllListeners();
            m_isKinematicToggle.onValueChanged.RemoveAllListeners();

            m_massSlider.onValueChanged.RemoveAllListeners();
            m_dragSlider.onValueChanged.RemoveAllListeners();
            m_angDragSlider.onValueChanged.RemoveAllListeners();

            m_maxMassButt10.onClick.RemoveAllListeners();
            m_maxMassButt100.onClick.RemoveAllListeners();
            m_maxMassButt1000.onClick.RemoveAllListeners();

            m_maxDragButt10.onClick.RemoveAllListeners();
            m_maxDragButt100.onClick.RemoveAllListeners();
            m_maxDragButt1000.onClick.RemoveAllListeners();

            m_maxAngDragButt01.onClick.RemoveAllListeners();
            m_maxAngDragButt05.onClick.RemoveAllListeners();
            m_maxAngDragButt1.onClick.RemoveAllListeners();
        }
    }

}