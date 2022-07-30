using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneState : SceneState {


    string load_scene;
    string load_state;

    public LoadSceneState(string state,string scene):base("LoadState","LoadScene"){
        load_scene = scene;
        load_state = state;
    }

    // 讀取畫面圖片淡出
    public void LoadFadeOut(object obj)
    {
        int fade_index = (int)obj;
        if(fade_index == 0 && GameSystemManager.GetSystem<ScreenEffect>().count == 1)
            GameSystemManager.GetSystem<ScreenEffect>().FadeOut(1.5f, 0);
        else
        {
            if (!GameSystemManager.GetSystem<ScreenEffect>().fading)
                GameSystemManager.GetSystem<ScreenEffect>().FadeOut(1.5f, 2);
            GameSystemManager.GetSystem<TimerManager>().Add(new Timer(1.5f, LoadFadeOut, 0));
        }

    }

    public override void StateExit()
    {
        if (GameSystemManager.GetSystem<SceneStateManager>().scene_now.switchfinished)
        {
            GameSystemManager.GetSystem<ScreenEffect>().FadeOut(2, 2);
            GameSystemManager.GetSystem<TimerManager>().Add(new Timer(1.5f, LoadFadeOut,2));
        }
        else
        {
            GameSystemManager.GetSystem<TimerManager>().Add(new Timer(0.25f, LoadFadeOut, 2));
        }



    }


    public void LoadTargetScene(object obj)
    {
        if(load_state == "MainSceneState")
            GameSystemManager.GetSystem<SceneStateManager>().LoadSceneState(new MainSceneState(load_state, load_scene));
        else if(load_state == "TitleSceneState")
            GameSystemManager.GetSystem<SceneStateManager>().LoadSceneState(new TitleSceneState(load_state, load_scene));
    }


    public override void StateStart()
    {
        Debug.Log("State -  " + scene_name + " Start");
        GameSystemManager.GetSystem<TimerManager>().Add(new Timer(1.5f, LoadTargetScene));
    }

    public override void StateUpdate()
    {
        // Fading in Loading background
        if (GameSystemManager.GetSystem<ScreenEffect>().screen_covered)
        {
            if (!GameSystemManager.GetSystem<ScreenEffect>().fading && GameSystemManager.GetSystem<ScreenEffect>().count == 1)
            {
                GameSystemManager.GetSystem<ScreenEffect>().FadeIn(1.5f, GameSystemManager.GetSystem<ScreenEffect>().back[0]);
            }
        }
    }



}
