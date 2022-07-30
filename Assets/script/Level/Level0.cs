using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level0 : Level
{
    [SerializeField]
    TipsSystem tipsSystem;
    private void Start()
    {
        setUp();
        tipsSystem.closeWhenEndPage = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (!targetSystem.targetStatus[0] && tipsSystem.hasRead)
        {
            targetSystem.targetStatus[0] = true;
            targetSystem.AccomplishTarget(0);
        }
        updateTarget();
        levelCostCount();
    }
}
