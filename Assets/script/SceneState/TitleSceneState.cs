using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneState : SceneState {
    
    bool initialization = false;

    public TitleSceneState(string state,string scene):base(state,scene){
        
    }

    public override void StateExit()
    {
        
    }

    public override void StateStart()
    {
        //Debug.Log("State -  " + scene_name + " Start");
        Cursor.visible = true;
    }

    public override void StateUpdate()
    {
        if (!initialization && switchfinished )
        {
            
        }

    }


}
