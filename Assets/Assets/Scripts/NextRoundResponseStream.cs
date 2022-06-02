using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts
{
    public class NextRoundResponseStream : MonoBehaviour
    {
        public static Text periodDataOutput;

        public static IEnumerator LogNextRoundResponse()
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

                    Period currentPeriod = gameResponse.period_data;

                    if (!currentPeriod.everyone_ordered)
                    {
                        yield return new WaitForSeconds(2f);
                    }
                    else
                    {
                        canStart = true;
                        Debug.Log("Next round can be started, updating data...");
                        WWWForm nextForm = new WWWForm();
                        nextForm.AddField("gameId", PlayerPrefs.GetInt("game.pk"));
                        nextForm.AddField("playerId", PlayerPrefs.GetInt("player.pk"));
                        nextForm.AddField("periodId", PlayerPrefs.GetInt("period.pk"));
                        int period_id = PlayerPrefs.GetInt("period.pk");
                        Debug.Log(period_id);
                        UnityWebRequest nextRoundRequest = UnityWebRequest.Post(APIConstants.NextRoundAddress, nextForm);
                        nextRoundRequest.SetRequestHeader("Authorization",
                            "Token " + PlayerPrefs.GetString("authToken"));

                        yield return nextRoundRequest.SendWebRequest();

                        if (nextRoundRequest.result != UnityWebRequest.Result.Success)
                        {
                            Debug.Log("Failed to start next round: " + nextRoundRequest.error + " " +
                                      nextRoundRequest.downloadHandler.error);
                        }
                        else
                        {
                            Debug.Log("Start next round successful: " + nextRoundRequest.downloadHandler.text);

                            string nextRoundJsonResponse = nextRoundRequest.downloadHandler.text;

                            GameResponse newGameResponse = JsonUtility.FromJson<GameResponse>(nextRoundJsonResponse);

                            Game game = newGameResponse.game_data;
                            Player player = newGameResponse.player_data;
                            Period period = newGameResponse.period_data;
                            // Player currentPlayer = JsonUtility.FromJson<Player>;

                            Debug.Log("Game Data full: " + jsonResponse);
                            Debug.Log("Game data response: " + gameResponse);
                            Debug.Log("Game status: " + game.game_status);
                            Debug.Log("Player role: " + player.role);

                            // Save new Period in PlayerPrefs
                            PlayerPrefs.SetInt("period.pk", period.pk);
                            PlayerPrefs.SetInt("period.backorders", period.backorders);
                            PlayerPrefs.SetInt("period.costs", period.costs);
                            PlayerPrefs.SetInt("period.current_period", period.current_period);
                            PlayerPrefs.SetInt("period.everyone_ordered", period.everyone_ordered ? 1 : 0);
                            PlayerPrefs.SetInt("period.game", period.game);
                            PlayerPrefs.SetInt("period.incoming_goods", period.incoming_goods);
                            PlayerPrefs.SetInt("period.inventory", period.inventory);
                            PlayerPrefs.SetInt("period.orders_processing", period.orders_processing);
                            PlayerPrefs.SetInt("period.outgoing_goods", period.outgoing_goods);
                            PlayerPrefs.SetInt("period.placed_orders", period.placed_orders);
                            PlayerPrefs.SetInt("period.player", period.player);
                            PlayerPrefs.SetInt("period.raw_materials", period.raw_materials);
                            PlayerPrefs.SetInt("period.incoming_orders", period.incoming_orders);
                            PlayerPrefs.Save();

                            // TODO: animation to show player that a new period started
                            
                            
                            periodDataOutput.text = period.ToString();

                            // Teleport the player back to the lobby once the game is finished
                             if (period.current_period == game.num_periods)
                            {
                                SceneManager.LoadScene("LobbyScene");
                            }
                        }
                    }
                }
            }
        }
    }
}