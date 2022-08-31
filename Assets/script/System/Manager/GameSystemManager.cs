using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameSystemManager  {

    private static GameSystemManager _instance = null;
    public static GameSystemManager Instance // 實體
    {
        get
        {
            // 產生唯一系統管理器
            if (_instance == null)
            {
                _instance = new GameSystemManager();
                AddSystem<AudioManager>(); // 音效管理器
                AddSystem<UnityUpdate>(); // 定期Update script
                AddSystem<SceneStateManager>(SceneStateManager.Instance); // 場景管理器
                AddSystem<PanelManager>(); // 介面管理器
                AddUpdate(GetSystem<SceneStateManager>().UpdateScene); // add Update scene function
                AddSystem<TimerManager>(TimerManager.Instance); // 計時管理器
                AddUpdate(GetSystem<TimerManager>().Update); // add 計時 update function
                
            }

            return _instance;
        }
    }

    private GameObject _root;
    private List<GameObject> system_objects;
    private Dictionary<Type, MonoBehaviour> _monoBehaviours; // 繼承Mono
    private Dictionary<Type, System.Object> _objects; // 非繼承Mono
    private List<Action> update_list;

    private GameSystemManager()
    {

        _root = new GameObject("[GameSystemManager]");// 掛載物件 繼承MonoBehavior者需要掛載於GameObject上 
        GameObject.DontDestroyOnLoad(_root); // 換場景時不刪除root 避免Mono類system manager被刪掉
        _monoBehaviours = new Dictionary<Type, MonoBehaviour>();
        _objects = new Dictionary<Type, System.Object>();
        update_list = new List<Action>();
        system_objects = new List<GameObject>();
    }


    public static void AddSystem<T>(System.Object entity) where T : class
    {
        if (HasSystem<T>())
        {
            Debug.LogException(new Exception(string.Format("[GameSystemManager] - GameSystem \"{0}\" has already existed!", typeof(T).Name)));
        }
        else
        {
            //Debug.LogFormat(string.Format("[GameSystemManager] - GameSystem \"{0}\" has set up.", typeof(T).Name));
            Instance._objects.Add(typeof(T), entity);
        }
    }

    // 額外添加至root的MonoBehaviour
    public static void AddSystem<T>() where T : MonoBehaviour
    {
        if (HasSystem<T>())
        {
            Debug.LogException(new Exception(string.Format("[GameSystemManager] - GameSystem \"{0}\" has already existed!", typeof(T).Name)));
        }
        else
        {
            //Debug.LogFormat(string.Format("[GameSystemManager] - GameSystem \"{0}\" has set up.", typeof(T).Name));
            Instance._monoBehaviours.Add(typeof(T), Instance._root.AddComponent<T>());
        }
    }

    // 已產生之MonoBehaviour
    public static void AddSystem<T>(GameObject Object) where T : MonoBehaviour
    {
        if (HasSystem<T>())
        {
            Debug.LogException(new Exception(string.Format("[GameSystemManager] - GameSystem \"{0}\" has already existed!", typeof(T).Name)));
        }
        else
        {
            //Debug.LogFormat(string.Format(Object.name + " - "+"[GameSystemManager] - GameSystem \"{0}\" has set up.", typeof(T).Name));
            Instance._monoBehaviours.Add(typeof(T), Object.GetComponent<T>() );
            GameObject.DontDestroyOnLoad(Object);
            Instance.system_objects.Add(Object);
        }
    }



    public static T GetSystem<T>() where T : class
    {
        if (HasSystem<T>())
        {
            if (IsInheritMonoBehaviour<T>())
            {
                if (Instance._root.GetComponent<T>() != null)
                {
                    return Instance._root.GetComponent<T>();
                }
                else
                {
                    for(int i = 0; i < Instance.system_objects.Count; i++)
                    {
                        if (Instance.system_objects[i].GetComponent<T>() != null)
                        {
                            return Instance.system_objects[i].GetComponent<T>();

                        }
                    }
                }
                
            }
            else
            {
                return Instance._objects[typeof(T)] as T;
            }
        }

        //Debug.LogException(new Exception(string.Format("[GameSystemManager] - GameSystemManager of {0} doesn't exist!", typeof(T).Name)));
        //Debug.Log("nono");
        return null;
    }


    public static bool HasSystem<T>() where T : class
    {
        if (IsInheritMonoBehaviour<T>())
        {
            return Instance._monoBehaviours.ContainsKey(typeof(T));
        }
        else
        {
            return Instance._objects.ContainsKey(typeof(T));
        }
    }


    private static bool IsInheritMonoBehaviour<T>() where T : class
    {
        return typeof(T).IsSubclassOf(typeof(MonoBehaviour));
    }

    public static void AddUpdate(Action method)
    {
        Instance.update_list.Add(method);
    }

    public static void UpdateSystem()
    {

        for (int i = 0; i < Instance.update_list.Count; i++)
        {
            Instance.update_list[i]();
        }
    }
}
