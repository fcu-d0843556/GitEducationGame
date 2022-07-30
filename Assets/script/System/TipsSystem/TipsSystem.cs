using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsSystem : MonoBehaviour
{
    [SerializeField]
    Button leftButton;
    [SerializeField]
    Button rightButton;
    [SerializeField]
    Text pageText;

    [SerializeField]
    int count;
    int nowPage = 0;

    [SerializeField]
    List<string> dialouge;

    [SerializeField]
    Text dialougeText;
    [SerializeField]
    List<Sprite> sprites;
    [SerializeField]
    float dialougeWaitSeconds;

    [SerializeField]
    Image image;

    string dialougeString;
    int dialougeStringIndex;
    bool waitEnd;

    Level nowLevel;

    public bool hasRead { private set; get; } = false;
    bool startDialouge = false;
    public bool closeWhenEndPage = true;
    // Start is called before the first frame update
    void Start()
    {

        leftButton.onClick.AddListener( delegate
        {
            PrePage();
         }
        );
        rightButton.onClick.AddListener(delegate
        {
            NextPage();
        }
        );
        pageText.text = (nowPage+1) + "/" +  count;
        dialougeString = dialouge[nowPage];
        if (sprites[nowPage])
        {
            image.sprite = sprites[nowPage];
        }

        waitEnd = true;
        GameObject tipsObject = gameObject;
        dialougeText.text = "";
        image.enabled = false;
        GameSystemManager.GetSystem<TimerManager>().Add(new Timer(4, TipsStart, null));
    }

    public void TipsStart(object obj)
    {
        startDialouge = true;
        image.enabled = true;
        // achieve 10
        if (Level.restartCount == 10)
        {
            StartCoroutine(GameSystemManager.GetSystem<AchievementManager>().logAchievement(10));
        }
    }

    private void OnEnable()
    {
        if (!nowLevel)
        {
            nowLevel = GameObject.Find("MainScreenObject").GetComponent<Level>();
        }
        if (nowLevel.levelStarted)
        {
            GameSystemManager.GetSystem<StudentEventManager>().logStudentEvent("tips_opened", "{}");
        }
    }

    private void OnDisable()
    {
        if (nowLevel.levelStarted)
        {
            GameSystemManager.GetSystem<StudentEventManager>().logStudentEvent("tips_closed", "{}");
        }
        // achieve  9
        if (nowPage == 0)
        {
            GameSystemManager.GetSystem<AchievementManager>().logAchievementByManager(9);
        }
        nowPage = 0;
        pageText.text = (nowPage + 1) + "/" + count;
        image.color = new Color(1, 1, 1, 0);
        dialougeString = dialouge[nowPage];
        if (sprites[nowPage])
        {
            image.sprite = sprites[nowPage];
        }
        dialougeStringIndex = 0;
        waitEnd = true;
    }


    private void Update()
    {
        if (startDialouge)
        {
            dialougeText.text = dialougeString.Substring(0, dialougeStringIndex);
            if (dialougeStringIndex < dialougeString.Length && waitEnd == true)
            {
                StartCoroutine(WaitForDialouge());
            }
            if (sprites[nowPage])
            {
                image.color = new Color(1, 1, 1, Mathf.Lerp(image.color.a, 1, 0.04f));
            }
            else
            {
                image.color = new Color(0, 0, 0, 0);
            }
        }
    }

    void PrePage()
    {
        if ( nowPage !=0 )
        {
            nowPage--;
            pageText.text = (nowPage + 1) + "/" + count;
            dialougeString = dialouge[nowPage];
            if (sprites[nowPage] )
            {
                if (sprites[nowPage + 1] && (sprites[nowPage].texture != sprites[nowPage + 1].texture))
                {
                    image.color = new Color(1, 1, 1, 0);
                }
                image.sprite = sprites[nowPage];
            }
            dialougeStringIndex = 0;
            waitEnd = true;
        }

    }

    void NextPage()
    {
        if (nowPage < count - 1 )
        {
            nowPage++;
            pageText.text = (nowPage + 1) + "/" + count;
            dialougeString = dialouge[nowPage];
            if (sprites[nowPage])
            {
                if (sprites[nowPage - 1] && (sprites[nowPage].texture != sprites[nowPage - 1].texture))
                {
                    image.color = new Color(1, 1, 1, 0);
                }
                image.sprite = sprites[nowPage];
            }
            dialougeStringIndex = 0;
            waitEnd = true;
            if(nowPage == count - 1)
            {
                hasRead = true;
            }
        }
        else if(closeWhenEndPage)
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator WaitForDialouge()
    {
        waitEnd = false;
        dialougeStringIndex++;
        yield return new WaitForSeconds(dialougeWaitSeconds);
        waitEnd = true;
    }
}
