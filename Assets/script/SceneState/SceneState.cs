using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public abstract class SceneState  {

    public string state_name { get { return _state_name; } }
    public string scene_name { get { return _scene_name; } }

    private string _state_name;
    private string _scene_name;
    AsyncOperation async;
    public float progress;
    public bool loadfinished;
    public bool switchfinished { get { return async.isDone; } }


    public SceneState(string state_name,string scene_name)
    {
        _state_name = state_name;
        _scene_name = scene_name;
        loadfinished = false;
        Application.backgroundLoadingPriority = ThreadPriority.BelowNormal;// 後台加載的優先順序
        
        
    }

    public abstract void StateStart(); // 場景轉換成功
    public abstract void StateUpdate();
    public abstract void StateExit();

    public IEnumerator LoadScene()
    {
        if (async == null)
        {

            async = SceneManager.LoadSceneAsync(scene_name);// 後台讀取
            async.allowSceneActivation = false;// 加載完不立即切換
        }
        progress = async.progress;
        //Debug.Log("Loading..." + progress);
        if (async.progress == 0.9f)
        {
            loadfinished = true;
        }

        yield return new WaitForEndOfFrame();

    }

    public void ChangeScene()
    {
        if (loadfinished)
        {
            async.allowSceneActivation = true;

        }
    }


}
