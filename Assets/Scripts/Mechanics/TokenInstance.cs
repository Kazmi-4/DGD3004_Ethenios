using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    [RequireComponent(typeof(Collider2D))]
    public class TokenInstance : MonoBehaviour
    {
        public AudioClip tokenCollectAudio;

        [Tooltip("If true, animation will start at a random position in the sequence.")]
        public bool randomAnimationStartTime = false;

        [Tooltip("List of frames that make up the animation.")]
        public Sprite[] idleAnimation, collectedAnimation;

        internal Sprite[] sprites = new Sprite[0];
        internal SpriteRenderer _renderer;

        internal int tokenIndex = -1;
        internal TokenController controller;
        internal int frame = 0;
        internal bool collected = false;

        void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            // Initialize with idle sprites
            sprites = idleAnimation;

            if (randomAnimationStartTime && sprites.Length > 0)
                frame = Random.Range(0, sprites.Length);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            var player = other.gameObject.GetComponent<PlayerController>();
            if (player != null) OnPlayerEnter(player);
        }

        void OnPlayerEnter(PlayerController player)
        {
            if (collected) return;

            // Switch to collected animation sprites
            frame = 0;
            sprites = collectedAnimation;
            collected = true;

            var ev = Schedule<PlayerTokenCollision>();
            ev.token = this;
            ev.player = player;

            // Note: If you want the "collected" animation to play before vanishing, 
            // you'd usually wait. But here we disable it immediately.
            gameObject.SetActive(false);
        }

        public void ResetToken()
        {
            // 1. Reset logic state
            collected = false;

            // 2. Point back to the idle sheet
            sprites = idleAnimation;

            // 3. Reset the frame counter to start the animation over
            if (randomAnimationStartTime && sprites.Length > 0)
                frame = Random.Range(0, sprites.Length);
            else
                frame = 0;

            // 4. Force the renderer to show the first idle frame immediately
            if (_renderer != null)
            {
                _renderer.enabled = true; // Ensure renderer is on
                if (sprites.Length > 0)
                    _renderer.sprite = sprites[frame];
            }

            // 5. Turn the object back on
            gameObject.SetActive(true);
        }
    }
}