using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager:MonoBehaviour{

    static private AudioSource player = null;
    static private AudioSource player_se = null;
    [SerializeField]
    private List<GameObject> sounds;
    float fade_time;
    bool fade_in;

    public bool IsFade { get { return fade_time > 0; } }
    [SerializeField]
    private float volume_bgm = 1;
    [SerializeField]
    private float volume_se = 1;

    private void Awake()
    {
        if (!player)
        {
            player = gameObject.AddComponent<AudioSource>();
            player.loop = true;
            player.playOnAwake = false;
            player.maxDistance = 10000;
        }
        if (!player_se)
        {
            player_se = gameObject.AddComponent<AudioSource>();
            player_se.loop = false;
            player_se.playOnAwake = false;
            player_se.maxDistance = 10000;
        }
        sounds = new List<GameObject>();
    }
    
    public void FadeInBgm(AudioClip clip,float fade_time)
    {
        player.volume = 0;
        fade_in = true;
        player.clip = clip;
        player.Play();
        this.fade_time = fade_time;
    }

    public void AddSoundSource(GameObject root,string name) // 增加新audio source
    {
        GameObject audio_source = new GameObject(name);
        audio_source.AddComponent<AudioSource>();
        audio_source.transform.SetParent(root.transform);
        audio_source.transform.localPosition = new Vector3(0, 0, 0);
        sounds.Add(audio_source);
        sounds[sounds.Count - 1].GetComponent<AudioSource>().maxDistance = 100;
        sounds[sounds.Count - 1].GetComponent<AudioSource>().playOnAwake = false;
        sounds[sounds.Count - 1].GetComponent<AudioSource>().spatialBlend = 0.7f;

    }

    /*public void AddSoundSource(AudioSource root, string name) // 使用者預先設定
    {
        sounds.Add(root);
        sounds[sounds.Count - 1].maxDistance = 50;
        sounds[sounds.Count - 1].name = name;
        sounds[sounds.Count - 1].playOnAwake = false;
    }*/

    public void Play3DSound(string name,AudioClip clip,bool loop) // 播放3D聲音 具有獨立AudioSource組件才能使用
    {

        int i;
        for (i=0; i < sounds.Count; i++)
        {
            if (sounds[i].name == name)
                break;
        }
        if (i == sounds.Count)
        {
            return;
        }
        if(clip !=null)
            sounds[i].GetComponent<AudioSource>().clip = clip;
        sounds[i].GetComponent<AudioSource>().loop = loop;
        sounds[i].GetComponent<AudioSource>().Stop();
        //sounds[i].GetComponent<AudioSource>().volume = volume_se;
        StartCoroutine(FadeInSound(sounds[i].GetComponent<AudioSource>(), 1));
        //sounds[i].GetComponent<AudioSource>().Play();

    }

    public void Stop3DSound(string name)
    {
        int i;
        for (i = 0; i < sounds.Count; i++)
        {
            if (sounds[i].name == name)
                break;
        }
        if (i == sounds.Count)
        {
            return;
        }
        StartCoroutine(FadeOutSound(sounds[i].GetComponent<AudioSource>(), 1));
    }

    IEnumerator FadeOutSound(AudioSource sound,float time)
    {
        while(time > 0 && sound.volume > 0 && sound.isPlaying)
        {
            sound.volume -= sound.volume /time *Time.deltaTime;
            time -= Time.deltaTime;
            yield return null;
        }
        sound.volume = 0;
        sound.Stop();
    }

    IEnumerator FadeInSound(AudioSource sound, float time)
    {
        sound.Play();
        while (time > 0)
        {
            float value = 1 / time * Time.deltaTime;
            if(value > 0)
                sound.volume += value;
            time -= Time.deltaTime;
            if (time <= 0)
                sound.volume = 1;
            yield return null;
        }
    }

    public void PlayBgm(AudioClip clip)
    {
        player.clip = clip;
        player.Play();
    }

    public void FadeOutBgm(float time)
    {
        fade_in = false;
        fade_time = time;
    }

    public void SetBgmVolume(float volume)
    {
        if (volume > 1)
            volume = 1;
        if (volume < 0)
            volume = 0;
        player.volume = volume;
        volume_bgm = volume;
        fade_in = true;
        fade_time = 0.1f;
    }

    public void SetSeVolume(float volume)
    {
        if (volume > 1)
            volume = 1;
        if (volume < 0)
            volume = 0;
        volume_se = volume;
    }

    private void Update()
    {
        if(fade_time > 0)
        {
            if (!fade_in) // 淡出
            {
                player.volume -= (player.volume / fade_time * Time.deltaTime);
                if (player.volume <= 0)
                    fade_time = 0;
            }
            else // 淡入
            {
                player.volume += ( (volume_bgm - player.volume) / fade_time * Time.deltaTime);
                if (player.volume >= volume_bgm)
                    fade_time = 0;
            }
            fade_time -= Time.deltaTime;
            if(fade_time <= 0)
            {
                player.volume = volume_bgm;
                if (!fade_in)
                {
                    player.Stop();

                }
            }
        }
    }
    // 單次播放聲音 用於處理2D音效 不需要出現於場景中使用 如選單介面音效等
    public void PlayOneShot(AudioClip clip,float volume)
    {
        player_se.PlayOneShot(clip, volume);

    }
    public void PlayOneShot(AudioClip clip)
    {
        player_se.PlayOneShot(clip, volume_se);
        
    }

    private void LateUpdate()
    {
        for(int i = 0; i < sounds.Count; i++)
        {
            if (sounds[i] == null)
                sounds.Remove(sounds[i]);
        }
    }
}
