using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehavior : MonoBehaviour
{
    [SerializeField]
    private int coinValue = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        string type = other.name;

        if (type == "Player")
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.gold += coinValue;
                Messenger.Broadcast(GameEvent.GOLD_COLLECTED);
                Destroy(this.gameObject);
            }
        }
    }
}