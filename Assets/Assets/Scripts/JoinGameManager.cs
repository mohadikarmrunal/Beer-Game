using System.Collections;
using UnityEngine.Networking;
using UnityEngine;

namespace Assets.Scripts
{
    public class JoinGameManager : MonoBehaviour
    {
        public void JoinGame()
        {
            StartCoroutine(JoinGameRequest());
        }

        private IEnumerator JoinGameRequest()
        {
            UnityWebRequest joinGameRequest = UnityWebRequest.Get(APIConstants.JoinGameAddress);
            Debug.Log("Current auth token:  " + PlayerPrefs.GetString("authToken"));
            joinGameRequest.SetRequestHeader("Authorization", "Token " + PlayerPrefs.GetString("authToken"));

            yield return joinGameRequest.SendWebRequest();

            if (joinGameRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Failed to join game: " + joinGameRequest.error + " " +
                          joinGameRequest.downloadHandler.error);
            }
            else
            {
                Debug.Log("Join game successful: " + joinGameRequest.downloadHandler.text);

                string jsonResponse = joinGameRequest.downloadHandler.text;

                GameResponse gameResponse = JsonUtility.FromJson<GameResponse>(jsonResponse);

                Game game = gameResponse.game_data;
                Player player = gameResponse.player_data;
                Period period = gameResponse.period_data;
                // Player currentPlayer = JsonUtility.FromJson<Player>;

                Debug.Log("Game Data full: " + jsonResponse);
                Debug.Log("Game data response: " + gameResponse);
                Debug.Log("Game status: " + game.game_status);
                Debug.Log("Player role: " + player.role);

                // Save current Game in PlayerPrefs
                PlayerPrefs.SetInt("game.pk", game.pk);
                PlayerPrefs.SetInt("game.can_start", game.can_start ? 1 : 0);
                PlayerPrefs.SetString("game.created_at", game.created_at);
                PlayerPrefs.SetString("game.finished_at", game.finished_at);
                PlayerPrefs.SetString("game.game_status", game.game_status);
                PlayerPrefs.SetInt("game.is_joinable", game.is_joinable ? 1 : 0);
                PlayerPrefs.SetInt("game.num_periods", game.num_periods);
                PlayerPrefs.SetInt("game.num_players", game.num_players);
                PlayerPrefs.Save();

                // Save current Player in PlayerPrefs
                PlayerPrefs.SetInt("player.pk", player.pk);
                PlayerPrefs.SetInt("player.ordered", player.ordered ? 1 : 0);
                PlayerPrefs.SetInt("player.game", player.game);
                PlayerPrefs.SetInt("player.role", player.role);
                PlayerPrefs.SetInt("player.user", player.user);
                PlayerPrefs.Save();

                // Save current Period in PlayerPrefs
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

                yield return WaitingRoomResponseStream.LogWaitingRoomResponse();
            }
        }
    }
}