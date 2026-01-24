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
        
        private readonly SaveService _saveService;
        private readonly PlayerInitData _playerInitData;
        private readonly BusinessConfig[] _businessConfigs;
        private readonly BusinessNames[] _businessNames;
        
        public GameInitSystem(PlayerInitData playerInitData, BusinessConfig[] businessConfigs, BusinessNames[] businessNames, SaveService saveService)
        {
            _playerInitData = playerInitData;
            _businessConfigs = businessConfigs;
            _businessNames = businessNames;
            _saveService = saveService;
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

            BusinessSaveData[] businessDataArray = null;

            if (_saveService.TryLoad(out SaveData saveData))
            {
                playerData.Balance = saveData.Balance;
                
                var evt = _world.NewEntity();
                var balanceChangedPool = _world.GetPool<BalanceChangedEvent>();
                ref var balanceChanged = ref balanceChangedPool.Add(evt);
                balanceChanged.NewBalance = playerData.Balance;
                
                businessDataArray = saveData.BusinessSaveDataArray;
            }

            for (int i = 0; i < _businessConfigs.Length; i++)
            {
                var entity = _world.NewEntity();
                
                ref var businessConfig = ref _businessConfigPool.Add(entity);
                businessConfig.Config = _businessConfigs[i];
                businessConfig.BusinessNames = _businessNames[i];
                
                ref var data = ref _businessDataComponentPool.Add(entity);

                if (businessDataArray != null && i < businessDataArray.Length && businessDataArray[i] != null)
                {
                    var businessData = businessDataArray[i];
                    
                    data.Index = i;
                    data.Level = businessData.Level;
                    data.CurrentIncome = businessData.CurrentIncome;
                    data.CurrentLevelUpCost = (data.Level + 1) * businessConfig.Config.baseCost;
                    data.ExpiredTime = businessData.ExpiredTime;
                    data.ProgressTime = businessData.ProgressTime;
                    data.HasUpgrade1 = businessData.HasUpgrade1;
                    data.HasUpgrade2 = businessData.HasUpgrade2;
                }
                else
                {
                    data.Index = i;
                    data.Level = i == 0 ? 1 : 0;
                    data.CurrentIncome = businessConfig.Config.baseIncome;
                    data.CurrentLevelUpCost = (data.Level + 1) * businessConfig.Config.baseCost;
                    data.ExpiredTime = 0f;
                    data.ProgressTime = 0f;
                    data.HasUpgrade1 = false;
                    data.HasUpgrade2 = false;
                }

                _businessInfoChangedPool = _world.GetPool<BusinessUiDirtyTag>();
                _businessInfoChangedPool.Add(entity);
            }
        }
    }
} 