using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts
{
    public class WaitingRoomResponseStream : MonoBehaviour
    {
        public static IEnumerator LogWaitingRoomResponse()
        {
            bool canStart = false;

            while (!canStart)
            {
                WWWForm form = new WWWForm();
                form.AddField("gameId", PlayerPrefs.GetInt("game.pk"));
                form.AddField("playerId", PlayerPrefs.GetInt("player.pk"));
                form.AddField("periodId", PlayerPrefs.GetInt("period.pk"));
                UnityWebRequest checkGameRequest = UnityWebRequest.Post(APIConstants.WaitingRoomAddress, form);
                checkGameRequest.SetRequestHeader("Authorization", "Token " + PlayerPrefs.GetString("authToken"));

                yield return checkGameRequest.SendWebRequest();

                if (checkGameRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log("Load game data for checking failed: " + checkGameRequest.error + " " +
                              checkGameRequest.downloadHandler.error);
                    yield return new WaitForSeconds(5f);
                }
                else
                {
                    Debug.Log("Check game successful: " + checkGameRequest.downloadHandler.text);
                    string jsonResponse = checkGameRequest.downloadHandler.text;
                    GameResponse gameResponse = JsonUtility.FromJson<GameResponse>(jsonResponse);

                    Game currentGame = gameResponse.game_data;

                    if (!currentGame.can_start)
                    {
                        yield return new WaitForSeconds(2f);
                    }
                    else
                    {
                        canStart = true;
                        Debug.Log("Game can be started, loading scene...");
                        switch (PlayerPrefs.GetInt("player.role"))
                        {
                            case 1:
                                SceneManager.LoadScene("RetailerScene");
                                break;
                            case 2:
                                SceneManager.LoadScene("WarehouseScene");
                                break;
                            case 3:
                                SceneManager.LoadScene("WarehouseScene");
                                break;
                            case 4:
                                SceneManager.LoadScene("BreweryScene");
                                break;
                        }
                    }
                }
            }
        }
    }
}


// Use this code if you ever want to try to get event streams working

// namespace Assets.Scripts
// {
//     public class WaitingRoomResponseStream : MonoBehaviour
//     {
//         public static async Task LogWaitingRoomResponse(int game_pk)
//         {
//             HttpClient client = new HttpClient();
//             client.Timeout = TimeSpan.FromSeconds(25);
//             string url = $"{APIConstants.WaitingRoomAddress}{game_pk}/events/";
//
//             while (true)
//             {
//                 try
//                 {
//                     Debug.Log("Establishing waiting room connection");
//                     using (var streamReader =
//                         new StreamReader(
//                             await client.GetStreamAsync("http://localhost:8000/beergame/waiting_room_112/events/")))
//                     {
//                         while (!streamReader.EndOfStream)
//                         {
//                             var message = await streamReader.ReadLineAsync();
//                             Debug.Log($"Received update: {message}");
//                         }
//                     }
//                 }
//                 catch (Exception e)
//                 {
//                     //Here you can check for 
//                     //specific types of errors before continuing
//                     //Since this is a simple example, i'm always going to retry
//                     Debug.Log($"Error: {e.Message}");
//                     Debug.Log("Retrying in 15 seconds");
//                     await Task.Delay(TimeSpan.FromSeconds(25));
//                 }
//             }
//         }
//     }
// }
//

// Debug.Log("ReadStream called with game id: " + game_pk);
// HttpWebRequest request = (HttpWebRequest)WebRequest.Create(APIConstants.WaitingRoomAddress + game_pk + "/events/");
// HttpWebResponse response = (HttpWebResponse) request.GetResponse();
// Debug.Log("Stream response: " + response.StatusCode);
// Stream responseStream = response.GetResponseStream();
// Debug.Log("Streeeeam " + responseStream);
// Encoding encode = Encoding.GetEncoding(("utf-8"));
//
// responseStream.
//
//
// if (responseStream != null)
// {
//     // Pipes the stream to a higher level stream reader with the required encoding format.
//     StreamReader readStream = new StreamReader( responseStream, encode );
//     Debug.Log("\r\nResponse stream received.");
//     Char[] read = new Char[256];
//     // Reads 256 characters at a time.
//     int count = readStream.Read( read, 0, 256 );
//     Debug.Log("HTML...\r\n");
//     while (count > 0)
//     {
//         // Dumps the 256 characters on a string and displays the string to the console.
//         String str = new String(read, 0, count);
//         Console.Write(str);
//         count = readStream.Read(read, 0, 256);
//     }
//     // Debug.Log("");
//     // // Releases the resources of the response.
//     // readStream.Close();
//     // // Releases the resources of the Stream.
//     // readStream.Close();
// }

//         }
//     }
// }