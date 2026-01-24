using Leopotam.EcsLite;

namespace ECSTest
{
    public class UpdateDataSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            
            var playerDataPool = world.GetPool<PlayerDataComponent>();
            var playerFilter = world.Filter<PlayerDataComponent>().End();
            
            int playerEntity = -1;

            foreach (var player in playerFilter)
            {
                playerEntity = player;
                break;
            }
            if (playerEntity == -1) return;
            
            ref var playerData = ref playerDataPool.Get(playerEntity);
            
            HandleLevelUp(world, ref playerData);
            HandleUpgrade(world, ref playerData);
        }

        private void HandleUpgrade(EcsWorld world, ref PlayerDataComponent playerData)
        {
            var upgradeEventPool = world.GetPool<UpgradeEvent>();
            var config = world.GetPool<BusinessConfigReference>();
            var dirtyPool = world.GetPool<BusinessUiDirtyTag>();
            var dataPool = world.GetPool<BusinessDataComponent>();
            var balanceChangedPool = world.GetPool<BalanceChangedEvent>();
            var filterUpgrade = world.Filter<UpgradeEvent>().End();

            foreach (var entity in filterUpgrade)
            {
                var businessEntity = upgradeEventPool.Get(entity).BusinessEntity;
                var upgradeIndex = upgradeEventPool.Get(entity).upgradeIndex;

                if (!dataPool.Has(businessEntity) || !config.Has(businessEntity)) continue;
                
                ref var data = ref dataPool.Get(businessEntity);
                var cnfg = config.Get(businessEntity);

                if (upgradeIndex == 1)
                {
                    if (data.HasUpgrade1 || cnfg.Config.upgrade1Cost > playerData.Balance)
                    {
                        world.DelEntity(entity);
                        continue;
                    }
                    
                    playerData.Balance -= cnfg.Config.upgrade1Cost;
                    
                    int e = world.NewEntity();
                    ref var evt = ref balanceChangedPool.Add(e);
                    evt.NewBalance = playerData.Balance;
                    
                    data.HasUpgrade1 = true;
                    CalculateNewIncome(ref data, ref cnfg);
                }
                else if (upgradeIndex == 2)
                {
                    if (data.HasUpgrade2 || cnfg.Config.upgrade2Cost > playerData.Balance)
                    {
                        world.DelEntity(entity);
                        continue;
                    }
                    
                    playerData.Balance -= cnfg.Config.upgrade2Cost;
                    
                    int e = world.NewEntity();
                    ref var evt = ref balanceChangedPool.Add(e);
                    evt.NewBalance = playerData.Balance;
                    
                    data.HasUpgrade2 = true;
                    CalculateNewIncome(ref data, ref cnfg);
                }
                    
                if (!dirtyPool.Has(businessEntity))
                {
                    dirtyPool.Add(businessEntity);
                }
                
                world.DelEntity(entity);
            }
        }

        private void HandleLevelUp(EcsWorld world, ref PlayerDataComponent playerData)
        {
            var buttonClickedEventPool = world.GetPool<LevelUpEvent>();
            var businessDataPool = world.GetPool<BusinessDataComponent>();
            var cfgPool = world.GetPool<BusinessConfigReference>();
            var dirtyPool = world.GetPool<BusinessUiDirtyTag>();
            var balanceChangedPool = world.GetPool<BalanceChangedEvent>();
            var filterLevelUp = world.Filter<LevelUpEvent>().End();

            foreach (var entity in filterLevelUp)
            {
                int businessEntity = buttonClickedEventPool.Get(entity).BusinessEntity;

                if (!businessDataPool.Has(businessEntity) || !cfgPool.Has(businessEntity)) continue;
                
                ref var businessData = ref businessDataPool.Get(businessEntity);
                var config = cfgPool.Get(businessEntity);

                if (playerData.Balance < businessData.CurrentLevelUpCost)
                {
                    world.DelEntity(entity);
                    continue;
                }

                playerData.Balance -= businessData.CurrentLevelUpCost;
                        
                int e = world.NewEntity();
                ref var evt = ref balanceChangedPool.Add(e);
                evt.NewBalance = playerData.Balance;

                businessData.Level += 1;
                CalculateNewIncome(ref businessData, ref config);
                
                businessData.CurrentLevelUpCost = config.Config.baseCost * (businessData.Level + 1);

                if (!dirtyPool.Has(businessEntity))
                {
                    dirtyPool.Add(businessEntity);
                }
                    
                world.DelEntity(entity);
            }
        }
        
        private void CalculateNewIncome(ref BusinessDataComponent data, ref BusinessConfigReference cnfg)
        {
            float multiplier = 1f;
                    
            if (data.HasUpgrade1)
            {
                multiplier += cnfg.Config.upgrade1Modificator;
            }

            if (data.HasUpgrade2)
            {
                multiplier += cnfg.Config.upgrade2Modificator;
            }
                    
            data.CurrentIncome = (int)(data.Level * cnfg.Config.baseIncome * multiplier);
        }
    }
}