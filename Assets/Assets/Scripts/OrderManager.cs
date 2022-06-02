using System;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Assets.Scripts
{
    public class OrderManager : MonoBehaviour
    {
        public InputField orderQuantityInput;
        public Text periodDataOutput;

        public void SubmitOrder()
        {
            StartCoroutine(SubmitOrderRequest());
        }

        private IEnumerator SubmitOrderRequest()
        {
            if (orderQuantityInput == null) yield break;

            int orderQuantity = Int16.Parse(orderQuantityInput.text);

            Debug.Log($"Ordered {orderQuantity} items");

            WWWForm form = new WWWForm();
            form.AddField("orderQuantity", orderQuantity);
            form.AddField("gameId", PlayerPrefs.GetInt("game.pk"));
            form.AddField("playerId", PlayerPrefs.GetInt("player.pk"));
            form.AddField("periodId", PlayerPrefs.GetInt("period.pk"));
            UnityWebRequest orderRequest = UnityWebRequest.Post(APIConstants.OrderAddress, form);
            orderRequest.SetRequestHeader("Authorization", "Token " + PlayerPrefs.GetString("authToken"));

            yield return orderRequest.SendWebRequest();

            if (orderRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Placing order failed: " + orderRequest.error + " " + orderRequest.downloadHandler.error);
            }
            else
            {
                Debug.Log("Place order successful: " + orderRequest.downloadHandler.text);
                string jsonResponse = orderRequest.downloadHandler.text;

                GameResponse gameResponse = JsonUtility.FromJson<GameResponse>(jsonResponse);

                Period period = gameResponse.period_data;

                periodDataOutput.text = period.ToString();


                yield return NextRoundResponseStream.LogNextRoundResponse();
            }
        }
    }
}