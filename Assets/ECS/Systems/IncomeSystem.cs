using Leopotam.EcsLite;
using UnityEngine;

namespace ECSTest
{
    public class IncomeSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var businessConfigPool = world.GetPool<BusinessConfigReference>();
            var businessDataPool = world.GetPool<BusinessDataComponent>();
            var businessFilter = world.Filter<BusinessConfigReference>().Inc<BusinessDataComponent>().End();
            
            var playerDataPool = world.GetPool<PlayerDataComponent>();
            var playerFilter = world.Filter<PlayerDataComponent>().End();

            foreach (var entityBusiness in businessFilter)
            {
                ref var data = ref businessDataPool.Get(entityBusiness);

                ref var businessConfig = ref businessConfigPool.Get(entityBusiness);
                var config = businessConfig.Config;
                
                if (data.Level <= 0) continue;
                
                data.CurrentProgress += Time.deltaTime;

                if (data.CurrentProgress >= config.timeToIncome)
                {
                    data.CurrentProgress = 0f;
                    
                    float multiplier = 1f;
                    
                    if (data.HasUpgrade1)
                    {
                        multiplier += config.upgrade1Modificator;
                    }

                    if (data.HasUpgrade2)
                    {
                        multiplier += config.upgrade2Modificator;
                    }
                    
                    int income = (int)(data.Level * config.baseIncome * multiplier);

                    foreach (var entity in playerFilter)
                    {
                        ref var playerData = ref playerDataPool.Get(entity);
                        playerData.Balance += income;
                        Debug.Log($"Player's balance now: {playerData.Balance}");
                    }
                    
                    Debug.Log($"Business {config.name} earned {income}$. Player Balance updated.");
                }
            }
        }
    }
}