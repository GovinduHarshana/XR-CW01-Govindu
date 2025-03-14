using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
namespace XRLab
{
    /// <summary>
    /// passes references between all of the manager scripts
    /// and also adds components to object
    /// </summary>
    [AddComponentMenu("XRLab/XR Editor/Menu Component Manager")]
    public class XRIObjectComponentManager : MonoBehaviour
    {
        [System.Serializable]
        [SerializeField]
        private struct AddCustomComponent
        {
            [Tooltip("The reference to the target button")]
            [SerializeField] public Button button;
            [Tooltip("The component type to be added. note this needs to be instanced somewhere within the scene to add")]
            [SerializeField] public Component component;
        }

        [Header("Object Selection References")]
        [Tooltip("The Text label used to define which object is currently selected")]
        [SerializeField] private Text m_selectedObjectName;

        //note: possibly change script refs into an array so multiple menus can be synced together
        [Tooltip("A Reference to the material manager script, if left unassigned will attempt to automatially locate the script")]
        [SerializeField] private ObjectMaterialManager m_materialManager;
        [Tooltip("A Reference to the rigid body manager script, if left unassigned will attempt to automatially locate the script")]
        [SerializeField] private ObjectRigidBodyManager m_rigidbodyManager;
        [Tooltip("An output used to show which components are currently located within the object")]
        [SerializeField] private Text m_output;

        [Header("Select/remove Components")]
        [SerializeField] private Text m_selectedComponentIndexText;
        [SerializeField] private Button m_selectNextComponentButton;
        [SerializeField] private Button m_selectPreviousComponentButton;
        [SerializeField] private Button m_DeleteComponentButton;
        [SerializeField] private Button m_deleteObjectButton;
        [Tooltip("The structures for buttons that add components to objects")]
        [SerializeField] private AddCustomComponent[] m_addComponentButtons;

        private GameObject m_selectedGameObject;
        private Component[] m_selectedObjectComponents;
        private int m_componentSelectIndex = 0;


        void Awake()
        {
            if (m_materialManager == null) //if left unassigned will try and locate a material manager attached to the same game object
                m_materialManager = gameObject.GetComponent<ObjectMaterialManager>();

            if (m_rigidbodyManager == null)
                m_rigidbodyManager = gameObject.GetComponent<ObjectRigidBodyManager>();
        }

        /// <summary>
        /// sets the name for the selected game object in the UI and passes 
        /// the referenceto any scripts that require the selected object 
        /// reference
        /// </summary>
        private void UpdateObjectUI()
        {
            if (m_selectedObjectName != null)
                m_selectedObjectName.text = m_selectedGameObject.name;

            m_materialManager.SetSelectedObject(m_selectedGameObject, true);
            m_rigidbodyManager.SetSelectedObject(m_selectedGameObject, true);

            ListComponents(m_selectedGameObject);
        }

        /// <summary>
        /// attempts to locate all components that can be accessed
        /// by the editor and outputs them to a text object 
        /// </summary>
        /// <param name="obj">The target object</param>
        public void ListComponents(GameObject obj)
        {
            Debug.Log("listing components");
            m_selectedObjectComponents = null;

            m_selectedObjectComponents = obj.GetComponents<Component>();

            m_output.text = "";

            //outputting materials
            m_output.text += "\n" + obj.name + "\n ----- Materials -----\n";
            Material[] mats = obj.GetComponent<MeshRenderer>().materials;

            foreach (Material mat in mats)
            {
                m_output.text += mat.name + "\n";
            }

            m_output.text += "\n ----- Child Objects ----- \n";

            foreach (Transform child in obj.GetComponentInChildren<Transform>())
            {
                m_output.text += child.name + "\n";
            }

            m_output.text += "\n----- Components -----\n";

            for (int i = 0; i < m_selectedObjectComponents.Length; i++)
            {
                m_output.text += i + ". " + m_selectedObjectComponents[i].GetType().Name + "\n";
            }

            m_selectedComponentIndexText.text = "Component " + m_componentSelectIndex + "/" + (m_selectedObjectComponents.Length - 1);
        }

        /// <summary>
        /// when called, will toggle the enabled setting of the passed
        /// object
        /// </summary>
        /// <param name="obj">the reference to the target game object</param>
        public void ToggleGameObjectEnabled(GameObject obj)
        {
            obj.SetActive(!obj.activeInHierarchy);
        }

        /// <summary>
        /// takes the passed object and sets it as the selected game object to allow
        /// referencing from other scripts in game. Also runs the UpdateObjectUI 
        /// function 
        /// </summary>
        /// <param name="obj">The reference to the object being selected</param>
        public void SetSelectedGameObject(GameObject obj)
        {
            m_selectedGameObject = obj;
            UpdateObjectUI();
        }

        private void DeleteSpecificComponent(int index)
        {
            //Debug.Log("yee");

            string output = "Before delete: ";

            for (int i = 0; i < m_selectedObjectComponents.Length; i++)
            {
                output += i + ". " + m_selectedObjectComponents[i].GetType().Name + "\n";
            }

            Debug.Log(output);

            Destroy(m_selectedObjectComponents[index]);

            //running in a coroutine so the code can be ran on the next frame
            StartCoroutine(UpdateComponentListAfterFrame());

            m_componentSelectIndex = 0;
        }

        /// <summary>
        /// updates the lost of components from the selected object after
        /// the next frame is rendered
        /// </summary>
        /// <returns></returns>
        private IEnumerator UpdateComponentListAfterFrame()
        {
            yield return new WaitForEndOfFrame();

            m_selectedObjectComponents = m_selectedGameObject.GetComponents<Component>();

            ListComponents(m_selectedGameObject);
        }

        /// <summary>
        /// when called, will destroy the selected object if it's
        /// layer is 31 (31 is the layer all runtime instanced 
        /// objects are assigned)
        /// </summary>
        public void DeleteSelectedObject()
        {
            if (m_selectedGameObject.layer == 31)
                Destroy(m_selectedGameObject);
        }

        /// <summary>
        /// changes the currently selected component so it can be edited
        /// </summary>
        /// <param name="nextComponent">should the component index go up or down</param>
        private void ChangeSelectedComponent(bool nextComponent)
        {
            if (nextComponent)
            {
                if (m_componentSelectIndex < m_selectedObjectComponents.Length - 1)
                {
                    m_componentSelectIndex++;
                }
                else
                {
                    m_componentSelectIndex = 0;
                }
            }
            else
            {
                if (m_componentSelectIndex > 0)
                {
                    m_componentSelectIndex--;
                }
                else
                {
                    m_componentSelectIndex = m_selectedObjectComponents.Length - 1;
                }
            }

            m_selectedComponentIndexText.text = "Component " + m_componentSelectIndex + "/" + (m_selectedObjectComponents.Length - 1);
        }

        /// <summary>
        /// changes the currently selected component so it can be edited
        /// </summary>
        /// <param name="indexChange">How much the selected component should change</param>
        private void ChangeSelectedComponent(int indexChange)
        {
            if (indexChange > 0)
            {
                if (m_componentSelectIndex < m_selectedObjectComponents.Length - 1)
                {
                    m_componentSelectIndex++;
                }
                else
                {
                    m_componentSelectIndex = 0;
                }
            }
            else if (indexChange < 0)
            {
                if (m_componentSelectIndex > 0)
                {
                    m_componentSelectIndex--;
                }
                else
                {
                    m_componentSelectIndex = m_selectedObjectComponents.Length - 1;
                }
            }

            if (m_selectedComponentIndexText != null)
                m_selectedComponentIndexText.text = "Component " + m_componentSelectIndex + "/" + (m_selectedObjectComponents.Length - 1);
        }

        /// <summary>
        /// Adds a component to the selected game object unless it has the "Floor" tag
        /// </summary>
        /// <param name="component">The component to add, can be any type but must already be instanced in game</param>
        private void AddComponentToObject(Component component)
        {
            //if the selected object is the floor then stop to prevent user unintentionally breaking the game
            if (m_selectedGameObject.CompareTag("Floor"))
                return;

            //getting the type of component passed and adding it to the object
            m_selectedGameObject.AddComponent(component.GetType());

            //listing the components so the most accurate list is shown
            ListComponents(m_selectedGameObject);
        }


        private void OnEnable()
        {
            //going through the add components structure array and setting up the listeners for them
            foreach (AddCustomComponent item in m_addComponentButtons)
            {
                item.button.onClick.AddListener(() => AddComponentToObject(item.component));
            }

            m_selectNextComponentButton.onClick.AddListener(() => ChangeSelectedComponent(1));
            m_selectPreviousComponentButton.onClick.AddListener(() => ChangeSelectedComponent(-1));
            m_DeleteComponentButton.onClick.AddListener(() => DeleteSpecificComponent(m_componentSelectIndex));

            if (m_deleteObjectButton != null)
                m_deleteObjectButton.onClick.AddListener(() => DeleteSelectedObject());
        }

        private void OnDisable()
        {
            //going through the add components structure array and removing all the listeners
            foreach (AddCustomComponent item in m_addComponentButtons)
            {
                item.button.onClick.RemoveAllListeners();
            }

            m_selectNextComponentButton.onClick.RemoveAllListeners();
            m_selectPreviousComponentButton.onClick.RemoveAllListeners();
            m_DeleteComponentButton.onClick.RemoveAllListeners();

            if (m_deleteObjectButton != null)
                m_deleteObjectButton.onClick.RemoveAllListeners();
        }
    }

}