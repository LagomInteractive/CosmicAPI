using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Tests : MonoBehaviour {

    public CosmicAPI api;
    public GameObject cardPrefab;
    public GameObject canvas;

    public InputField usernameInput, passwordInput;
    public Button loginButton;
    public Text wrongPasswordWarning, pingText;

    int cardIndex = 0;

    void Start() {
        api.OnLogin += () => {
            GameObject.Find("LoggedInStatus").GetComponent<Text>().text = "Logged in as " + api.GetMe().username;
            wrongPasswordWarning.gameObject.SetActive(false);
            usernameInput.text = api.GetMe().username;
            passwordInput.text = "";
        };

        api.OnConnected += () => {
            Task.Run(async () => {
                for (; ; ) {
                    await Task.Delay(400);
                    api.Ping();
                }

            });
        };

        api.OnDisconnected += () => {
            GameObject.Find("LoggedInStatus").GetComponent<Text>().text = "Disconnected, trying to relogin";
        };

        api.OnPing += (int ping) => {
            pingText.text = "Ping: " + ping + "ms";
        };

        loginButton.onClick.AddListener(() => {
            api.Login(usernameInput.text, passwordInput.text);
        });

        api.OnLoginFail += () => {
            wrongPasswordWarning.gameObject.SetActive(true);
        };

        api.OnGameStart += () => {
            Debug.Log("New game started!");
        };

        api.OnOpponentCard += () => {
            Debug.Log("Opponent drew a card");
        };

        api.OnCard += (cardId) => {
            Debug.Log("Drew a card: " + api.GetCard(cardId).name);
        };

        api.OnTurn += (attackingPlayer) => {
            string attackingPlayerName = "Opponent is";
            if (attackingPlayer == api.GetMe().id) attackingPlayerName = "You are";
            Debug.Log("New round (" + api.GetGame().round + ") starting! " + attackingPlayerName + " attacking!");
        };





    }

    public void SpawnRandomCard() {
        GameObject card = api.InstantiateWorldCard(api.GetAllCardIDs()[cardIndex % api.GetAllCardIDs().Length]);
        cardIndex++;
        card.transform.position = card.transform.position + new Vector3(UnityEngine.Random.Range(-17, 17), UnityEngine.Random.Range(-5, 8), 0);
    }

}
