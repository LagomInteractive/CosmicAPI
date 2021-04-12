using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character {
    public string id;
    public int hp;
    public bool isAttacking, hasAttacked, hasBeenAttacked;
    public Buff buff;
}

public class Minion : Character {
    public int origin, spawnRound;
    public bool canSacrifice;
    public Player owner;
}

public class Player : Character {
    public string name;
    public bool isBot;
    public int level, manaLeft, totalMana;
    public Card[] cards;
    public Minion[] minions;
}

public class Buff {
    public int sacrifices, damage, mana;
}

public class Card {
    public int id, cost, damage, hp;
    public Rarity rarity;
    public Texture2D image;
    public string name, description;
    public CardType type;
    public Element element;
}

public enum CardType {
    Minion, TargetSpell, AOESpell
}

public enum Element {
    Lunar, Solar, Zenith, Nova
}

public enum Rarity {
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

public class CosmicAPI : MonoBehaviour
{

    public Action OnUpdate { get; set; }
    // UUID Minion
    public Action<string> OnMinionSpawned { get; set; }
    // UUID Minion
    public Action<string> OnMinionDeath { get; set; }
    // Card ID
    public Action<int> OnCard { get; set; }
    // UUID Attacking Player
    public Action<string> OnRoundEnd { get; set; }
    // UUID Winning player
    public Action<string> OnGameEnd { get; set; }
    // UUID from, UUID to, float amount
    public Action<string, string, float> OnDamage { get; set; }



    public Player[] players;
    public Card[] cards;

    // Client account ID
    public string me;
    // Game ID
    public string id;
    public DateTime gameStarted, roundStarted;
    public int roundLength, round;
    public bool opponentIsBot;

    public Player GetMe() {
        foreach(Player player in players) {
            if (player.id == me) return player;
        }
        return null;
    }

    public Player GetOpponent() {
        foreach (Player player in players) {
            if (player.id == me) return player;
        }
        return null;
    }

    public Character GetCharacter(string id) {
        List<Character> characters = GetAllCharacters();
        foreach(Character character in characters) {
            if (character.id == id) return character;
        }
        return null;
    }

    public List<Character> GetAllCharacters() {
        List<Character> characters = new List<Character>();
        // Get all players and add them the the List
        foreach (Player player in players) {
            characters.Add(player);
            // Add all the players minions to the List
            foreach(Minion minion in player.minions) characters.Add(minion);
            
        }

        return characters;
    }

    public Card GetCard(int id) {
        foreach(Card card in cards) {
            if (card.id == id) return card;
        }
        return null;
    }

    // Play a card from the hand
    public void PlayMinion(int id) {
        Debug.Log("Played minion ID: " + id);
    }

    // Play a spell card, only if the card is a targeted spell provide a target player or minion
    public void PlaySpell(int id, string target = null) {
        Debug.Log("Played spell: " + id + " targeted: " + target);
    }

    public void Sacrifice(string id) {
        Debug.Log("Sacrificed minion ID: " + id);
    }

    public void EndRound() {
        Debug.Log("Ended round");
    }

    void Start() {
        
    }

    void Update() {
       
    }
}
