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

        api.OnEverythingLoaded += () => {
            Debug.Log("Everything loaded!");
        };

        api.OnGameStart += () => {
            Debug.Log("New game started!");
            Debug.Log("My deck size: " + api.GetPlayer().deck.Length);
        };

        api.OnTurn += (attackingPlayer) => {
            Debug.Log("New round starting! " + attackingPlayer + " is attacking");
        };



    }

    public void SpawnRandomCard() {
        api.InstantiateWorldCard(api.GetAllCardIDs()[cardIndex++]);
    }

}
