using UnityEngine;
using Platformer.Core;
using Platformer.Model;

public class Check_point : MonoBehaviour
{
    PlatformerModel model;

    void Awake()
    {
        model = Simulation.GetModel<PlatformerModel>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // player check (NO PlayerController dependency)
        if (other.CompareTag("Player"))
        {
            // 🔑 MOVE the existing spawn point
            model.spawnPoint.position = transform.position;
        }
    }
}
