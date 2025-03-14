using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// listens for any outputs to the unity console and 
/// adds them to a text object in the world
/// </summary>
[AddComponentMenu("XRLab/Generic VR/Console Passthrough")]
public class UnityConsolePassthrough : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The UI text component to output the console")]
    [SerializeField] Text m_outputText;
    [Tooltip("Reference to the clear button for the console")]
    [SerializeField] Button m_clearButton;

    [Header("Settings")]
    [Tooltip("Should logs be outputted")]
    [SerializeField] bool m_showLogs = true;
    [Tooltip("Should logs be outputted")]
    [SerializeField] bool m_showWarnings = true;
    [Tooltip("Should logs be outputted")]
    [SerializeField] bool m_showErrors = true;
    [Tooltip("Should logs be outputted")]
    [SerializeField] bool m_showExceptions = true;
    [Tooltip("Should logs be outputted")]
    [SerializeField] bool m_showAsserts = true;
    [SerializeField] int m_maximumMessages = 20;
    private int m_msgCount = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (m_msgCount >= m_maximumMessages)
        {
            ClearConsole();
        }
    }

    /// <summary>
    /// Gets called by outputs to onsole and passes message through
    /// to the output reference 
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="trace">The information of the error location in the scripts</param>
    /// <param name="type">The type of message</param>
    private void ConsoleCallback(string message, string trace, LogType type)
    {
        if (m_outputText == null) //stopping errors
            return;

        if(type == LogType.Error && m_showErrors)
            m_outputText.text += (type + " - " + message + " " + trace + "\n\n");
        if (type == LogType.Exception && m_showExceptions)
            m_outputText.text += (type + " - " + message + " " + trace + "\n\n");
        if (type == LogType.Assert && m_showAsserts)
            m_outputText.text += (type + " - " + message + " " + trace + "\n\n");
        if (type == LogType.Warning && m_showWarnings)
            m_outputText.text += (type + " - " + message + " " + trace + "\n\n");
        else if(type == LogType.Log && m_showLogs)
            m_outputText.text += (type + " - " + message + "\n\n");

        m_msgCount++; //counting up the messages so they can be cleaned up to prevent crashes or errors
    }

    /// <summary>
    /// sets the output text to nothing and resets
    /// the message count to 0 
    /// </summary>
    private void ClearConsole()
    {
        m_outputText.text = "";
        m_msgCount = 0;
    }

    private void OnEnable()
    {
        Application.logMessageReceived += ConsoleCallback;

        if(m_clearButton != null)
            m_clearButton.onClick.AddListener(() => ClearConsole());
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= ConsoleCallback;
        
        if(m_clearButton != null)
            m_clearButton.onClick.RemoveAllListeners();
    }
}
