using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level5 : Level
{

    private void Start()
    {

        setUp();

        

        gitSystem.addRemote("server"); 
        gitSystem.serverRepository = new Repository();

        Commit firstCommit = new Commit("init commit", "");
        firstCommit.addModifiedFile(new KeyValuePair<string, string>("index", "init"));
        gitSystem.serverRepository.Commit(firstCommit);

        Commit secondCommit = new Commit("Add ignore", "");
        secondCommit.addModifiedFile(new KeyValuePair<string, string>("page1", "init"));
        gitSystem.serverRepository.Commit(secondCommit);

        Commit thirdCommit = new Commit("front update", "");
        thirdCommit.addModifiedFile(new KeyValuePair<string, string>("page1", "HelloWorld !\nsomeone changed"));
        gitSystem.serverRepository.Commit(thirdCommit);

        // gitSystem.buildRepository();
        // setUp();
        // gitSystem.addRemote("server");
        // gitSystem.serverRepository = new Repository();
        // Debug.Log( " 1");
        // Commit firstCommit = new Commit("init commit", "");
        // Commit secondCommit = new Commit("Add ignore", "");
        // Commit thirdCommit = new Commit("front update", "");
        // Debug.Log( " 2");

        // gitSystem.serverRepository.Commit(firstCommit);
        // gitSystem.serverRepository.Commit(secondCommit);
        // gitSystem.serverRepository.Commit(thirdCommit);
    }
    // Update is called once per frame
    void Update()
    {
        if ( !targetSystem.targetStatus[0] &&  gitSystem.cloned)
        {
            targetSystem.targetStatus[0] = true;
            targetSystem.AccomplishTarget(0);
        }
        if (!targetSystem.targetStatus[1] && gitSystem.cloned && gitSystem.sync == false)
        {
            targetSystem.targetStatus[1] = true;
            targetSystem.AccomplishTarget(1);
        }
        if (!targetSystem.targetStatus[2] &&  gitSystem.hasPush && gitSystem.cloned && gitSystem.sync == true)
        {
            targetSystem.targetStatus[2] = true;
            targetSystem.AccomplishTarget(2);
        }
        updateTarget();
        levelCostCount();
    }
}
