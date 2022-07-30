using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogMessenger : MonoBehaviour {
    GUIStyle style = new GUIStyle();
    
    private class LogData
    {
        public string LogString;
        public string StackTrace;
        public LogType LogType;

        public LogData(string logString, string stackTrace, LogType logType)
        {
            LogString = logString;
            StackTrace = stackTrace;
            LogType = logType;
        }
    }

    private List<LogData> m_logDatas = new List<LogData>();
    private string m_onGUIString;

    private void Awake()
    {
        Application.logMessageReceived += OnLogMessageReceived;
        style.fontSize = 18;
    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= OnLogMessageReceived;
    }
    private void OnLogMessageReceived(string logString, string stackTrace, LogType logType)
    {
        m_logDatas.Add(new LogData(logString, stackTrace, logType));

        if (m_logDatas.Count > 50)
        {
            m_logDatas.RemoveAt(0);
        }

        m_onGUIString = string.Empty;
        int count = m_logDatas.Count - 1;
        for (int i = count; i >= 0; i--)
        {
            if (i != count)
            {
                m_onGUIString += "\n";
            }

            switch (m_logDatas[i].LogType)
            {
                case LogType.Log:
                    m_onGUIString += "<color=white>";
                    break;
                case LogType.Warning:
                    m_onGUIString += "<color=yellow>";
                    break;
                case LogType.Error:
                case LogType.Exception:
                case LogType.Assert:
                    m_onGUIString += "<color=red>";
                    break;
            }

            m_onGUIString += m_logDatas[i].LogString;
            m_onGUIString += "</color>";
        }
    }
    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), m_onGUIString,style);
    }

}
