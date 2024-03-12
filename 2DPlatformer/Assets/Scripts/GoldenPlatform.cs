using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenPlatform : MonoBehaviour
{
    [SerializeField]
    private Vector3 finishPos;

    [SerializeField]
    private float speed = 0.5f;

    [SerializeField]
    private int movementDistanceX = 0;

    [SerializeField]
    private int movementDistanceY = 0;

    private Vector3 startPos;

    private float trackPercent = 0f;

    private float x;

    private float y;

    private Player player;

    void Start()
    {
        startPos = transform.position;
        x = startPos.x;
        y = startPos.y;
        finishPos.x = x + movementDistanceX;
        finishPos.y = y + movementDistanceY;

        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        if (player.onGoldenPlatform && trackPercent <= 1f) 
        {
            trackPercent += speed * Time.deltaTime;

            x = (finishPos.x - startPos.x) * trackPercent + startPos.x;
            y = (finishPos.y - startPos.y) * trackPercent + startPos.y;

            transform.position = new Vector3(x, y, startPos.z);
        }
    }
}
