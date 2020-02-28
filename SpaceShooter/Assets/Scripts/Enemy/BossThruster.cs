using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossThruster : WeakPoint
{
    [Header("Thruster")]
    [SerializeField] bool reducesSpeed = true;
    [SerializeField] float speedReduction;
    [SerializeField] float turnSpeedReduction;
    [SerializeField] Animator thrusterEffect;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if(owner.IsMoving())
        {
            thrusterEffect.SetBool("IsOn", true);
        }
        else
        {
            thrusterEffect.SetBool("IsOn", false);
        }
    }

    protected override void PrepareForDeath()
    {
        base.PrepareForDeath();
        spriteRenderer.color = new Color(68, 68, 68);
        if (reducesSpeed)
        {
            owner.ReduceSpeed(speedReduction);
            owner.ReduceTurnSpeed(turnSpeedReduction);
            thrusterEffect.gameObject.SetActive(false);
        }
    }
}
