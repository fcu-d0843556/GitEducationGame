using System;
using System.Collections.Generic;
using UnityEngine;

public class Branch 
{
    public Commit nowCommit { private set; get; }
    public List<Commit> commits;
    public string branchName;
    public int commitCounts { private set; get; }
    public bool branchStart { private set; get; }

    public Branch(string name)
    {
        branchName = name;
        commitCounts = 0;
        branchStart = false;
        commits = new List<Commit>();
    }

    public void setCommit(Commit commit)
    {
        nowCommit = commit;
        commitCounts = 1;
        branchStart = true;
    }
    public void updateCommit(Commit commit)
    {
        branchStart = false;
        if (nowCommit == null)
        {
            nowCommit = commit;
            commitCounts = 1;
            
        }
        else
        {
            nowCommit.nextCommit = commit;
            commit.lastCommit = nowCommit;
            foreach(KeyValuePair<string, string> file in nowCommit.allFiles)
            {
                commit.allFiles.Add(file);
            }
            nowCommit = nowCommit.nextCommit;
            commitCounts++;
            commits.Add(commit);
        }
        foreach (KeyValuePair<string, string> file in commit.modifiedFiles)
        {
            commit.updateFileInAllFiles(file);
        }
    }

    public Branch clone()
    {
        Branch cloned = new Branch(branchName);
        Commit firstCommit = nowCommit;
        while (firstCommit.lastCommit != null)
        {
            
            firstCommit = firstCommit.lastCommit;
            commitCounts++;
        }
        commitCounts++;
        commitCounts /= 2;
        firstCommit = nowCommit.clone();
        return cloned;
    }
}
