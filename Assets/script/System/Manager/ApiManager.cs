using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApiManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private List<api> apiList;

    public void addApi(string name, string url)
    {
        apiList.Add(new api(name, url));
    }

    public string getApiUrl(string name)
    {
        string url = apiList.Find(x => x.name == name).url;
        return url;
    }

    [System.Serializable]
    public class api
    {
        public string name;
        public string url;

        public api(string name, string url) { this.name = name; this.url = url; }
    }
}
