using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConflictSystem : MonoBehaviour, Panel
{

    List<KeyValuePair<string, string>> remoteFiles;
    List<KeyValuePair<string, string>> localFiles;
    List<string> conflictFiles;

    [SerializeField]
    GameObject inputObject;
    [SerializeField]
    Text confilectedFileName;
    [SerializeField]
    GameObject remoteFilesObject;
    [SerializeField]
    GameObject localFilesObject;
    [SerializeField]
    InputField outputFilesObject;

    [SerializeField]
    GitSystem gitSystem;

    public bool usedRemote { private set; get; } = true;

    bool branch = false;
    string branchName;

    [SerializeField]
    Toggle localToggle;
    [SerializeField]
    Toggle remoteToggle;

    void Start()
    {
        Button[] btns = inputObject.GetComponentsInChildren<Button>();
        // close file
        btns[0].onClick.AddListener(delegate
        {
            SoloveConflict();
            gameObject.SetActive(false);
        }
        );

        localToggle.onValueChanged.AddListener(delegate
        {
            SetOutputValue();
        });

        remoteToggle.onValueChanged.AddListener(delegate
        {
            SetOutputValue();
        });
    }

    public void PanelInput()
    {

    }

    private void SetOutputValue()
    {
        if (localToggle.isOn && remoteToggle.isOn)
        {
            foreach (KeyValuePair<string, string> file in localFiles)
            {
                if (file.Key == conflictFiles[0])
                {
                    outputFilesObject.text = file.Value;
                    usedRemote = true;
                    break;
                }
            }
            foreach (KeyValuePair<string, string> file in remoteFiles)
            {
                if (file.Key == conflictFiles[0])
                {
                    outputFilesObject.text = "<color=red>" + outputFilesObject.text + "</color>" + "\n" + "<color=yellow>" + file.Value + "</color>";
                    break;
                }
            }
        }
        else if (localToggle.isOn)
        {
            foreach (KeyValuePair<string, string> file in localFiles)
            {
                if (file.Key == conflictFiles[0])
                {
                    outputFilesObject.text = file.Value;
                    usedRemote = false;
                    outputFilesObject.textComponent.color = Color.red;
                    break;
                }
            }
        }
        else if (remoteToggle.isOn)
        {
            foreach (KeyValuePair<string, string> file in remoteFiles)
            {
                if (file.Key == conflictFiles[0])
                {
                    outputFilesObject.text = file.Value;
                    usedRemote = true;
                    outputFilesObject.textComponent.color = Color.yellow;
                    break;
                }
            }
        }
        if ( !remoteToggle.isOn && !localToggle.isOn)
        {
            outputFilesObject.text = "";
            usedRemote = false;
            outputFilesObject.textComponent.color = Color.white;
        }
    }

    public void OpenConflict(List<string> conflictFiles, List<KeyValuePair<string, string>> localFiles, List<KeyValuePair<string, string>> remoteFiles, bool branch,string branchName)
    {
        this.conflictFiles = conflictFiles;
        this.localFiles = localFiles;
        this.remoteFiles = remoteFiles;
        SetWindow();
        this.branch = branch;
        this.branchName = branchName;
    }

    public void SetWindow()
    {
        confilectedFileName.text = conflictFiles[0];
        foreach (KeyValuePair<string, string> file in localFiles)
        {
            if (file.Key == conflictFiles[0])
            {
                localFilesObject.transform.GetChild(0).GetComponent<Text>().text = file.Value;
                break;
            }
        }
        foreach (KeyValuePair<string, string> file in remoteFiles)
        {
            if (file.Key == conflictFiles[0])
            {
                remoteFilesObject.transform.GetChild(0).GetComponent<Text>().text = file.Value;
                break;
            }
        }
        SetOutputValue();
    }

    private void SoloveConflict()
    {
        gitSystem.SolvedConflict(branch,branchName);
        GameSystemManager.GetSystem<PanelManager>().ReturnTopPanel();
    }
}

