using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tests : MonoBehaviour {

    public CosmicAPI api;
    public GameObject cardPrefab;
    public GameObject canvas;

    public InputField usernameInput, passwordInput;
    public Button loginButton;
    public Text wrongPasswordWarning;

    void Start() {
        api.OnLogin += () => {
            GameObject.Find("LoggedInStatus").GetComponent<Text>().text = "Logged in as " + api.me.username;
            wrongPasswordWarning.gameObject.SetActive(false);
            usernameInput.text = api.me.username;
            passwordInput.text = "";
        };

        api.OnCard += (int id) => {
            Debug.Log("Oncard called with id " + id);
        };

        loginButton.onClick.AddListener(() => {
            api.Login(usernameInput.text, passwordInput.text);
        });

        api.OnLoginFail = () => {
            wrongPasswordWarning.gameObject.SetActive(true);

        };

    }

}
