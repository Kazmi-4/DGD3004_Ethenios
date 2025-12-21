using System.Collections;
using System.Collections.Generic;
using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    [RequireComponent(typeof(AnimationController), typeof(Collider2D))]
    public class EnemyController : MonoBehaviour
    {
        public PatrolPath path;
        public AudioClip ouch;

        // Slot for your Animator Override Controller
        public AnimatorOverrideController animatorOverride;

        internal PatrolPath.Mover mover;
        internal AnimationController control;
        internal Collider2D _collider;
        internal AudioSource _audio;
        SpriteRenderer spriteRenderer;

        Vector3 _initialPosition;

        public Bounds Bounds => _collider.bounds;

        void Awake()
        {
            control = GetComponent<AnimationController>();
            _collider = GetComponent<Collider2D>();
            _audio = GetComponent<AudioSource>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            _initialPosition = transform.position;

            // Apply the override only if it exists
            var animator = GetComponent<Animator>();
            if (animator != null && animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (!control.enabled) return;

            var player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                // FORCE ANIMATION INSTANTLY
                var animator = GetComponent<Animator>();
                if (animator != null)
                {
                    animator.Play("hurt");
                }

                var ev = Schedule<PlayerEnemyCollision>();
                ev.player = player;
                ev.enemy = this;
            }
        }

        void Update()
        {
            if (path != null && control.enabled)
            {
                if (mover == null) mover = path.CreateMover(control.maxSpeed * 0.5f);
                control.move.x = Mathf.Clamp(mover.Position.x - transform.position.x, -1, 1);
            }
        }

        public void ResetEnemy()
        {
            // 1. Teleport and Reactivate
            transform.position = _initialPosition;
            gameObject.SetActive(true);

            // 2. STOP FALLING (Reset velocity)
            var rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            // 3. Re-enable Physics and Control
            if (_collider != null) _collider.enabled = true;
            if (control != null)
            {
                control.enabled = true;
                control.move = Vector2.zero;
            }

            // 4. Reset Animation Safely
            var animator = GetComponent<Animator>();
            // Check if animator exists AND is ready to avoid the NullRef error
            if (animator != null && animator.runtimeAnimatorController != null)
            {
                animator.ResetTrigger("hurt");
                // Play from the beginning of the default state
                animator.Play(0, -1, 0f);
            }

            // 5. Reset Path
            mover = null;
        }
    }
}