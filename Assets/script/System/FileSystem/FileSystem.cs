using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileSystem : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    List<string> fileNames;
    [SerializeField]
    List<string> fileContents;
    [SerializeField]
    List<GameObject> fileObjects;
    [SerializeField]
    GameObject inputObject;
    [SerializeField]
    int maxFileCount;

    int openedFileIndex;

    void Start()
    {
        for ( int i = 0 ; i < fileNames.Count; i++)
        {
            fileObjects[i].GetComponentInChildren<Text>().text = fileNames[i];
            fileObjects[i].SetActive(true);
            Button btn = fileObjects[i].GetComponentInChildren<Button>();
            btn.gameObject.name = i.ToString();
        }
        for (int i = 0; i < fileObjects.Count; i++)
        {
            Button btn = fileObjects[i].GetComponentInChildren<Button>();
            btn.onClick.AddListener(delegate
            {
                OpenFile(btn.name);
            }
            );
        }

        Button[] btns = inputObject.GetComponentsInChildren<Button>();
        // close file
        btns[0].onClick.AddListener(delegate
            {
                inputObject.SetActive(false);
            }
        );
        // save file
        btns[1].onClick.AddListener(delegate
            {
                fileContents[openedFileIndex] = inputObject.GetComponent<InputField>().text;
            }
        );
    }

    void OpenFile(string index)
    {
        openedFileIndex = int.Parse(index);
        inputObject.GetComponent<InputField>().text = fileContents[openedFileIndex];
        inputObject.SetActive(true);
    }

    public bool DeleteFile(string fileName)
    {
        int index = fileNames.FindIndex( a => a == fileName);
        if (index == -1)
            return false;

        fileObjects[fileNames.Count - 1].SetActive(false);
        fileNames.RemoveAt(index);
        fileContents.RemoveAt(index);


        for (int i = 0; i < fileNames.Count; i++)
        {
            fileObjects[i].GetComponentInChildren<Text>().text = fileNames[i];
            Button btn = fileObjects[i].GetComponentInChildren<Button>();
            btn.gameObject.name = i.ToString();
        }
        return true;

    }
    public bool NewFile(string fileName)
    {
        if (fileNames.Count == maxFileCount)
            return false;
        fileNames.Add(fileName);
        fileContents.Add("");

        for (int i = 0; i < fileNames.Count; i++)
        {
            fileObjects[i].GetComponentInChildren<Text>().text = fileNames[i];
            Button btn = fileObjects[i].GetComponentInChildren<Button>();
            btn.gameObject.name = i.ToString();
        }
        fileObjects[fileNames.Count - 1].SetActive(true);
        return true;
    }

    public bool NewFile(string fileName, string content)
    {
        if (fileNames.Count == maxFileCount)
            return false;
        fileNames.Add(fileName);
        fileContents.Add(content);

        for (int i = 0; i < fileNames.Count; i++)
        {
            fileObjects[i].GetComponentInChildren<Text>().text = fileNames[i];
            Button btn = fileObjects[i].GetComponentInChildren<Button>();
            btn.gameObject.name = i.ToString();
        }
        fileObjects[fileNames.Count - 1].SetActive(true);
        return true;
    }

    public bool SetFileContent(string fileName, string content)
    {
        int index = -1;
        for (int i = 0; i < fileNames.Count; i++)
        {
            if (fileNames[i] == fileName)
            {
                index = i;
            }
        }



        if (index == -1)
        {
            return false;
        }
        fileContents[index] = content;
        return true;
    }

    public bool CopyFile(string originalFile, string newFile)
    {
        if (fileNames.Count == maxFileCount)
            return false;

        int index = fileNames.FindIndex(a => a == originalFile);
        if (index == -1)
            return false;

        fileNames.Add(newFile);
        fileContents.Add(fileContents[index]);

        for (int i = 0; i < fileNames.Count; i++)
        {
            fileObjects[i].GetComponentInChildren<Text>().text = fileNames[i];
            Button btn = fileObjects[i].GetComponentInChildren<Button>();
            btn.gameObject.name = i.ToString();
        }
        fileObjects[fileNames.Count - 1].SetActive(true);
        return true;
    }

    public List<string> getFilesName()
    {
        return fileNames;
    }
    public List<string> getFilesContent()
    {
        return fileContents;
    }

    public void trackFile(string fileName)
    {
        int index = -1;
        for( int i = 0; i< fileNames.Count; i++ )
        {
            if( fileNames[i] == fileName)
            {
                index = i;
            }
        }
        if (index != -1)
        {
            fileObjects[index].GetComponentInChildren<Text>().color = Color.red;
        }
    }
    public void untrackFile(string fileName)
    {
        int index = -1;
        for (int i = 0; i < fileNames.Count; i++)
        {
            if (fileNames[i] == fileName)
            {
                index = i;
            }
        }
        if (index != -1)
        {
            fileObjects[index].GetComponentInChildren<Text>().color = new Color(255, 255, 255);
        }
    }

    public bool fileIsTracked(string fileName)
    {
        int index = -1;
        for (int i = 0; i < fileNames.Count; i++)
        {
            if (fileNames[i] == fileName)
            {
                index = i;
            }
        }
        if (index != -1)
        {
            if (fileObjects[index].GetComponentInChildren<Text>().color == Color.red)
            {
                return true;
            }
        }
        return false;
    }
}
