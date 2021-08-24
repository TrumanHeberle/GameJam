using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    public class TopDownCharacterController : MonoBehaviour
    {
        public float speed;

        private Animator animator;
        private SpriteRenderer spriteRender;
        private Rigidbody2D body;

        private void Start()
        {
            animator = GetComponent<Animator>();
            spriteRender = GetComponent<SpriteRenderer>();
            body = GetComponent<Rigidbody2D>();
        }


        private void Update()
        {
            bool W = Input.GetKey(KeyCode.W);
            bool A = Input.GetKey(KeyCode.A);
            bool S = Input.GetKey(KeyCode.S);
            bool D = Input.GetKey(KeyCode.D);
            Vector2 dir = new Vector2((D?1:0)-(A?1:0), (W?1:0)-(S?1:0));
            if (A) {
              spriteRender.flipX = true;
            }
            else if (D) {
              spriteRender.flipX = false;
            }
            dir.Normalize();
            animator.SetBool("IdleOrMoving", dir.magnitude > 0);
            body.velocity = speed * dir;
        }
    }
}
