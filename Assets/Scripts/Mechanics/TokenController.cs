using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This class animates all token instances in a scene.
    /// </summary>
    public class TokenController : MonoBehaviour
    {
        [Tooltip("Frames per second at which tokens are animated.")]
        public float frameRate = 12;
        [Tooltip("Instances of tokens which are animated. If empty, token instances are found and loaded at runtime.")]
        public TokenInstance[] tokens;

        float nextFrameTime = 0;

        [ContextMenu("Find All Tokens")]
        void FindAllTokensInScene()
        {
            // Using Include ensures we find tokens even if they are currently disabled in the editor
            tokens = Object.FindObjectsByType<TokenInstance>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        }

        void Awake()
        {
            if (tokens == null || tokens.Length == 0)
                FindAllTokensInScene();

            for (var i = 0; i < tokens.Length; i++)
            {
                if (tokens[i] != null)
                {
                    tokens[i].tokenIndex = i;
                    tokens[i].controller = this;
                }
            }
        }

        void Update()
        {
            if (Time.time - nextFrameTime > (1f / frameRate))
            {
                for (var i = 0; i < tokens.Length; i++)
                {
                    var token = tokens[i];

                    // Logic: Only animate if the token exists and is currently active (respawned)
                    if (token != null && token.gameObject.activeInHierarchy)
                    {
                        token._renderer.sprite = token.sprites[token.frame];

                        if (token.collected && token.frame == token.sprites.Length - 1)
                        {
                            // Disable the object when the collection animation finishes
                            token.gameObject.SetActive(false);

                            // IMPORTANT: We NO LONGER set tokens[i] = null here. 
                            // This allows the token to stay in the list so it can be animated again after respawn.
                        }
                        else
                        {
                            // Cycle through the sprite array
                            token.frame = (token.frame + 1) % token.sprites.Length;
                        }
                    }
                }
                nextFrameTime += 1f / frameRate;
            }
        }
    }
}