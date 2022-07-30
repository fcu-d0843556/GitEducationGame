using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementAnimation : MonoBehaviour
{

    public bool testSwitch;

    public bool popuping { get; private set; } = false;
    public bool hiding { get; private set; } = false;
    public bool animationPlaying { get; private set; } = false;
    [SerializeField]
    private int speed;
    [SerializeField]
    private float stopTime;
    [SerializeField]
    private float topPos;
    [SerializeField]
    private float bottomPos;
    [SerializeField]
    Image icon;
    [SerializeField]
    Text description;

    List<Sprite> iconQueue = new List<Sprite>();
    List<string> descriptionQueue = new List<string>();

    // Update is called once per frame
    void Update()
    {
        if (testSwitch)
        {
            popup();
            testSwitch = false;
        }
        if (popuping)
        {
            if (transform.localPosition.y >= topPos)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, topPos, transform.localPosition.z);
                popuping = false;
                GameSystemManager.GetSystem<TimerManager>().Add(new Timer(stopTime, hide));
            }
            else
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + Time.deltaTime * speed, transform.localPosition.z);
            }
        }
        if (hiding)
        {
            if (transform.localPosition.y <= bottomPos)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, bottomPos, transform.localPosition.z);
                hiding = false;
                animationPlaying = false;
                iconQueue.RemoveAt(0);
                descriptionQueue.RemoveAt(0);
                if (iconQueue.Count != 0)
                {
                    this.icon.sprite = iconQueue[0];
                    this.description.text = descriptionQueue[0];
                    popup();
                }
            }
            else
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + Time.deltaTime * -speed, transform.localPosition.z);
            }
        }
    }


    public void popup(Sprite icon, string description)
    {
        iconQueue.Add(icon);
        descriptionQueue.Add(description);
        if (!animationPlaying)
        {
            this.icon.sprite = icon;
            this.description.text = description;
            animationPlaying = true;
            popup();
        }
    }
    private void popup()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, bottomPos , transform.localPosition.z);
        popuping = true;
    }

    void hide(object obj)
    {
        hiding = true;
    }
}
