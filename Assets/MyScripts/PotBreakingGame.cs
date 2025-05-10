using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PotBreakingGame : MonoBehaviour
{
    public static PotBreakingGame Instance;

    [Header("UI Elements")]
    public TMP_Text potCountText; 
    public GameObject congratsPanel;
    public Button closeButton;

    private int potsBroken = 0;

    void Awake()
    {
        Instance = this;
        congratsPanel.SetActive(false);
        closeButton.onClick.AddListener(CloseCongratsPanel);
        UpdatePotCount();
    }

    public void PotBroken()
    {
        potsBroken++;
        UpdatePotCount();

        if (potsBroken == 10)
        {
            ShowCongratsPanel();
        }
    }

    void UpdatePotCount()
    {
        potCountText.text = "Magic Pots: " + potsBroken;
    }

    void ShowCongratsPanel()
    {
        congratsPanel.SetActive(true);
        
    }

    void CloseCongratsPanel()
    {
        congratsPanel.SetActive(false);
        
    }
}
