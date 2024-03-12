using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField]
    TMP_Text goldLabel;

    [SerializeField]
    TMP_Text deadLabel;

    [SerializeField]
    private Player player;

    private void Start()
    {
        GoldCounter();
    }


    private void OnEnable()
    {
        Messenger.AddListener(GameEvent.GOLD_COLLECTED, GoldCounter);
        Messenger.AddListener(GameEvent.GAME_OVER, GameOver);
        Messenger.AddListener(GameEvent.WIN , Win);
    }


    private void OnDisable()
    {
        Messenger.RemoveListener(GameEvent.GOLD_COLLECTED, GoldCounter);
        Messenger.RemoveListener(GameEvent.WIN , Win);
    }

    private void GoldCounter()
    {
        goldLabel.text = "Gold: " + player.gold.ToString();
    }

    private void GameOver()
    {
        deadLabel.color = new Color(255, 0, 0, 255);
        deadLabel.text = "Game Over";
    }

    private void Win()
    {
        deadLabel.color = new Color(0, 255, 0, 255);
        deadLabel.text = "You Win";
    }
}
