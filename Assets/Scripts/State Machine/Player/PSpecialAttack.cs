using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PSpecialAttack : PState
{
    protected float attackTimeLeft = 0f;
    public PSpecialAttack(Player player) : base(player)
    {
        animatorEventName = "MainAttack";
    }

    public override void Enter()
    {
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 playerPos = Camera.main.WorldToScreenPoint(_player.transform.position);
        attackTimeLeft = _player.playerWeapons.specialWeaponAttack.projectileData.inmobileTime;
        UpdateAnimatorDirection(_player, mouseScreenPos - playerPos);
        _player.playerWeapons.SpawnSpecialWeaponAttack(_player, mouseScreenPos - playerPos);
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
