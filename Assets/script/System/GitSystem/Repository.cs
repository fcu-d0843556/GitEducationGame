using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repository
{
    public List<Branch> branches { private set; get; }
    public Branch nowBranch { private set; get; }
    public int branchCounts { get { return branches.Count; } }

    public Repository()
    {
        Branch main = new Branch("master");
        branches = new List<Branch>();
        branches.Add(main);
        nowBranch = main;
        
    }

    public void Commit(Commit commit)
    {
        if ( nowBranch != null )
        {
            nowBranch.updateCommit(commit);
        }
    }
    public void CreateBranch(string branchName)
    {
        Branch branch = new Branch(branchName);
        branch.setCommit(nowBranch.nowCommit);
        branches.Add( branch );
    }

    public bool switchBranch(string branchName)
    {
        if  ( branches.Exists( x => x.branchName == branchName )   )
        {
            nowBranch = branches.Find(x => x.branchName == branchName);
            return true;
        }
        return false;
    }

    public Repository clone()
    {
        Repository cloned = new Repository();
        for(int i = 0; i < branches.Count; i++)
        {
            cloned.branches[i] = branches[i].clone();
        }

        return this;
    }

    public int commitCounts()
    {
        return nowBranch.commitCounts;
    }

    public bool hasBranch(string branchName)
    {
        if (branches.Exists(x => x.branchName == branchName))
        {
            return true;
        }
        return false;
    }

    public bool deleteBranch(string branchName)
    {
        if (hasBranch(branchName))
        {
            branches.Remove(branches.Find(x => x.branchName == branchName));
            return true;
        }
        return false;
    }


}
