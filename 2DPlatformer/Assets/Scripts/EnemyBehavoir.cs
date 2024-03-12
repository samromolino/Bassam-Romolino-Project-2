using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;

    private Player player;

    private float minHeight;

    private float maxHeight;

    private Vector3 startPos;

    private Vector3 currentPos;

    private BoxCollider2D box;


    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        startPos = transform.position;

        minHeight = startPos.y - 2;

        maxHeight = startPos.y + 2;

        box = GetComponent<BoxCollider2D>();

    }

    void Update()
    {
        currentPos = transform.position;

        if (player.isAlive)
        {
            if (minHeight <= player.transform.position.y && player.transform.position.y <= maxHeight)
            {
                if (currentPos.x <= player.transform.position.x)
                {
                    transform.Translate(speed * Time.deltaTime, 0, 0);
                }
                else
                {
                    transform.Translate(-speed * Time.deltaTime, 0, 0);
                }
            }
        }

        if (transform.position.y < -10) 
        {
            Destroy(this.gameObject);
        }
    }

    public void OnPlayerHit()
    {
        if (player.transform.position.y > currentPos.y + .1f)
        {
            OnDeath();
        }
        else
        {
            player.isAlive = false;
            player.OnDeath();
            Messenger.Broadcast(GameEvent.GAME_OVER);
        }
    }

    void OnDeath()
    {
        player.kills++;
        Vector3 flipperY = transform.localScale;
        flipperY.y *= -1;
        transform.localScale = flipperY;
        box.isTrigger = true;
    }
}
