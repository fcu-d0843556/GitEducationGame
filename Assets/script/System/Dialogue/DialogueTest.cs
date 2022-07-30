using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTest : MonoBehaviour {
    [SerializeField]
    Canvas dialougeCanvas;
    [SerializeField]
    Text dialougebox;
    AudioSource voice;
    bool dialouge_end = true;
	// Use this for initialization
	void Start () {
        voice = GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {

        }
	}
    public void Dialouge(string message,bool end,AudioClip voice_clip)
    {
        if (message == dialougebox.text)
        {
            return;
        }
        dialougeCanvas.enabled = true;
        dialougebox.text = message;
        voice.clip = voice_clip;
        voice.Play();
        dialouge_end = end;

    }
    public void CloseDialouge()
    {
        dialougeCanvas.enabled = false;
    }
}
