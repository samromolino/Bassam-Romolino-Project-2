using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 4.5f;

    [SerializeField]
    private float jumpForce = 12.0f;

    [SerializeField]
    public int gold;

    [SerializeField]
    public int kills;

    [SerializeField]
    private GameObject goldenPlatformPrefab;

    [SerializeField]
    private GameObject bossPrefab;

    private GameObject goldenPlatform;

    private GameObject bossCharacter;

    public bool onGoldenPlatform;

    public bool isAlive = true;

    public bool proceed = false;

    public bool battle = false;

    public Rigidbody2D body;

    private Animator anim;

    private BoxCollider2D box;

    private ContactFilter2D filter;

    public bool facingRight = true;

    public float drag;


    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();

        filter.useTriggers = false;
        filter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        filter.useLayerMask = true;
    }

    void Update()
    {
        if (isAlive)
        {
            transform.parent = null;

            float deltaX = ((Input.GetAxis("Horizontal") + GetDrag(drag)) * speed) ;
            if (deltaX > 0 && !facingRight)
            {
                Flip();
            }
            else if (deltaX < 0 && facingRight)
            {
                Flip();
            }
            Vector2 movement = new Vector2(deltaX, body.velocity.y);
            body.velocity = movement;

            Vector3 max = box.bounds.max;
            Vector3 min = box.bounds.min;

            Vector2 corner1 = new Vector2(max.x, max.y);
            Vector2 corner2 = new Vector2(min.x, min.y - 0.03f);

            List<Collider2D> colliderList = new List<Collider2D>();

            int touchingObjects = Physics2D.OverlapArea(max, min, filter, colliderList);


            bool grounded = false;

            if (colliderList.Count > 1)
            {
                grounded = true;
            }

            if (!grounded)
            {
                onGoldenPlatform = false;
            }

            body.gravityScale
                = (grounded && Mathf.Approximately(deltaX, 0)) ? 0 : 1;

            if (grounded && Input.GetKeyDown(KeyCode.Space))
            {
                body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }

            GoldenPlatform goldenPlatform = null;
            MovingPlatform movingPlatform = null;
            EnemyBehavior enemy = null;
            BossBehavior boss = null;

            foreach (var hit in colliderList)
            {
                goldenPlatform = hit.GetComponent<GoldenPlatform>();
                movingPlatform = hit.GetComponent<MovingPlatform>();
                enemy = hit.GetComponent<EnemyBehavior>();
                boss = hit.GetComponent<BossBehavior>();

                if (movingPlatform != null)
                {
                    transform.parent = movingPlatform.transform;
                }

                if (goldenPlatform != null)
                {
                    onGoldenPlatform = true;
                    transform.parent = goldenPlatform.transform;
                }

                if (enemy != null)
                {
                    enemy.OnPlayerHit();
                }

                if (boss != null)
                {
                    boss.OnPlayerHit();
                }

            }

            anim.SetFloat("speed", Mathf.Abs(deltaX));

            if (gold == 10 && !proceed)
            {
                this.goldenPlatform = Instantiate(goldenPlatformPrefab) as GameObject;
                this.goldenPlatform.transform.position = new Vector3(6.5f, 0, 0);
                proceed = true;
            }

            if (kills == 3 && !battle)
            {
                this.bossCharacter = Instantiate(bossPrefab) as GameObject;
                this.bossCharacter.transform.position = new Vector3(50.0f, 17f, 0);
                battle = true;
            }

            if (transform.position.y < 0)
            {
                isAlive = false;
                Messenger.Broadcast(GameEvent.GAME_OVER);
            }
        }
    }

    float GetDrag(float slowdown)
    {
        if (slowdown < 0)
        {
            drag = drag + (50 * Time.deltaTime);
        }
        else
        {
            drag = 0;
        }
        slowdown = drag;
        return slowdown;
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 flipperX = transform.localScale;
        flipperX.x *= -1;
        transform.localScale = flipperX;
    }

    public void OnDeath()
    {
        Vector3 flipperY = transform.localScale;
        flipperY.y *= -1;
        transform.localScale = flipperY;
        box.isTrigger = true;
    }
}