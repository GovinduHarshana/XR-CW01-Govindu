using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * TODO:
 * - replace update colour with callback code for efficiency and cleaner code
 * - add further ducumentation onto code
 * - add support for toggling attributes
 */

[AddComponentMenu("XRLab/XR Editor/Menu Material Manager")]
public class ObjectMaterialManager : MonoBehaviour
{

    #region Declarations
    #region Game Object References
    [Header("Text References")]
    [Tooltip("The Text reference for the label that shows which material index is currently selected")]
    [SerializeField] private Text selectedMaterialReadout;    
    [Tooltip("The Text reference to the readout indicating the current colour of the material")]
    [SerializeField] private Text materialColourReadout;

    [Header("Slider References")]
    [Header("Colour")]
    [Tooltip("Slider for a colour channel, if left unassigned regular OnValueChanged callbacks will be required, if assigned correctly, colour can be changed automatically")]
    [SerializeField] private Slider redSlider;
    [Tooltip("Slider for a colour channel, if left unassigned regular OnValueChanged callbacks will be required, if assigned correctly, colour can be changed automatically")]
    [SerializeField] private Slider greenSlider;
    [Tooltip("Slider for a colour channel, if left unassigned regular OnValueChanged callbacks will be required, if assigned correctly, colour can be changed automatically")]
    [SerializeField] private Slider blueSlider;
    [Tooltip("Slider for a colour channel, if left unassigned regular OnValueChanged callbacks will be required, if assigned correctly, colour can be changed automatically")]
    [SerializeField] private Slider alphaSlider;

    [Header("Emissive Colour")]
    [Tooltip("Slider for a colour channel, if left unassigned regular OnValueChanged callbacks will be required, if assigned correctly, colour can be changed automatically")]
    [SerializeField] private Slider emissiveRedSlider;
    [Tooltip("Slider for a colour channel, if left unassigned regular OnValueChanged callbacks will be required, if assigned correctly, colour can be changed automatically")]
    [SerializeField] private Slider emissiveGreenSlider;
    [Tooltip("Slider for a colour channel, if left unassigned regular OnValueChanged callbacks will be required, if assigned correctly, colour can be changed automatically")]
    [SerializeField] private Slider emissiveBlueSlider;

    [Header("Offset Settings")]
    [Tooltip("Slider for the texture offset on the X axis")]
    [SerializeField] private Slider offsetX;
    [Tooltip("Slider for the texture offset on the Y axis")]
    [SerializeField] private Slider offsetY;

    [Header("Additional Settings")]
    [Tooltip("Slider for a colour channel, if left unassigned regular OnValueChanged callbacks will be required, if assigned correctly, colour can be changed automatically")]
    [SerializeField] private Slider metallicSlider;
    [Tooltip("Slider for a colour channel, if left unassigned regular OnValueChanged callbacks will be required, if assigned correctly, colour can be changed automatically")]
    [SerializeField] private Slider specularSlider; 
    #endregion

    public GameObject m_selectedGameObject; //the current game object that is selected by the user

    private int m_selectedMaterialIndex = 0; //the index for the material the user selects
    //private Material[] selectedMaterials; //an array of all of the materials that are cotained within the selected object 
    public Material m_selectedMat; //the material of the currently selected object

    private Color m_targetColour; //holder for the colour that the user will apply when modifying the base colour
    private Vector3 m_targetEmissiveColour; //the emissive colour holder 
    private bool m_loadingFromSelectedObject = false; //used to prevent unintentional glitches when loading in material data from newly selected objects
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        if (materialColourReadout == null)
            Debug.LogWarning("WARNING: Material text readout is unassigned, please assign it to a Text object to avoid instability");

        //find a better fix for this later
        //m_selectedMat = gameObject.GetComponent<MeshRenderer>().material;
    }

    /// <summary>
    /// sets the selected game object to the object passed to it. 
    /// </summary>
    /// <param name="obj">The new selected object</param>
    /// <param name="updateUI">If true, will update UI immediatly after setting new selected object</param>
    public void SetSelectedObject(GameObject obj, bool updateUI)
    {
        m_selectedMaterialIndex = 0;
        m_selectedGameObject = obj; //assigning the reference to the selected object

        m_loadingFromSelectedObject = true; 

        if (updateUI) //optionally update UI immediatly after getting new target 
            UpdateUI();
        
    }

    /// <summary>
    /// updates the UI with all available information from the
    /// selected material
    /// </summary>
    private void UpdateUI()
    {
        if (m_selectedGameObject == null)
            return;

        //setting the material to whatever the selected index is, note if an object without a mesh renderer is selected, will fire a null reference 
        m_selectedMat = m_selectedGameObject.GetComponent<MeshRenderer>().materials[m_selectedMaterialIndex];

        if (m_selectedMat.shader.name == "Standard")
        {
            //updating the material to show how many materials there are and which material is selected
            selectedMaterialReadout.text = ("Material " + (m_selectedMaterialIndex + 1) + "/" + m_selectedGameObject.GetComponent<MeshRenderer>().materials.Length);
            //updating the colour label to show the currently assigned colour
            materialColourReadout.text = ("Colour: " + m_selectedMat.color);

            //if values are being loaded then do this code as well while updating UI
            if (m_loadingFromSelectedObject)
            {
                    //changing the value f the colour sliders to match the value of the selected object
                    redSlider.value = m_selectedMat.color.r;
                    greenSlider.value = m_selectedMat.color.g;
                    blueSlider.value = m_selectedMat.color.b;
                    alphaSlider.value = m_selectedMat.color.a;

                    //changing the values of the emissive sliders to show the values found in the selected object
                    emissiveRedSlider.value = m_selectedMat.GetVector("_EmissionColor").x;
                    emissiveGreenSlider.value = m_selectedMat.GetVector("_EmissionColor").y;
                    emissiveBlueSlider.value = m_selectedMat.GetVector("_EmissionColor").z;

                    //getting thhe metallic and specular values of the object and setting the sliders to that value
                    metallicSlider.value = m_selectedMat.GetFloat("_Metallic");
                    specularSlider.value = m_selectedMat.GetFloat("_Glossiness");



                //changing the offset values to match the selected material
                offsetX.value = m_selectedMat.mainTextureOffset.x;
                offsetY.value = m_selectedMat.mainTextureOffset.y;

                //setting the loading bool to false to increase efficiency
                m_loadingFromSelectedObject = false;
            }
        }
        else
        {
            Debug.Log("Warning: Shader " + m_selectedMat.shader.name + " is not supported, menu will not function correctly");
        }
    }


    #region Slider changes (Not currently in use)
    /// <summary>
    /// manually change just the red clour channel
    /// </summary>
    /// <param name="value"> the colour value from the slider</param>
    public void ChangeRedChannel(float value)
    {
        m_targetColour.r = value;
    }

    /// <summary>
    /// manually change just the green clour channel
    /// </summary>
    /// <param name="value"> the colour value from the slider</param>
    public void ChangeGreenChannel(float value)
    {
        m_targetColour.g = value;
    }

    /// <summary>
    /// manually change just the blue clour channel
    /// </summary>
    /// <param name="value"> the colour value from the slider</param>
    public void ChangeBlueChannel(float value)
    {
        m_targetColour.b = value;
    }

    /// <summary>
    /// manually change just the alpha clour channel
    /// </summary>
    /// <param name="value"> the colour value from the slider</param>
    public void ChangeAlphaChannel(float value)
    {
        m_targetColour.a = value;
    }

    public void ChangeMetallicChannel(float value)
    {
        m_selectedMat.SetFloat("_Metallic", value);
    }
    #endregion

    /// <summary>
    /// Iterates through the array of materials on an object and 
    /// sets the current index to the selected material
    /// </summary>
    /// <param name="nextMat"> Makes the selection fo forwards or backwards</param>
    public void ChangeSelectedMaterial(bool nextMat)
    {
        int materialsLength = m_selectedGameObject.GetComponent<MeshRenderer>().materials.Length;

        if (nextMat)
        {
            if (m_selectedMaterialIndex < materialsLength - 1)
            {
                m_selectedMaterialIndex++;
            }
            else
            {
                m_selectedMaterialIndex = 0;
            }
        }
        else
        {
            if (m_selectedMaterialIndex > 0)
            {
                m_selectedMaterialIndex--;
            }
            else
            {
                m_selectedMaterialIndex = materialsLength - 1;
            }
        }

        m_loadingFromSelectedObject = true;
        UpdateUI();

       // selectedMaterialReadout.text = ("Material " + (selectedMaterialIndex + 1) + "/" + materialsLength);
    }

    /// <summary>
    /// take the colour values from the target colour
    /// and apply them to the target object
    /// </summary>
    public void UpdateColourValues()
    {
        if (m_loadingFromSelectedObject || m_selectedMat == null) //if there is an object being loaded then return here to prevent unintentional updates
            return;

        //modifying the colour holder with the values in the sliders
        m_targetColour.r = redSlider.value;
        m_targetColour.g = greenSlider.value;
        m_targetColour.b = blueSlider.value;
        m_targetColour.a = alphaSlider.value;
        //setting the material to the selected colour created above
        m_selectedMat.color = m_targetColour;

        //modifying the vector4 that represents the Emissive shader parameter. note this parameter may not be in all shaders
        m_targetEmissiveColour.x = emissiveRedSlider.value;
        m_targetEmissiveColour.y = emissiveGreenSlider.value;
        m_targetEmissiveColour.z = emissiveBlueSlider.value;
        //enabling emissive colour and then attempting to assign the value to the selected material
        m_selectedMat.EnableKeyword("_EMISSION");
        m_selectedMat.SetVector("_EmissionColor", m_targetEmissiveColour);

        //setting the metallic and specularity parameters of the shaders. note depending on the shader these parameters may not be present
        m_selectedMat.SetFloat("_Metallic", metallicSlider.value);
        m_selectedMat.SetFloat("_Glossiness", specularSlider.value);

        m_selectedMat.mainTextureOffset = new Vector2(offsetX.value, offsetY.value);

        //updating the UI with the relevant material information
        UpdateUI();
    }

    //private void ChangeAttribute(string attribute, float value)
    //{
    //    if (attribute.Equals("red colour"))
    //    {
    //        m_targetColour.r = value;
    //    }
    //    else if (attribute.Equals("green colour"))
    //    { 
        
    //    }
    //    else if (attribute.Equals("blue colour"))
    //    {

    //    }
    //    else if (attribute.Equals("alpha colour"))
    //    {

    //    }
    //}

    private void OnEnable()
    {
        redSlider.onValueChanged.AddListener((value) => UpdateColourValues());
        greenSlider.onValueChanged.AddListener((value) => UpdateColourValues());
        blueSlider.onValueChanged.AddListener((value) => UpdateColourValues());
        alphaSlider.onValueChanged.AddListener((value) => UpdateColourValues());

        emissiveRedSlider.onValueChanged.AddListener((value) => UpdateColourValues());
        emissiveGreenSlider.onValueChanged.AddListener((value) => UpdateColourValues());
        emissiveBlueSlider.onValueChanged.AddListener((value) => UpdateColourValues());

        offsetX.onValueChanged.AddListener((value) => UpdateColourValues());
        offsetY.onValueChanged.AddListener((value) => UpdateColourValues());

        metallicSlider.onValueChanged.AddListener((value) => UpdateColourValues());
        specularSlider.onValueChanged.AddListener((value) => UpdateColourValues());
    }
}
