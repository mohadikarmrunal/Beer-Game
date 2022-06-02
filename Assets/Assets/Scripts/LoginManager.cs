using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class LoginManager : MonoBehaviour
    {
        public InputField userNameInput;
        public InputField passwordInput;

        public void Login()
        {
            StartCoroutine(LoginRequest());
        }

        private IEnumerator LoginRequest()
        {
            if (userNameInput == null || passwordInput == null) yield break;
            String userName = userNameInput.text;
            String password = passwordInput.text;

            WWWForm form = new WWWForm();
            form.AddField("username", userName);
            form.AddField("password", password);
            UnityWebRequest loginRequest = UnityWebRequest.Post(APIConstants.AuthTokenAddress, form);


            yield return loginRequest.SendWebRequest();

            if (loginRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Login failed: " + loginRequest.error + " " + loginRequest.downloadHandler.error);
            }
            else
            {
                Debug.Log("Login successful: " + loginRequest.downloadHandler.text);
                string json = loginRequest.downloadHandler.text;
                User currentUser = JsonUtility.FromJson<User>(json);
                PlayerPrefs.SetString("username", currentUser.username);
                PlayerPrefs.SetString("name", currentUser.name);
                PlayerPrefs.SetString("authToken", currentUser.authToken);
                PlayerPrefs.SetString("userId", currentUser.id);
                PlayerPrefs.SetString("userEmail", currentUser.email);
                PlayerPrefs.Save();

                SceneManager.LoadScene("LobbyScene");
            }
        }
    }
}