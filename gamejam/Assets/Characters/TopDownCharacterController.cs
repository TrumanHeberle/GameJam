﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    public class TopDownCharacterController : MonoBehaviour
    {
        public float speed;

        private Animator animator;
        private SpriteRenderer spriteRender;

        private void Start()
        {
            animator = GetComponent<Animator>();
            spriteRender = GetComponent<SpriteRenderer>();
        }


        private void Update()
        {
            Vector2 dir = Vector2.zero;
            if (Input.GetKey(KeyCode.A))
            {
                dir.x = -1;
                spriteRender.flipX = true;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                dir.x = 1;
                spriteRender.flipX = false;
            }

            if (Input.GetKey(KeyCode.W))
            {
                dir.y = 1;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                dir.y = -1;
            }

            dir.Normalize();
            animator.SetBool("IdleOrMoving", dir.magnitude > 0);

            GetComponent<Rigidbody2D>().velocity = speed * dir;
        }
    }
}
