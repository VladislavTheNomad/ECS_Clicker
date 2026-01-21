using ECSTest.ECS.Data;
using Leopotam.EcsLite;

namespace ECSTest
{
    public class GameInitSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private EcsPool<PlayerDataComponent> _playerDataPool;
        private EcsPool<BusinessConfigReference> _businessConfigPool;
        private EcsPool<BusinessDataComponent> _businessDataComponentPool;
        private EcsPool<BusinessUiDirtyTag> _businessInfoChangedPool;
        
        private readonly PlayerInitData _playerInitData;
        private readonly BusinessConfig[] _businessConfigs;
        
        public GameInitSystem(PlayerInitData playerInitData, BusinessConfig[] businessConfigs)
        {
            _playerInitData = playerInitData;
            _businessConfigs = businessConfigs;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _playerDataPool = _world.GetPool<PlayerDataComponent>();
            
            int player = _world.NewEntity();
            
            ref var playerData = ref _playerDataPool.Add(player);
            playerData.Balance = _playerInitData.defaultBalance;
            
            _businessConfigPool =  _world.GetPool<BusinessConfigReference>();
            _businessDataComponentPool = _world.GetPool<BusinessDataComponent>();

            for (int i = 0; i < _businessConfigs.Length; i++)
            {
                var entity = _world.NewEntity();
                
                ref var businessConfig = ref _businessConfigPool.Add(entity);
                businessConfig.Config = _businessConfigs[i];
                
                ref var data = ref _businessDataComponentPool.Add(entity);
                
                data.Level = i == 0 ? 1 : 0;
                data.CurrentIncome = businessConfig.Config.baseIncome;
                data.CurrentLevelUpCost = (data.Level + 1) * businessConfig.Config.baseCost;
                data.ExpiredTime = 0f;
                data.ProgressTime = 0f;
                data.HasUpgrade1 = false;
                data.HasUpgrade2 = false;

                _businessInfoChangedPool = _world.GetPool<BusinessUiDirtyTag>();
                _businessInfoChangedPool.Add(entity);
            }
        }
    }
} 