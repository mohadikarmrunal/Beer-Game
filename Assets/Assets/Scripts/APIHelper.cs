using System.Net;
using System.IO;
using Assets.Scripts;
using UnityEngine;

static public class APIHelper
{
    public static User GetUser()
    {
        HttpWebRequest request = (HttpWebRequest) WebRequest.Create("");
        HttpWebResponse response = (HttpWebResponse) request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();
        return JsonUtility.FromJson<User>(json);
    }
}
