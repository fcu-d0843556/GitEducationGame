using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityUpdate : MonoBehaviour {

    /*public void AddCoroutine(Func<bool> method, YieldInstruction wait)
    {
        StartCoroutine(RunCoroutine(method,wait));
    }*/


    void Update() {
        GameSystemManager.UpdateSystem();
    }
    /*public IEnumerator RunCoroutine(Func<bool> method, YieldInstruction wait)
    {
        bool finished = false;
        while (!finished)
        {
            yield return wait;
        }
        finished = method();
    }*/

}
