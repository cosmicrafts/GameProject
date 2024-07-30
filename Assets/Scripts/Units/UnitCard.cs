using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class EnergySystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        // Update player energy
        Entities.ForEach((ref PlayerComponent player) =>
        {
            if (player.CurrentEnergy < player.MaxEnergy)
            {
                player.CurrentEnergy += player.SpeedEnergy * deltaTime;
            }
        }).Schedule();

        // Update bot enemy energy
        Entities.ForEach((ref BotEnemyComponent bot) =>
        {
            if (bot.CurrentEnergy < bot.MaxEnergy)
            {
                bot.CurrentEnergy += bot.SpeedEnergy * deltaTime;
            }
        }).Schedule();
    }
}

public class HealthSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref HealthComponent health, in PlayerComponent player) =>
        {
            // Example logic for updating health
            if (health.HitPoints <= 0 && !health.IsDeath)
            {
                health.IsDeath = true;
                Debug.Log("Player is dead");
            }
        }).Schedule();

        Entities.ForEach((ref HealthComponent health, in BotEnemyComponent bot) =>
        {
            // Example logic for updating health
            if (health.HitPoints <= 0 && !health.IsDeath)
            {
                health.IsDeath = true;
                Debug.Log("Bot is dead");
            }
        }).Schedule();
    }
}
