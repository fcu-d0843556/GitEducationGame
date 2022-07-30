using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour {
    List<Panel> _panels;


    /*
     主動開Panel者 _panels需為 0個
     其餘由當前Panel自行開 sub跌加
     母panel 及 subPanel中只有最前者有input
         
         */

    public bool empty { get; private set; }

    private void Start()
    {
        if(_panels == null)
            _panels = new List<Panel>();
    }

    void Update() {
        //Debug.Log("Panel Count : " + _panels.Count);
        if (_panels.Count > 0)
        {
            //Debug.Log("Now Panel : " + _panels[_panels.Count - 1]);
            _panels[_panels.Count - 1].PanelInput();
            empty = false;
        }
        else
        {
            empty = true;
        }
    }
    
    public void AddSubPanel(Panel subpanel)
    {
        if (_panels == null)
            _panels = new List<Panel>();
        _panels.Add(subpanel);
    }

    public void ReturnTopPanel()
    {
        if(_panels.Count> 0)
            _panels.Remove(_panels[_panels.Count - 1]);
    }  


}
