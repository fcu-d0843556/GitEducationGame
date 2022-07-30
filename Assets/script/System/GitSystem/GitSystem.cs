using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GitSystem : MonoBehaviour , Panel
{
    [SerializeField]
    GameObject mainFlag;
    [SerializeField]
    GameObject headFlag;

    List <GameObject> flagObjects = new List<GameObject>();
    List<GameObject> commitObjects = new List<GameObject>();
    [SerializeField]
    GameObject startCommit;
    [SerializeField]
    GameObject remoteObjects;
    [SerializeField]
    GameObject localObjects;
    public GameObject nowCommit { private set; get; }
    public Repository localRepository;

    [SerializeField]
    FileSystem fileSystem;
    [SerializeField]
    ConflictSystem conflictSystem;

    [SerializeField]
    GameObject fileInputField;
    [SerializeField]
    GameObject gitInputField;
    [SerializeField]
    GameObject gitFileObject;
    List<KeyValuePair<string, string>> modifiedFiles;
    List<string> confilctFiles;

    [SerializeField]
    List<string> remotes;

    public Repository serverRepository;

    public bool cloned { private set; get; } = false;
    public bool hasPush { private set; get; } = false;
    public bool sync { private set; get; } = false;
    public bool conflicted { private set; get; } = false;

    public void buildRepository()
    {
        if ( modifiedFiles == null )
        {
            modifiedFiles = new List<KeyValuePair<string, string>>();
        }
        localRepository = new Repository();
        mainFlag.SetActive(true);
        mainFlag.GetComponent<Image>().color = Color.red;
        headFlag = mainFlag;
        flagObjects.Add(mainFlag);
    }

    public bool hasRepository()
    {
        if (localRepository != null)
        {
            return true;
        }
        return false;
    }
    public bool hasRemote()
    {
        if(remotes.Count != 0)
        {
            return true;
        }
        return false;
    }
    public void addRemote(string name)
    {
        remotes.Add(name);
    }

    public void Commit(string name)
    {
        if (hasRepository() && !conflicted)
        {

            Commit newCommit = new Commit(name, "");
            for (int i = 0; i < modifiedFiles.Count; i++)
            {
                newCommit.addModifiedFile(modifiedFiles[i]);
                fileSystem.untrackFile(modifiedFiles[i].Key);

            }
            if (nowCommit == null)
            {
                nowCommit = startCommit;
                startCommit.SetActive(true);
            }
            else
            {
                GameObject newCommitObject = Instantiate(nowCommit, nowCommit.transform.parent);
                newCommitObject.transform.GetChild(1).gameObject.SetActive(true);
                newCommitObject.GetComponent<RectTransform>().localPosition = new Vector3(nowCommit.GetComponent<RectTransform>().localPosition.x - 150, nowCommit.GetComponent<RectTransform>().localPosition.y, nowCommit.GetComponent<RectTransform>().localPosition.z);
                nowCommit = newCommitObject;
            }
            nowCommit.GetComponentInChildren<Text>().text = newCommit.name;
            nowCommit.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(100 - newCommit.name.Length * 5, -49, 0);
            // normal flag set
            headFlag.GetComponent<RectTransform>().localPosition = new Vector3(nowCommit.GetComponent<RectTransform>().localPosition.x - 160, nowCommit.GetComponent<RectTransform>().localPosition.y + 5, headFlag.GetComponent<RectTransform>().localPosition.z);
            // new branch and start commit
            if (localRepository.nowBranch.branchStart)
            {
                nowCommit.GetComponent<RectTransform>().localPosition = new Vector3(nowCommit.GetComponent<RectTransform>().localPosition.x, nowCommit.GetComponent<RectTransform>().localPosition.y - 145, nowCommit.GetComponent<RectTransform>().localPosition.z);
                nowCommit.transform.GetChild(1).GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 45);
                nowCommit.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(67, 45, 0);
                headFlag.GetComponent<RectTransform>().localPosition = new Vector3(headFlag.GetComponent<RectTransform>().localPosition.x, headFlag.GetComponent<RectTransform>().localPosition.y - 145 + 5, headFlag.GetComponent<RectTransform>().localPosition.z);
                nowCommit.GetComponent<Image>().color = Random.ColorHSV();
            }
            else
            {
                nowCommit.transform.GetChild(1).GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
                nowCommit.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(67, 0, 0);
            }
            headFlag.transform.GetChild(1).gameObject.SetActive(true);
            localRepository.Commit(newCommit);
            modifiedFiles = new List<KeyValuePair<string, string>>();
            nowCommit.name = name + "Commit";
            commitObjects.Add(nowCommit);
            //nowCommit.GetComponent<Image>().color = new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255));
            sync = false;
            hasPush = false;
        }
    }

    public void Commit()
    {
        if (hasRepository() && !conflicted)
        {
            gitFileObject.SetActive(true);
            GameSystemManager.GetSystem<PanelManager>().AddSubPanel(this);
        }
    }

    public bool Merge(string branchName)
    {
        if (!localRepository.hasBranch(branchName))
            return false;

        // if conflict
        confilctFiles = new List<string>();
        Branch branch = localRepository.branches.Find(x => x.branchName == branchName);
        for (int i = 0; i < branch.nowCommit.allFiles.Count; i++)
        {
            for (int j = 0; j < localRepository.nowBranch.nowCommit.allFiles.Count; j++)
            {
                //Debug.Log(serverRepository.nowBranch.nowCommit.allFiles[i].Key + " : " +  modifiedFiles[j].Key);
                if (branch.nowCommit.allFiles[i].Key == localRepository.nowBranch.nowCommit.allFiles[j].Key)
                {
                    if (branch.nowCommit.allFiles[i].Value != localRepository.nowBranch.nowCommit.allFiles[j].Value)
                    {
                        Debug.Log("Conflicted: " + localRepository.nowBranch.nowCommit.allFiles[j].Key);
                        Debug.Log(localRepository.nowBranch.nowCommit.allFiles[j].Value + " : " + branch.nowCommit.allFiles[i].Value);
                        confilctFiles.Add(localRepository.nowBranch.nowCommit.allFiles[j].Key);
                    }
                }
            }

        }
        if (confilctFiles.Count > 0)
        {
            conflicted = true;
            conflictSystem.OpenConflict(confilctFiles, localRepository.nowBranch.nowCommit.allFiles, branch.nowCommit.allFiles, true, branchName);
            GameSystemManager.GetSystem<PanelManager>().AddSubPanel(conflictSystem);
            conflictSystem.gameObject.SetActive(true);
        }
        else
        {
            Commit("Merge branch " + branchName);
            nowCommit.transform.GetChild(2).gameObject.SetActive(true);
        }
        return true;
    }

    public bool Pull(string remote, string branch)
    {
        if (remotes.Contains(remote))
        {
           
            if (serverRepository.hasBranch(branch))
            {
                
                confilctFiles = new List<string>();
                //Debug.Log(serverRepository.nowBranch.nowCommit.allFiles.Count + " : " + modifiedFiles.Count);
                for (int i = 0; i < serverRepository.nowBranch.nowCommit.allFiles.Count; i++)
                {
                    for(int j = 0; j< modifiedFiles.Count; j++)
                    {
                        //Debug.Log(serverRepository.nowBranch.nowCommit.allFiles[i].Key + " : " +  modifiedFiles[j].Key);
                        if (serverRepository.nowBranch.nowCommit.allFiles[i].Key == modifiedFiles[j].Key)
                        {
                            //Debug.Log("Conflicted: " + modifiedFiles[j].Key);
                            confilctFiles.Add(modifiedFiles[j].Key);
                        }
                    }

                }
                if( confilctFiles.Count > 0)
                {
                    conflicted = true;
                    GameSystemManager.GetSystem<PanelManager>().AddSubPanel(conflictSystem);
                    conflictSystem.OpenConflict(confilctFiles, modifiedFiles, serverRepository.nowBranch.nowCommit.allFiles,false,"");
                    conflictSystem.gameObject.SetActive(true);
                }
                return true;
            }
        }
        return false;
    }

    public void Push()
    {
        if (remoteObjects != null)
        {
            Destroy(remoteObjects);
        }
        remoteObjects = Instantiate(localObjects, localObjects.transform.parent);
        remoteObjects.GetComponent<RectTransform>().localPosition = new Vector3(0, 174, 0);
        remoteObjects.transform.GetChild(0).GetComponent<Image>().color = Color.white;
        hasPush = true;
        sync = true;
    }

    

    public void trackFile(string fileName, string Content)
    {
        fileSystem.trackFile(fileName);
        modifiedFiles.Add( new KeyValuePair<string,string>(fileName, Content));
    }
    public void untrackFile(string fileName)
    {
        fileSystem.untrackFile(fileName);
        modifiedFiles.Remove( modifiedFiles.Find( x=> x.Key == fileName  )  );
    }

    public void PanelInput()
    {
        if ((Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)))
        {
            if( gitInputField.GetComponent<InputField>().text == "!wq" )
            {
                Commit(fileInputField.GetComponent<InputField>().text);
                gitFileObject.SetActive(false);
                GameSystemManager.GetSystem<PanelManager>().ReturnTopPanel();
            }
            //Commit(gitInputField.);
        }
    }

    public bool cloneRepository(string remote)
    {
        if (remotes.Contains(remote))
        {
            Destroy(localObjects);
            localObjects = Instantiate(remoteObjects, remoteObjects.transform.parent);
            localObjects.GetComponent<RectTransform>().localPosition = new Vector3(0, -200, 0);
            cloned = true;
            localRepository = serverRepository.clone();
            mainFlag = localObjects.transform.GetChild(0).gameObject;
            //Debug.Log(localRepository.commitCounts());
            nowCommit = localObjects.transform.GetChild(localRepository.commitCounts()).gameObject;
            sync = true;
            hasPush = false;
            fileSystem.NewFile("index","<h1>Hello World!</h1>");
            fileSystem.NewFile("page1", "<h2>page1</h2>");

            if (modifiedFiles == null)
            {
                modifiedFiles = new List<KeyValuePair<string, string>>();
            }
            mainFlag.GetComponent<Image>().color = Color.red;
            headFlag = mainFlag;
            flagObjects.Add(mainFlag);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool checkout(string name)
    {
        GameObject switchFlag = flagObjects.Find(x => x.name == name + "Flag");
        if(switchFlag == null)
        {
            return false;
        }
        switchFlag.GetComponent<Image>().color = Color.red;
        localRepository.switchBranch(name);
        headFlag.GetComponent<Image>().color = Color.white;
        headFlag = switchFlag;
        nowCommit = commitObjects.Find(x => x.name == localRepository.nowBranch.nowCommit.name + "Commit");
        return true;
        
    }

    public bool createBranch(string name)
    {
        if (localRepository.hasBranch(name))
            return false;
    
        localRepository.CreateBranch(name);
        GameObject newFlag;
        newFlag = Instantiate(headFlag, headFlag.transform.parent);
        newFlag.GetComponent<Image>().color = Color.white;
        newFlag.GetComponentInChildren<Text>().text = name;
        newFlag.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(80 - name.Length * 6, -70, 0);
        newFlag.GetComponent<RectTransform>().position = new Vector3(headFlag.GetComponent<RectTransform>().position.x - 200, headFlag.GetComponent<RectTransform>().position.y, headFlag.GetComponent<RectTransform>().position.z);
        newFlag.name = name + "Flag";
        flagObjects.Add(newFlag);
        return true;
    }

    public bool deleteBranch(string name)
    {
        if (!localRepository.hasBranch(name))
            return false;

        GameObject deleteFlag = flagObjects.Find(x => x.name == name + "Flag");
        flagObjects.Remove(deleteFlag);
        Destroy(deleteFlag);
        localRepository.deleteBranch(name);

        return true;
    }
    public void SolvedConflict(bool branch,string name)
    {
        conflicted = false;
        if (branch)
        {
            Commit("Merge branch " + name);
            nowCommit.transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            Commit(serverRepository.nowBranch.nowCommit.name);
            fileSystem.NewFile("page2", "");
            trackFile("page2", "init");
        }
    }
}

