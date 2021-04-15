using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tests : MonoBehaviour
{

    public CosmicAPI api;
    public GameObject cardPrefab;
    public GameObject canvas;

    void Start() {
        api.OnLogin += () => {
            //GameObject.Find("LoggedInStatus").GetComponent<Text>().text = "Logged in as " + api.me.username;
        };
    }
   
}
