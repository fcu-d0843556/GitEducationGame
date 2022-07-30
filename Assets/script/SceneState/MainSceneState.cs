using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneState : SceneState {
    
    bool initialization = false;

    public MainSceneState(string state,string scene):base(state,scene){
        
    }

    public override void StateExit()
    {
        
    }

    public override void StateStart()
    {
        //Debug.Log("State -  " + scene_name + " Start");
    }

    public override void StateUpdate()
    {
        if (!initialization && switchfinished )
        {
        }
    }


}
