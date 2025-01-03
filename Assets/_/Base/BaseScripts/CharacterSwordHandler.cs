﻿using System;
using UnityEngine;
using V_AnimationSystem;
using CodeMonkey.Utils;

/*
 * Player movement with Arrow keys
 * Attack with Space
 * */
public class CharacterSwordHandler : MonoBehaviour {

    private const float speed = 50f;
        
    private V_UnitSkeleton unitSkeleton;
    private V_UnitAnimation unitAnimation;
    private AnimatedWalker animatedWalker;
    private State state;

    private enum State {
        Normal,
        Busy,
    }

    private void Start() {
        Transform bodyTransform = transform.Find("Body");
        unitSkeleton = new V_UnitSkeleton(1f, bodyTransform.TransformPoint, (Mesh mesh) => bodyTransform.GetComponent<MeshFilter>().mesh = mesh);
        unitAnimation = new V_UnitAnimation(unitSkeleton);
        animatedWalker = new AnimatedWalker(unitAnimation, GameAssets.UnitAnimTypeEnum.dSwordTwoHandedBack_Idle, GameAssets.UnitAnimTypeEnum.dSwordTwoHandedBack_Walk, 1f, .75f);

        state = State.Normal;
    }

    private void Update() {
        switch (state) {
        case State.Normal:
            HandleMovement();
            HandleAttack();
            break;
        case State.Busy:
            HandleAttack();
            break;
        }
        unitSkeleton.Update(Time.deltaTime);
    }

    private void HandleMovement() {
        float moveX = 0;
        float moveY = 0;

        if (Input.GetKey(KeyCode.UpArrow)) {
            moveY = 1;
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            moveY = -1;
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            moveX = -1;
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            moveX = 1;
        }

        Vector3 moveDir = new Vector3(moveX, moveY).normalized;
        bool isIdle = moveX == 0 && moveY == 0;
        if (!isIdle) {
            Dirt_Handler.SpawnInterval(GetPosition() + new Vector3(0, -4), moveDir * -1);
        }
        animatedWalker.SetMoveVector(moveDir);
        transform.position = transform.position + moveDir * speed * Time.deltaTime;
    }

    private void HandleAttack() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            // Attack
            SetStateBusy();

            Vector3 attackDir = animatedWalker.GetLastMoveVector();
            transform.position = transform.position + attackDir * 4f;

            /*
            Transform swordSlashTransform = Instantiate(GameAssets.i.pfSwordSlash, GetPosition() + attackDir * 13f, Quaternion.Euler(0, 0, UtilsClass.GetAngleFromVector(attackDir)));
            swordSlashTransform.GetComponent<SpriteAnimator>().onLoop = () => Destroy(swordSlashTransform.gameObject);

            UnitAnimType activeAnimType = unitAnimation.GetActiveAnimType();
            if (activeAnimType == GameAssets.UnitAnimTypeEnum.dSwordTwoHandedBack_Sword) {
                swordSlashTransform.localScale = new Vector3(swordSlashTransform.localScale.x, swordSlashTransform.localScale.y * -1, swordSlashTransform.localScale.z);
                unitAnimation.PlayAnimForced(GameAssets.UnitAnimTypeEnum.dSwordTwoHandedBack_Sword2, attackDir, 1f, (UnitAnim unitAnim) => SetStateNormal(), null, null);
            } else {
                unitAnimation.PlayAnimForced(GameAssets.UnitAnimTypeEnum.dSwordTwoHandedBack_Sword, attackDir, 1f, (UnitAnim unitAnim) => SetStateNormal(), null, null);
            }
            */
        }
    }

    private void SetStateBusy() {
        state = State.Busy;
    }

    private void SetStateNormal() {
        state = State.Normal;
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

        
}
