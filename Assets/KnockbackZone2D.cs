using UnityEngine;
using Platformer.Mechanics; // for PlayerController

[RequireComponent(typeof(Collider2D))]
public class KnockbackZone2D : MonoBehaviour
{
    [Header("Knockback")]
    [Tooltip("How hard to push left on enter (negative X).")]
    public float pushX = -8f;     // negative = push left
    [Tooltip("Optional extra vertical kick (e.g., small pop).")]
    public float pushY = 0f;      // keep 0 for pure horizontal

    [Header("Filter")]
    public bool onlyAffectPlayer = true;

    void Reset()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true; // make sure it's a trigger
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var pc = other.GetComponentInParent<PlayerController>();
        if (onlyAffectPlayer && pc == null) return;

        if (pc != null)
        {
            // One-shot push when entering the zone.
            // PlayerController will add this to targetVelocity and decay it with externalDecay.
            pc.externalMove = new Vector2(pushX, pushY);
        }
    }
}
