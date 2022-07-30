using System;
using System.Collections.Generic;
using UnityEngine;


public class TimerManager
{
    private List<Timer> _timers;
    private bool _pause = false;
    private Timer _timerCache;

    private static TimerManager _instance = null;
    public static TimerManager Instance
    {
        get
        {
            // 產生唯一系統管理器
            if (_instance == null)
            {
                _instance = new TimerManager();
            }

            return _instance;
        }
    }

    private TimerManager()
    {
        _timers = new List<Timer>();
    }


    public void Add(Timer timer)
    {
        _timers.Add(timer);
    }


    public void Remove(Timer timer)
    {
        _timers.Remove(timer);
    }


    public void Update()
    {

        if (_timers.Count != 0 && !_pause)
        {
            for (int cnt = 0; cnt < _timers.Count; cnt++)
            {
                _timerCache = _timers[cnt];
                _timerCache.Update(Time.deltaTime);

                if (_timerCache.finished)
                {
                    Remove(_timerCache);
                }
            }
        }

    }


    public void Pause()
    {
        _pause = true;
    }


    public void Resume()
    {
        _pause = false;
    }
}
