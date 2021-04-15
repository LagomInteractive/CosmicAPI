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
        api.OnConnected += () => {
            //api.StartTestGame();
            
            Card[] cards = api.GetCards();

            for (int i = 0; i < cards.Length; i++) {
                Card card = cards[i];

                int x = i % 15;
                int y = (i-  x) / 15;

                Transform testCard = Instantiate(cardPrefab, canvas.transform).transform;
                testCard.GetComponent<RectTransform>().localPosition = new Vector3(-342.2f + (x * 50), 103.4f - (y * 130));
                testCard.transform.Find("Name").GetComponent<Text>().text = card.name;
                testCard.transform.Find("Description").GetComponent<Text>().text = card.description;
                testCard.transform.Find("Image").GetComponent<Image>().sprite = card.image;
            }
        };
    }
   
}
