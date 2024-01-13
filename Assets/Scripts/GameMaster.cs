using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance;

    public Player playerPrefab;

    protected Player player;

    private void Awake()
    {
        Instance = this;
        SpawnPlayer(transform.position);
    }

    public Player SpawnPlayer(Vector3 pos)
    {
        if (player) Destroy(player.gameObject);
        player = Instantiate(playerPrefab);
        player.transform.position = pos;
        return player;
    }

    public Player Player { get { return player; } }
}
