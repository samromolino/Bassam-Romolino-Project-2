using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.5f;

    [SerializeField]
    private int movementDistanceX = 3;

    [SerializeField]
    private float jumpForce = 5.0f;

    [SerializeField]
    private int hp = 3;

    [SerializeField]
    private int knockback = 20;

    private Vector2 force;

    private Vector3 startPos;

    private Vector3 finishPos;

    private Vector3 currentPos;

    private float trackPercent = 0f;

    private int direction = 1;

    private float x;

    private Player player;

    private Rigidbody2D body;

    private BoxCollider2D box;

    private bool isAlive = true;

    private float damageCooldown = 1f;

    private float damageTimer = 0f;


    void Start()
    {
        force = new Vector2(knockback, -knockback);
        body = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        player = GameObject.Find("Player").GetComponent<Player>();

        startPos = transform.position;
        x = startPos.x;
        finishPos.x = x + movementDistanceX;
    }


    void Update()
    {
        if (player.isAlive)
        {
            damageTimer += Time.deltaTime;

            currentPos = transform.position;

            trackPercent += direction * speed * Time.deltaTime;
            transform.Translate(speed * Time.deltaTime * direction, 0, 0);

            x = (finishPos.x - startPos.x) * trackPercent + startPos.x;

            if ((direction == 1 && trackPercent >= 1f) || (direction == -1 && trackPercent <= 0f))
            {
                direction *= -1;
            }

            var rnd = new System.Random();
            int turn = rnd.Next(1, 1001);

            if (turn == 1)
            {
                direction *= -1;
            }

            int jump = rnd.Next(1, 1001);

            if (jump == 1)
            {
                body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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
            if (isAlive && (damageTimer >= damageCooldown))
            {
                hp--;
                if (hp >= 1)
                {
                    player.body.AddForce(force, ForceMode2D.Impulse);
                    player.drag = -10;
                    damageTimer = 0;
                }
                else
                {
                    OnDeath();
                }
            }
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
        Vector3 flipperY = transform.localScale;
        flipperY.y *= -1;
        transform.localScale = flipperY;
        box.isTrigger = true;
        Messenger.Broadcast(GameEvent.WIN);
    }
}
