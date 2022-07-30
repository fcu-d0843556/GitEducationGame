using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStateManager {

    public SceneState scene_pre { get { return _scene_pre; } }
    public SceneState scene_now { get { return _scene_now; } }
    public SceneState scene_next { get { return _scene_next; } }

    private SceneState _scene_pre;
    private SceneState _scene_now;
    private SceneState _scene_next;

    public float progress;
    public bool loading = false;
    bool immediately;

    private static long startLoadTime = 0;



    private static SceneStateManager _instance = null;
    public static SceneStateManager Instance
    {
        get
        {
            // 產生唯一系統管理器
            if (_instance == null)
            {
                _instance = new SceneStateManager();
            }

            return _instance;
        }
    }
    private SceneStateManager() {
        _scene_now = new TitleSceneState("TitleScene", "TitleScene");
        //_scene_now = new MainSceneState("null", null);
    }

    public void LoadSceneState(SceneState state,bool immediately = true)
    {
        if (_scene_next == null)
        {
            _scene_next = state;
            GameSystemManager.GetSystem<AudioManager>().FadeOutBgm(1);
            GameSystemManager.GetSystem<ScreenEffect>().FadeInBlack(1.5f);
            this.immediately = immediately;
        }
    }


    public void ChangeSceneState()
    {
        if (_scene_next != null  && GameSystemManager.GetSystem<ScreenEffect>().screen_covered)
        {

            _scene_next.ChangeScene();
            _scene_pre = _scene_now;
            _scene_now = _scene_next;
            _scene_next = null;
            _scene_now.StateStart();
            progress = 0;
            loading = false;
            if (_scene_pre != null)
                _scene_pre.StateExit();
        }
    }

    public void UpdateScene()
    {
        if (startLoadTime == 0)
            startLoadTime = getCurrentTimeSeconds();
        if (scene_now!=null && scene_now.loadfinished)
            _scene_now.StateUpdate();
        if (_scene_next != null && !_scene_next.loadfinished && getCurrentTimeSeconds() - startLoadTime > 1  ) // 讀取
        {
            _scene_next.LoadScene().MoveNext();
            progress = _scene_next.progress;
            loading = true;
            if (_scene_next.loadfinished)
            {
                //Debug.Log("Loading Finish");
            }
        }
        else
            loading = false;
        if (_scene_next != null && _scene_next.loadfinished)
        {
            if (immediately)
                ChangeSceneState();
        }
    }

    public static long getCurrentTimeSeconds()
    {
        return ConvertDateTimeToInt(System.DateTime.Now) / 1000;

    }

    public static long ConvertDateTimeToInt(System.DateTime time)
    {
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
        long t = (time.Ticks - startTime.Ticks) / 10000;
        return t;
    }

}
