using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardFloating : MonoBehaviour, IPointerEnterHandler , IPointerExitHandler
{

    public Vector3 offSet;
    public Vector3 oriW;
    public Vector3 originalPosition;
    public float dist;
    public float max;

    public Text title;
    public Vector3 originalPositionText;

    bool covered = false;
    bool reading = false;
    int readTimes = 0;
    static int allReadTimes = 0;

    Image img;
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        offSet = new Vector3( (dist - 1) / 2 * oriW.x,  (dist - 1)  * oriW.y);
        gameObject.transform.localPosition = (originalPosition  + offSet);
        gameObject.transform.localScale = new Vector3(dist, dist, 0);
        //title.GetComponent<RectTransform>().sizeDelta = new Vector2(320 * dist, 40 * dist);
        //title.fontSize = (int)  (18 * dist);
        //title.gameObject.transform.localScale = new Vector3( 1 / dist, 1 / dist, 1);
        if (covered)
        {
            dist = Mathf.Lerp(dist, max, 0.1f);
            if ( (max - dist) < 0.01f)
            {
                reading = true;
            }
        }
        else
        {
            dist = Mathf.Lerp(dist, 1, 0.1f);
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        covered = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        covered = false;
        if (reading)
        {
            Level.levelScene nowLevel = GameObject.Find("MainScreenObject").GetComponent<Level>().nowLevel;
            GameSystemManager.GetSystem<StudentEventManager>().logStudentEvent("card_read", "{level:'" + nowLevel + "'" +
            ", card:'" + title.text + "'}");
            readTimes++;
            allReadTimes++;
            // achieve 5
            if (readTimes == 5)
            {
                StartCoroutine(GameSystemManager.GetSystem<AchievementManager>().logAchievement(5));
            }
            // achieve 6
            if (allReadTimes == 25)
            {
                StartCoroutine(GameSystemManager.GetSystem<AchievementManager>().logAchievement(6));
            }
            reading = false;
        }
    }
}
