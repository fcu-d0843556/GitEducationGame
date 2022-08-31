using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commit
{
    public string name {  private set;  get; }
    public string description { private set; get; }
    public string id { private set; get; }
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
        id = randomString(5);
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

    static string chars = "0123456789abcdefghijklmnopqrstuvwxyz0123456789";
    public static string randomString(int length)
    {
        int maxIndex = chars.Length-1;
        string commitId = "";
        for(int i = 0; i< length; i++)
        {
            commitId += chars[Random.Range(0, maxIndex)];
        }
        return commitId;
    }
}
