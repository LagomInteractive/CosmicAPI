using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tests : MonoBehaviour
{

    public CosmicAPI api;

    void Start() {
        api.OnConnected += () => {
            api.StartTestGame();
        };
    }
   
}
