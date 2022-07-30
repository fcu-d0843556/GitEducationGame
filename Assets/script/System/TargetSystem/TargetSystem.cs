using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetSystem : MonoBehaviour
{

    int targetAccomplishCount;
    [SerializeField]
    List<GameObject> targetObject;

    [SerializeField]
    public List<bool> targetStatus;

    [SerializeField]
    List<string> targets;

    // Start is called before the first frame update
    void Start()
    {
        targetAccomplishCount = 0;
        for (int i = 0; i < targets.Count; i++)
        {

            targetObject[i].GetComponentInChildren<Text>().text = targets[i];
            targetObject[i].SetActive(true);
        }
    }

    public void AccomplishTarget(int index)
    {
        targetObject[index].GetComponent<Image>().color = new Color(0.19f, 1, 0.063f, 0.39f);
        Level.levelScene nowLevel = GameObject.Find("MainScreenObject").GetComponent<Level>().nowLevel;
        GameSystemManager.GetSystem<StudentEventManager>().logStudentEvent("target_accomplished", "{level:'" + nowLevel + "'" +
            ", target:'" + (index+1) + "', target_content:'" + targets[index] + "' }");
    }
    public void UndoTarget(int index)
    {
        targetObject[index].GetComponent<Image>().color = new Color(1, 0.2f, 0.063f, 0.66f);
    }
}
