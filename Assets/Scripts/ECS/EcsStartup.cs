using ECSTest.ECS.Data;
using Leopotam.EcsLite;
using UnityEngine;

namespace ECSTest.ECS
{
    public class EcsStartup : MonoBehaviour
    {
        [SerializeField] private UIView _uiView;
        [SerializeField] private GameObject _scrollViewContent;
        [SerializeField] private GameObject _businessCard;
        [SerializeField] private PlayerInitData _playerData;
        [SerializeField] private BusinessConfig[] _businessConfigs;
        [SerializeField] private BusinessNames[] _businessNames;
        
        private SaveService _saveService;
        private EcsSystems _systems;
        private EcsWorld _world;

        private void Start()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
            
            _saveService = new SaveService(_world, _businessConfigs.Length);
            
            _systems
                .Add(new GameInitSystem(_playerData, _businessConfigs, _businessNames, _saveService))
                .Add(new IncomeSystem())
                .Add(new UpdateDataSystem())
                .Add(new UpdateUISystem(_uiView))
                .Init();
            
            CreateBusinessCards();
        }
        
        private void CreateBusinessCards()
        {
            var cfgPool = _world.GetPool<BusinessConfigReference>();
            
            var filter = _world.Filter<BusinessDataComponent>()
                .Inc<BusinessConfigReference>()
                .End();

            foreach (var entity in filter)
            {
                var go = Instantiate(_businessCard, _scrollViewContent.transform);
                var view = go.GetComponent<BusinessCardView>();
                view.Init(_world, entity);
                var config = cfgPool.Get(entity).Config;
                var names = cfgPool.Get(entity).BusinessNames;
                
                _uiView.RegisterNewBusiness(entity, view, config, names);
            }
        }
        
        private void Update()
        {
            _systems?.Run();
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                _saveService?.SaveGame();
            }
        }

        private void OnApplicationQuit()
        {
            _saveService?.SaveGame();
        }

        private void OnDestroy()
        {
            _saveService?.SaveGame();
            _systems?.Destroy();
            _world?.Destroy();
        }
    }
}