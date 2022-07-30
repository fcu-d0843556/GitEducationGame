using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenEffect : MonoBehaviour {

    [SerializeField]
    RawImage white_back;
    [SerializeField]
    RawImage black_back;
    [SerializeField]
    RawImage fade_image;
    RawImage _temp_fade_image;
    [SerializeField]
    Canvas canvas;


    // Scene讀取進度 介於0到1
    float load_progress = 0;
    public CanvasGroup tips_back;
    public Texture2D[] back;
    public Texture2D[] load_gif;
    private int nowFram = 0;
    private int gif_fram;


    public int count { get; private set; }
    public float current_time { get; private set; }
    bool fade_out = false;
    public bool fading = false;
    public bool screen_covered = false;
    bool gameover = false;

    GUIStyle loadingGuiStyle = new GUIStyle();

    private void Start()
    {
        _temp_fade_image = fade_image;
        canvas.enabled = false;
        loadingGuiStyle.fontStyle = new FontStyle();
        loadingGuiStyle.fontSize = 24;
        loadingGuiStyle.font = new Font("JF-Dot-Ayu 18");
        loadingGuiStyle.normal.textColor = Color.white;
    }
    // Update is called once per frame
    void Update()
    {
        if (current_time > 0)
        {
            canvas.enabled = true;
            if (fade_out) // 淡出
            {
                fade_image.color = new Color(fade_image.color.r, fade_image.color.g, fade_image.color.b, fade_image.color.a - (fade_image.color.a) / current_time * Time.deltaTime);
                if (fade_image == _temp_fade_image)
                    tips_back.alpha = fade_image.color.a;
            }
            else // 淡入
            {
                fade_image.color = new Color(fade_image.color.r, fade_image.color.g, fade_image.color.b, fade_image.color.a + (1 - fade_image.color.a) / current_time * Time.deltaTime);
                if (fade_image == _temp_fade_image)
                    tips_back.alpha = fade_image.color.a;
            }
            if (gameover)
            {
                Color original = fade_image.transform.GetChild(0).GetComponent<Text>().color;
                fade_image.transform.GetChild(0).GetComponent<Text>().color = new Color(original.r, original.g, original.b,fade_image.color.a*0.8f);
            }
            current_time -= Time.deltaTime;
            if (current_time <= 0)
            {
                fade_image.color = new Color(fade_image.color.r, fade_image.color.g, fade_image.color.b, fade_out ? 0 : 1);
                if (fade_image == _temp_fade_image)
                    tips_back.alpha = fade_image.color.a;
                fading = false;
                screen_covered = true;
                if (fade_out)
                {
                    screen_covered = false;
                    count--;
                    if (count == 0)
                        canvas.enabled = false;


                }
                else
                    count++;
                current_time = 0;

                if (gameover)
                {
                    Color original = fade_image.transform.GetChild(0).GetComponent<Text>().color;
                    fade_image.transform.GetChild(0).GetComponent<Text>().color = new Color(original.r, original.g, original.b,0);

                    gameover = false;
                }
            }
        }

       if (GameSystemManager.GetSystem<SceneStateManager>().loading)
        {
            load_progress = (GameSystemManager.GetSystem<SceneStateManager>().progress);

        }

    }

    private void OnGUI()
    {
        if (GameSystemManager.GetSystem<ScreenEffect>().screen_covered)
            DrawAnimation();
    }

    public void FadeInBlack(float time)
    {
        
        fade_out = false;
        fading = true;
        current_time = time;
        fade_image = black_back;
        Debug.Log("FadeBlack");
    }

    public void FadeInGameOver(float time)
    {

        fade_out = false;
        fading = true;
        current_time = time;
        fade_image = black_back;
       // Debug.Log("FadeBlack");
        gameover = true;
    }

    public void FadeInWhite(float time)
    {
        fade_out = false;
        fading = true;
        current_time = time;
        fade_image = white_back;
    }
    public void FadeIn(float time,Texture texture)
    {
        fade_out = false;
        fading = true;
        current_time = time;
        fade_image = _temp_fade_image;
        fade_image.texture = texture;

    }
    public void FadeOut(float time,int index)
    {
        current_time = time;
        fade_out = true;
        fading = true;

        if (index == 0)
        {
            fade_image = black_back;
        }
        else if (index == 1)
        {
            fade_image = white_back;
        }
        else
            fade_image = _temp_fade_image;
    }

    float framesPerSecond = 30.0f;
    string load_message = "";
    void DrawAnimation()
    {

        //GUI.DrawTexture(new Rect(1700, 710, 40, 60), tex[nowFram]);
        //在这里显示读取的进度。
        nowFram = (int)(Time.time * framesPerSecond);

        nowFram = nowFram % 7;
        if ( nowFram == 6)
        {
            load_message += ".";
        }
        if (load_message == "...")
        {
            load_message = "";
        }
        
        GUI.DrawTexture(new Rect(0.925f * Screen.width, 0, 80, 80), load_gif[nowFram]);
        GUI.Label(new Rect(0.45f * Screen.width, 0.4f * Screen.height, 600, 300), "Loading" + load_message, loadingGuiStyle);
        
        //GUI.Label(new Rect(0.93f * Screen.width, 0.9f * Screen.height, 600, 100), load_progress.ToString());
    }




}
