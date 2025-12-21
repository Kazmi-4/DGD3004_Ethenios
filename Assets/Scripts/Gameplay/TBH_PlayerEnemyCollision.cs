using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when a Player collides with an Enemy (TBH version).
    /// </summary>
    public class TBH_PlayerEnemyCollision : Simulation.Event<TBH_PlayerEnemyCollision>
    {
        public EnemyController enemy;
        public PlayerController player;

        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            bool willHurtEnemy = player.Bounds.center.y >= enemy.Bounds.max.y;

            if (willHurtEnemy)
            {
                Health enemyHealth = enemy.GetComponent<Health>();

                if (enemyHealth != null)
                {
                    enemyHealth.Decrement();

                    if (!enemyHealth.IsAlive)
                    {
                        Simulation.Schedule<EnemyDeath>().enemy = enemy;
                        player.Bounce(2f);
                    }
                    else
                    {
                        player.Bounce(7f);
                    }
                }
                else
                {
                    Simulation.Schedule<EnemyDeath>().enemy = enemy;
                    player.Bounce(2f);
                }
            }
            else
            {
                Simulation.Schedule<PlayerDeath>();
            }
        }
    }
}
