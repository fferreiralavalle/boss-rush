using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PMainAttack : PState
{
    protected float attackTimeLeft = 0f;
    public PMainAttack(Player player) : base(player)
    {
        animatorEventName = "MainAttack";
    }

    public override void Enter()
    {
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 playerPos = Camera.main.WorldToScreenPoint(_player.transform.position);
        attackTimeLeft = _player.playerWeapons.mainWeaponAttack.projectileData.inmobileTime;
        UpdateAnimatorDirection(_player, mouseScreenPos - playerPos);
        _player.playerWeapons.SpawnMainWeaponAttack(_player, mouseScreenPos - playerPos);
        base.Enter();
    }

    public override void Leave()
    {
        base.Leave();
    }

    public override void OnFixedUpdate()
    {
        attackTimeLeft -= Time.fixedDeltaTime;
        base.OnFixedUpdate();
        if (attackTimeLeft < 0f)
        {
            HandleFinish();
        }
    }
}
