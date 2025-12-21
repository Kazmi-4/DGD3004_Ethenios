using Platformer.Core;
using Platformer.Model;
using Platformer.Mechanics;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player has died.
    /// </summary>
    public class PlayerDeath : Simulation.Event<PlayerDeath>
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            var player = model.player;
            if (player == null || !player.health.IsAlive)
                return;

            // 💀 PLAYER DEATH
            player.health.Die();
            player.controlEnabled = false;

            if (model.virtualCamera != null)
            {
                model.virtualCamera.Follow = null;
                model.virtualCamera.LookAt = null;
            }

            if (player.audioSource && player.ouchAudio)
                player.audioSource.PlayOneShot(player.ouchAudio);

            if (player.animator != null)
            {
                player.animator.SetTrigger("hurt");
                player.animator.SetBool("dead", true);
            }

            // 🔁 RESET TOKENS (Include Inactive Objects)
            // We use FindObjectsInactive.Include because collected tokens are usually disabled.
            var tokens = Object.FindObjectsByType<TokenInstance>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

            foreach (var token in tokens)
            {
                if (token != null)
                    token.ResetToken();
            }

            // 🔁 RESET ENEMIES
            var enemies = Object.FindObjectsByType<EnemyController>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

            foreach (var enemy in enemies)
            {
                if (enemy != null)
                    enemy.ResetEnemy();
            }

            // ⏳ RESPAWN PLAYER
            Simulation.Schedule<PlayerSpawn>(2f);
        }
    }
}