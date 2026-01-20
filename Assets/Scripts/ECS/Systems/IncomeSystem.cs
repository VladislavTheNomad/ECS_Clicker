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
            
            var balanceChangedPool = world.GetPool<BalanceChangedEvent>();

            foreach (var entityBusiness in businessFilter)
            {
                ref var data = ref businessDataPool.Get(entityBusiness);

                ref var businessConfig = ref businessConfigPool.Get(entityBusiness);
                var config = businessConfig.Config;
                
                if (data.Level <= 0) continue;
                
                data.ExpiredTime += Time.deltaTime;
                data.ProgressTime = Mathf.Clamp01(data.ExpiredTime/config.timeToIncome);

                if (data.ExpiredTime >= config.timeToIncome)
                {
                    data.ExpiredTime = 0f;
                    
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
                        
                        int e = world.NewEntity();
                        ref var evt =  ref balanceChangedPool.Add(e);
                        evt.NewBalance = playerData.Balance;
                    }
                }
            }
        }
    }
}