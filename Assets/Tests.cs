using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tests : MonoBehaviour
{
    public Texture2D cardTextTexture;
    public CosmicAPI api;
    void Start() {
        Player me = CreateTestPlayer("Me!", false);
        Player opponent = CreateTestPlayer("Opponent", true);

        me.cards = new Card[] { CreateTestCard(), CreateTestCard(), CreateTestCard(), CreateTestCard() };
        me.minions = new Minion[] { CreateTestMinion(me), CreateTestMinion(me), CreateTestMinion(me) };

        opponent.cards = new Card[] { CreateTestCard(), CreateTestCard(), CreateTestCard(), CreateTestCard() };
        opponent.minions = new Minion[] { CreateTestMinion(opponent), CreateTestMinion(opponent), CreateTestMinion(opponent) };

        api.players = new Player[] { me, opponent };
        api.me = me.id;
        api.opponentIsBot = true;

        Debug.Log("Test started: Player list length: " + api.players.Length);
    }

    Player CreateTestPlayer(string name, bool isBot) {
        Player player = new Player();
        player.cards = new Card[] { CreateTestCard(), CreateTestCard(), CreateTestCard(), CreateTestCard(), CreateTestCard(), };
        player.hp = 30;
        player.id = CreateTestID();
        player.level = 1;
        player.manaLeft = 10;
        player.totalMana = 10;
        player.isAttacking = true;
        player.hasAttacked = false;
        player.hasBeenAttacked = false;
        player.name = name;
        player.isBot = isBot;
        return player;
    }

    Minion CreateTestMinion(Player owner) {
        Minion minion = new Minion();
        minion.origin = 0;
        minion.owner = owner;
        
        return minion;
    }

    Card CreateTestCard() {
        Card card = new Card();
        card.cost = 3;
        card.id = 0;
        card.damage = 5;
        card.element = Element.Lunar;
        card.name = "TEST-CARD";
        card.description = "DESCRIPTION\n<b>BOLD</b>\n<i>ITALIC</i>";
        card.rarity = Rarity.Legendary;
        card.type = CardType.Minion;
        card.image = cardTextTexture;
        return card;
    }

    string CreateTestID() {
        Guid g = Guid.NewGuid();
        string GuidString = Convert.ToBase64String(g.ToByteArray());
        GuidString = GuidString.Replace("=", "");
        GuidString = GuidString.Replace("+", "");
        return GuidString;
    }

    void Update() {
        
    }
}
