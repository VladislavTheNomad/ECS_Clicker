using ECSTest.ECS.Data;
using Leopotam.EcsLite;
using UnityEngine;

namespace ECSTest.ECS
{
    public class EcsStartup : MonoBehaviour
    {
        [SerializeField] private UIView uiView;
        [SerializeField] private GameObject scrollViewContent;
        [SerializeField] private GameObject businessCard;
        [SerializeField] private PlayerInitData playerData;
        [SerializeField] private BusinessConfig[] businessConfigs;
        [SerializeField] private BusinessNames[] businessNames;
        
        private SaveService _saveService;
        private EcsSystems _systems;
        private EcsWorld _world;

        private void Start()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
            
            _saveService = new SaveService(_world, businessConfigs.Length);
            
            _systems
                .Add(new GameInitSystem(playerData, businessConfigs, businessNames, _saveService))
                .Add(new IncomeSystem())
                .Add(new UpdateDataSystem())
                .Add(new UpdateUISystem(uiView))
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
                var go = Instantiate(businessCard, scrollViewContent.transform);
                var view = go.GetComponent<BusinessCardView>();
                view.Init(_world, entity);
                var config = cfgPool.Get(entity).Config;
                var names = cfgPool.Get(entity).BusinessNames;
                
                uiView.RegisterNewBusiness(entity, view, config, names);
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