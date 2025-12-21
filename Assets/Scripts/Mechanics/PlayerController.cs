using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;
using UnityEngine.InputSystem;

namespace Platformer.Mechanics
{
    /// <summary>
    /// Main player controller (2D platformer) with double jump + external forces (wind/conveyors).
    /// </summary>
    public class PlayerController : KinematicObject
    {
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;

        // Movement
        public float maxSpeed = 7;
        public float jumpTakeOffSpeed = 7;

        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;

        /*internal new*/
        public Collider2D collider2d;
        /*internal new*/
        public AudioSource audioSource;

        public Health health;
        public bool controlEnabled = true;

        bool jump;
        Vector2 move;

        SpriteRenderer spriteRenderer;
        internal Animator animator;

        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        private InputAction m_MoveAction;
        private InputAction m_JumpAction;

        public Bounds Bounds => collider2d.bounds;

        // -----------------------------
        // DOUBLE JUMP
        // -----------------------------
        [Header("Double Jump")]
        [Tooltip("How many extra jumps allowed while airborne (1 = classic double jump).")]
        public int extraAirJumps = 1;

        [Tooltip("Vertical speed applied for air jumps (leave 0 to use jumpTakeOffSpeed).")]
        public float airJumpVelocity = 0f;

        public AudioClip doubleJumpAudio;

        int airJumpsRemaining;
        bool requestAirJump;
        bool wasGrounded;

        // -----------------------------
        // EXTERNAL FORCES (wind/conveyors/etc.)
        // -----------------------------
        [Header("External Forces")]
        [Tooltip("Additive horizontal/vertical motion per frame applied by zones (set each frame while inside).")]
        public Vector2 externalMove = Vector2.zero;

        [Tooltip("How fast externalMove decays back to zero when not being set (units per second).")]
        public float externalDecay = 5f;

        /// <summary>
        /// Convenience: nudge external motion this frame.
        /// </summary>
        public void AddExternalMove(Vector2 amount) => externalMove += amount;

        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

            m_MoveAction = InputSystem.actions.FindAction("Player/Move");
            m_JumpAction = InputSystem.actions.FindAction("Player/Jump");

            m_MoveAction?.Enable();
            m_JumpAction?.Enable();

            airJumpsRemaining = extraAirJumps;

            if (airJumpVelocity <= 0f)
                airJumpVelocity = jumpTakeOffSpeed;

            // IMPORTANT: register this object as the player in the model
            model.player = this;
        }

        protected override void Update()
        {
            if (controlEnabled)
            {
                move.x = m_MoveAction != null
                    ? m_MoveAction.ReadValue<Vector2>().x
                    : 0f;

                // Ground jump input
                if (jumpState == JumpState.Grounded &&
                    m_JumpAction != null &&
                    m_JumpAction.WasPressedThisFrame())
                {
                    jumpState = JumpState.PrepareToJump;
                }
                else if (m_JumpAction != null &&
                         m_JumpAction.WasReleasedThisFrame())
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }

                // Air jump request (double jump)
                if (!IsGrounded &&
                    m_JumpAction != null &&
                    m_JumpAction.WasPressedThisFrame() &&
                    airJumpsRemaining > 0)
                {
                    requestAirJump = true;
                }
            }
            else
            {
                move.x = 0;
            }

            UpdateJumpState();

            // Reset air jumps when we touch ground
            if (IsGrounded && !wasGrounded)
            {
                airJumpsRemaining = extraAirJumps;
            }

            wasGrounded = IsGrounded;

            base.Update();
        }

        void UpdateJumpState()
        {
            jump = false;

            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;

                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;

                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    }
                    break;

                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    break;
            }
        }

        protected override void ComputeVelocity()
        {
            // Grounded jump impulse / variable jump
            if (jump && IsGrounded)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                    velocity.y *= model.jumpDeceleration;
            }

            // Perform double/air jump if requested
            if (requestAirJump && airJumpsRemaining > 0)
            {
                requestAirJump = false;
                airJumpsRemaining--;

                float vY = airJumpVelocity * model.jumpModifier;
                velocity.y = Mathf.Max(velocity.y, vY);

                if (audioSource && doubleJumpAudio)
                    audioSource.PlayOneShot(doubleJumpAudio);

                if (animator)
                {
                    animator.ResetTrigger("hurt");
                    animator.SetTrigger("doubleJump");
                }
            }

            // Facing visuals
            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;

            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            // Base desired horizontal velocity from input
            Vector2 desired = move * maxSpeed;

            // Apply external forces (wind/conveyors). Decay smoothly toward zero.
            if (externalMove.sqrMagnitude > 0.0001f)
            {
                desired += externalMove;
                externalMove = Vector2.MoveTowards(
                    externalMove,
                    Vector2.zero,
                    externalDecay * Time.deltaTime
                );
            }

            // Commit target velocity (KinematicObject handles integration)
            targetVelocity = desired;
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }
    }
}
