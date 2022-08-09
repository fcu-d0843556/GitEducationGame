using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commit
{
    public string name {  private set;  get; }
    public string description { private set; get; }
    public List<KeyValuePair<string,string>> modifiedFiles { private set; get; }
    public List<KeyValuePair<string, string>> allFiles { private set; get; }
    public Commit nextCommit;
    public Commit lastCommit;

    public int branchUsed;

    public Commit(string name, string description)
    {
        this.name = name;
        this.description = description;
        modifiedFiles = new List<KeyValuePair<string, string>>();
        allFiles = new List<KeyValuePair<string, string>>();
        branchUsed = 0;
    }

    public void addModifiedFile(KeyValuePair<string, string> file) 
    {
        modifiedFiles.Add(file);
    }

    public Commit clone()
    {
        Commit cloned = new Commit(name, description);
        modifiedFiles.CopyTo(cloned.modifiedFiles.ToArray());
        allFiles.CopyTo(cloned.allFiles.ToArray());
        return cloned;

    }

    public void updateFileInAllFiles(KeyValuePair<string, string> file)
    {
        int i = 0;
        for (; i < allFiles.Count; i++)
        {
            if (file.Key == allFiles[i].Key)
            {
                allFiles[i] = file;
                break;
            }
        }
        if (i == allFiles.Count)
        {
            allFiles.Add(file);
        }
    }

}
