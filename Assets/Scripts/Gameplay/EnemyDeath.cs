using Platformer.Core;
using Platformer.Mechanics;
using UnityEngine;

namespace Platformer.Gameplay
{
    public class EnemyDeath : Simulation.Event<EnemyDeath>
    {
        public EnemyController enemy;

        public override void Execute()
        {
            // 1. Kill physics and movement
            enemy._collider.enabled = false;
            enemy.control.enabled = false;

            // 2. Play Sound
            if (enemy._audio && enemy.ouch)
                enemy._audio.PlayOneShot(enemy.ouch);

            // 3. Trigger Animation (Using GetComponent to avoid protection level error)
            var animator = enemy.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("hurt");
            }

            // 4. Remove SetActive(false) so the animation actually plays
            // enemy.gameObject.SetActive(false); 
        }
    }
}