using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Websockets to connect with the server
using NativeWebSocket;
// JSON protocol to parse and pack packets from and to the server.
using Newtonsoft.Json;
using UnityEditor;

public class Character {
    public string id;
    public int hp;
    public bool isAttacking, hasAttacked, hasBeenAttacked;
    public Buff buff;
}

[Serializable]
public class Minion : Character {
    public int origin, spawnRound;
    public bool canSacrifice;
    public Player owner;
}

[Serializable]
public class Player : Character {
    public string name;
    public bool isBot;
    public int level, manaLeft, totalMana;
    public Card[] cards;
    public Minion[] minions;
}

[Serializable]
public class Buff {
    public int sacrifices, damage, mana;
}

[Serializable]
public class Card {
    public int id, cost, damage, hp;
    public string rarity;
    public Sprite image = null;
    public string name, description;
    public bool isRush, isTaunt;
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

[Serializable]
public class Profile {
    public string id, username;
    public int level, xp;
    public int[] cards;
    public bool admin;
    public Record record;
}
[Serializable]
public class Record {
    public int wins, losses;
}

[Serializable]
public class SocketPackage {
    public string identifier, packet, token;
}

[Serializable]
public class CosmicAPI : MonoBehaviour
{

    // Socket connection wit the server
    WebSocket ws;

    // On connection with the cosmic game server
    public Action OnConnected { get; set; }
    // On User info
    public Action OnLogin { get; set; }
    // On every game update from the server, not specifik
    public Action OnUpdate { get; set; }
    // When a new game starts (from main menu)
    public Action OnGameStart { get; set; }
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


    Player[] players;

    Card[] cards;

    // Client account ID
    public Profile me;
    // Game ID
    string id;
    DateTime gameStarted, roundStarted;
    int roundLength, round;
    bool opponentIsBot;
    bool activeGame = false;

    string token;

    public void StartTestGame() {
        Send("start_test");
    }

    public Player GetMe() {
        foreach(Player player in players) {
            if (player.id == me.id) return player;
        }
        return null;
    }

    public Player GetOpponent() {
        foreach (Player player in players) {
            if (player.id == me.id) return player;
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
        foreach (Card card in cards) {
            if (card.id == id) return card;
        }
        return null;
    }

    // Play a card from the hand
    public void PlayMinion(int id) {
        Debug.Log("Played minion ID: " + id);
    }

    // Gives you all the IDs of all the cards (For testing)
    public int[] GetAllCardIDs() {

        int[] cardIDs = new int[cards.Length];
        for(int i = 0; i < cards.Length; i++) {
            cardIDs[i] = cards[i].id;
        }
        return cardIDs;
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

    // Try to reconnect if it loses conection to the server
    IEnumerator Reconnect() {
        yield return new WaitForSeconds(1);
        ws.Connect();
    }

    async void Start() {

        token = PlayerPrefs.GetString("token");

        // Create socket and input the game server URL
        ws = new WebSocket("wss://api.cosmic.ygstr.com");

       
        ws.OnOpen += () => {
            Debug.Log("Connected to Cosmic server");
            Login();
        };

        ws.OnMessage += (bytes) => {

            string message = System.Text.Encoding.UTF8.GetString(bytes);
            SocketPackage package = JsonUtility.FromJson<SocketPackage>(message);

            switch (package.identifier) {
                case "cards":
                    LoadCards(package.packet);
                    break;
                case "new_token":
                    token = package.packet;
                    PlayerPrefs.SetString("token", token);
                    Login();
                break;
                case "user":
                OnUser(package.packet);
                break;
            }

        };


        ws.OnClose += (e) => {
            Debug.Log("Connection closed");
            StartCoroutine(Reconnect());
        };

        // Connect to the server
        await ws.Connect();
    }

    void LoadCards(string cardsJson) {
       
        cards = JsonConvert.DeserializeObject<Card[]>(cardsJson);   
        foreach(Card card in cards) {
            card.image = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/card-images/" + card.id + ".png", typeof(Sprite));
        }
        OnConnected();
    }

    public Card[] GetCards() {
  
        return cards;
    }

    void Send(string identifier) {
        Send(identifier, "");
    }

    void Send(string identifier, string data) {
        SocketPackage package = new SocketPackage();
        package.identifier = identifier;
        package.packet = data;
        package.token = token;
        string json = JsonUtility.ToJson(package);
        ws.SendText(json);
    }

    void Login(/*string username, string password*/) {
        Send("login");
    }

    void OnUser(string packet) {
        me = JsonConvert.DeserializeObject<Profile>(packet);
        Debug.Log("Logged in as " + me.username);
        OnLogin();
    }

    void Update() {
        
        #if !UNITY_WEBGL || UNITY_EDITOR
            ws.DispatchMessageQueue();
        #endif

        
    }
}
