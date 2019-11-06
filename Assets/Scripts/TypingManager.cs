using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Random = UnityEngine.Random;

public class TypingManager : MonoBehaviour
{
    private int wordScoreBasal = 10000;
    private int wordScore;
    public List<Word> words;
    public Hashtable leaderboard = new Hashtable();
    public Text scoreDisplay;
    public Text display;
    public Text objectiveWord;
    public GameObject barra;
    public Text errortext;
    private string username;
    public InputField UsernameInputField;
    public Button startButton;
    public Button leaderboardButton;
    public Text usernameText;
    public Button restartButton;
    public Button returnToMenuButton;
    public Text leaderboardText;
    public Button returnLeaderboardButton;

    public bool started;
    // Start is called before the first frame update
    void Start()
    {
        leaderboardText.text = "";
        wordScore = wordScoreBasal;
        barra.SetActive(false);
        display.text = "";
        objectiveWord.text = "";
        scoreDisplay.text = "";
        usernameText.text = "";
        returnToMenuButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        errortext.gameObject.SetActive(false);
        returnLeaderboardButton.gameObject.SetActive(false);
        wordsRefill();
        leaderboard.Add("Debug1",0);
        leaderboard.Add("Debug2",1000);
        leaderboard.Add("Debug3",500);
        leaderboard.Add("Debug4",780);
        leaderboard.Add("Debug5",3);
        leaderboard.Add("Debug6",0);
        leaderboard.Add("Debug7",1000);
        leaderboard.Add("Debug8",500);
        leaderboard.Add("Debug9",780);
        leaderboard.Add("Debug10",3);
        leaderboard.Add("Debug11",0);
        leaderboard.Add("Debug12",1000);
        leaderboard.Add("Debug13",500);
        leaderboard.Add("Debug14",780);
        leaderboard.Add("Debug15",3);
        leaderboard.Add("Debug16",0);
        leaderboard.Add("Debug17",1000);
        leaderboard.Add("Debug18",500);
        leaderboard.Add("Debug19",780);
        leaderboard.Add("Debug20",3);
        leaderboard.Add("Debug21",0);
        leaderboard.Add("Debug22",1000);
        leaderboard.Add("Debug23",500);
        leaderboard.Add("Debug24",780);
        leaderboard.Add("Debug25",3);
        for (int i = 0; i < words.Count; i++) {
            Word temp = words[i];
            int randomIndex = Random.Range(i, words.Count);
            words[i] = words[randomIndex];
            words[randomIndex] = temp;
        }
    }

    private void wordsRefill()
    {
        words.Add(new Word("teclado"));
        //words.Add(new Word("micrófono"));
        //words.Add(new Word("ratón"));
        //words.Add(new Word("mouse"));
        //words.Add(new Word("pointer"));
        //words.Add(new Word("Douglas Engelbart"));
        //words.Add(new Word("Johann Philipp Reis"));
        //words.Add(new Word("Christopher Sholes"));
    }

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            if (words.Count == 0)
            {
                started = false;
                if (leaderboard.Contains(username))
                {
                    leaderboard[username] = wordScore;
                }
                else
                {
                    leaderboard.Add(username, wordScore);
                }
                objectiveWord.text = "Felicidades " + username + "! has obtenido " + wordScore + " puntos.";
                returnToMenuButton.gameObject.SetActive(true);
                restartButton.gameObject.SetActive(true);
                return;
            }
            objectiveWord.text = words[0].text;
            if (wordScore == 0)
            {
            }
            else
            {
                wordScore--;
                scoreDisplay.text = "<color=green>Puntaje:</color>" + wordScore;
            }
        
            string input = Input.inputString;
            if(input.Equals(""))
                return;
            char c = input[0];
            string typing = "";
            for (int i = 0; i < words.Count; i++)
            {
                Word w = words[i];
                if (w.continueText(c))
                {
                    string typed = w.getTyped();
                    typing += typed + "\n";
                    if (typed.Equals(w.text))
                    {
                        words.Remove(w);
                        break;
                    }
                }
            }

            display.text = typing;
        }
    }
    public void onStartButtonClick()
    {
        if (leaderboard.Contains(UsernameInputField.text)) //si el usuario ya existe
        {
            errortext.GetComponent<Text>().text = "<color=red>Ese usuario ya existe, escoje otro.</color>";
            errortext.gameObject.SetActive(true);
        }
        else if(UsernameInputField.text.Length < 3) //si el nombre es muy corto
        {
            errortext.GetComponent<Text>().text = "<color=red>Debes tener un nombre de usuario de al menos 3 caracteres.</color>";
            errortext.gameObject.SetActive(true);
        }
        else //empieza la partida
        {
            errortext.gameObject.SetActive(false);
            barra.SetActive(true);
            leaderboardButton.gameObject.SetActive(false);
            startButton.gameObject.SetActive(false);
            username = UsernameInputField.text;
            UsernameInputField.gameObject.SetActive(false);
            usernameText.text = "Usuario: " + username;
            started = true;
        }
    }

    public void onRestartButtonClick()
    {
        display.text = "";
        wordsRefill();
        wordScore = wordScoreBasal;
        restartButton.gameObject.SetActive(false);
        returnToMenuButton.gameObject.SetActive(false);
        started = true;
    }

    public void onReturnToMenuButtonClick()
    {
        wordsRefill();
        wordScore = wordScoreBasal;
        UsernameInputField.gameObject.SetActive(true);
        startButton.gameObject.SetActive(true);
        leaderboardButton.gameObject.SetActive(true);
        returnToMenuButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        barra.SetActive(false);
        display.text = "";
        objectiveWord.text = "";
        scoreDisplay.text = "";
        usernameText.text = "";
    }

    public void onScoreboardButtonClick()
    {
        startButton.gameObject.SetActive(false);
        UsernameInputField.gameObject.SetActive(false);
        errortext.gameObject.SetActive(false);
        leaderboardButton.gameObject.SetActive(false);
        returnLeaderboardButton.gameObject.SetActive(true);
        var result = new List<DictionaryEntry>(leaderboard.Count);
        int pos = 1;
        foreach (DictionaryEntry entry in leaderboard)
        {
            result.Add(entry);
        }
        result.Sort(
            (x, y) =>
            {
                IComparable comparable = x.Value as IComparable;
                if (comparable != null)
                {
                    return comparable.CompareTo(y.Value);
                }
                return 0;
            });
        for (int i = result.Count - 1; i >= 0; i--)
        {
            DictionaryEntry entry = result[i];
            leaderboardText.text += pos + ". " + entry.Key.ToString() + " : " + entry.Value.ToString() + "\n";
            pos++;
        }
    }

    public void onReturnButtonClick()
    {
        leaderboardText.text = "";
        startButton.gameObject.SetActive(true);
        UsernameInputField.gameObject.SetActive(true);
        leaderboardButton.gameObject.SetActive(true);
        returnLeaderboardButton.gameObject.SetActive(false);
    }
}

[Serializable]
public class Word
{
    public string text;
    public UnityEvent onTyped;
    private string hasTyped = "";
    private int curChar = 0;

    public Word(string t)
    {
        text = t;
        hasTyped = "";
        curChar = 0;
    }

    public bool continueText(char c)
    {
        if (c.Equals(text[curChar]))
        {
            curChar++;
            hasTyped = text.Substring(0, curChar);
            if (curChar >= text.Length+1)
            {
                onTyped.Invoke();
                curChar = 0;
            }
            return true;
        }
        else
        {
            curChar = 0;
            hasTyped = "";
            return false;
        }
    }

    public string getTyped()
    {
        return hasTyped;
    }
}

