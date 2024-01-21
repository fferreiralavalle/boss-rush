using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PDeadState : PState
{
    public float gameOverScreenDelay = 2f;
    public PDeadState(Player player, float gameOverScreenDelay) : base(player)
    {
        animatorEventName = "Dead";
        this.gameOverScreenDelay = gameOverScreenDelay;
    }

    public override void Enter()
    {
        base.Enter();
        AudioMaster.Instance.StopMusic();
        _player.StartCoroutine(ShowGameoverScreen());
    }

    public IEnumerator ShowGameoverScreen()
    {
        yield return new WaitForSeconds(gameOverScreenDelay);
        UIGameOverMaster.Instance.Show();

    }

    public override void Leave()
    {
        base.Leave();
    }
}
