using Leopotam.EcsLite;

namespace ECSTest
{
    public class ButtonsClickedSystem : IEcsRunSystem
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

                if(businessDataPool.Has(businessEntity) && cfgPool.Has(businessEntity))
                {
                    ref var businessData = ref businessDataPool.Get(businessEntity);
                    var businessCfgRef = cfgPool.Get(businessEntity);

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
                    businessData.CurrentIncome = businessCfgRef.Config.baseIncome * businessData.Level;
                    businessData.CurrentLevelUpCost = businessCfgRef.Config.baseCost * (businessData.Level + 1);

                    if (!dirtyPool.Has(businessEntity))
                    {
                        dirtyPool.Add(businessEntity);
                    }
                    
                    world.DelEntity(entity);
                }
            }
        }
    }
}