using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2 : Level
{

    private void Start()
    {
        setUp();
    }
    // Update is called once per frame
    void Update()
    {
        if ( !targetSystem.targetStatus[0] && gitSystem.hasRepository()  )
        {
            targetSystem.targetStatus[0] = true;
            targetSystem.AccomplishTarget(0);
        }
        else if ( !gitSystem.hasRepository() )
        {
            targetSystem.targetStatus[0] = false;
            targetSystem.UndoTarget(0);
        }
        updateTarget();
        levelCostCount();
    }
}
