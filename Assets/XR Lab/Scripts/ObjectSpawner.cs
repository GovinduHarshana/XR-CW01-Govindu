using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("XRLab/XR Editor/Menu Object Spawner")]
public class ObjectSpawner : MonoBehaviour
{
    [Header("Prefab References")]
    [Tooltip("All of the prefabs that the player will be able to spawn in the game")]
    [SerializeField] private GameObject[] m_prefabsToSpawn;
    [Tooltip("The game object that will be used to show a preview of the object the player has selected. Note the object requires a MeshFilter and MeshRenderer component to work correctly")]
    [SerializeField] private GameObject m_preview;

    [Tooltip("The text UI to output information about which prefab is currenrtly selected by the player")]
    [SerializeField] private Text m_prefabIndexReadout;

    [Header("Buttons")]
    [SerializeField] Button m_spawnPrefabButton;
    [SerializeField] Button m_previousPrefabButton;
    [SerializeField] Button m_nextPrefabButton;

    private int m_prefabSelectIndex = 0; //used to determine which prefab to show on UI and spawn 
    
    private MeshFilter m_previewMeshFilter;
    private MeshRenderer m_previewMeshRender;

    // Awake is called after the first frame update
    void Awake()
    {
        m_previewMeshFilter = m_preview.GetComponent<MeshFilter>();
        m_previewMeshRender = m_preview.GetComponent<MeshRenderer>();

        ChangePreview();
        
        if(m_prefabIndexReadout != null)
            m_prefabIndexReadout.text = ("Prefab " +(m_prefabSelectIndex + 1) + "/" + m_prefabsToSpawn.Length);
    }

    public void ChangeSelectedPrefab(bool nextPrefab)
    {
        if (nextPrefab)
        { 
            if(m_prefabSelectIndex < m_prefabsToSpawn.Length - 1)
            {
                m_prefabSelectIndex++;
            }
            else
            {
                m_prefabSelectIndex = 0;    
            }
        }
        else
        {
            if (m_prefabSelectIndex > 0)
            {
                m_prefabSelectIndex--;
            }
            else
            {
                m_prefabSelectIndex = m_prefabsToSpawn.Length - 1;
            }
        }

        if(m_preview != null)
            ChangePreview(); //changing the preview model to be the one that is currently selected 

        if (m_prefabIndexReadout != null)
            m_prefabIndexReadout.text = ("Prefab " + (m_prefabSelectIndex + 1) + "/" + m_prefabsToSpawn.Length);
    }

    private void ChangePreview()
    {
        if (m_prefabsToSpawn.Length > 0) //if the array isnt empty
        {
            if (!m_prefabsToSpawn[m_prefabSelectIndex].Equals(null)) //if the selected entry isnt a null reference then instance the object
            {
                m_previewMeshFilter.sharedMesh = m_prefabsToSpawn[m_prefabSelectIndex].GetComponent<MeshFilter>().sharedMesh;
                m_previewMeshRender.sharedMaterials = m_prefabsToSpawn[m_prefabSelectIndex].GetComponent<MeshRenderer>().sharedMaterials;

                //imagePreview.texture = UnityEditor.AssetPreview.GetAssetPreview(prefabsToSpawn[prefabSelectIndex]);
            }
            else //if the object is null then spawn a blank gameobject
            {
                m_preview = new GameObject();
            }
        }
        else //if the array is empty instance an empty
        {
            m_preview = new GameObject();
        }
    }

    public void SpawnSelectedPrefab()
    {
        if (m_prefabsToSpawn.Length > 0) //if the array isnt empty
        {
            if (m_prefabsToSpawn[m_prefabSelectIndex] != null) //if the first entry isnt a null reference then instance the object
            {
                SpawnPrefab(m_prefabsToSpawn[m_prefabSelectIndex]);
            }
            else
            {
                Debug.LogError("ERROR: selected prefab is assigned as NULL. have to added the reference correctly");
            }
        }
        else
        {
            Debug.LogError("ERROR: prefabs array for object spawner is set up incorrectly. is the array length 0?");
        }
    }


    /// <summary>
    /// Instantiates the passed prefab and moves the object
    /// to the preview location. Also adds object to seperate layer
    /// for later access from selector scripts
    /// </summary>
    /// <param name="obj">The object to instance</param>
    public void SpawnPrefab(GameObject obj)
    {
        GameObject objInst; //holder for the object instance so we can modify it after instantiating 

        objInst = Instantiate(obj) as GameObject;
        objInst.transform.position = m_preview.transform.position; 
        objInst.layer = 31;
    }

    private void OnEnable()
    {
        m_spawnPrefabButton.onClick.AddListener(() => SpawnSelectedPrefab());
        m_nextPrefabButton.onClick.AddListener(() => ChangeSelectedPrefab(true));
        m_previousPrefabButton.onClick.AddListener(() => ChangeSelectedPrefab(false));

        //m_deletePrefabButton.onClick.AddListener(() => )
    }
}
