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
        
        private EcsSystems _systems;
        private EcsWorld _world;

        private void CreateBusinessCards()
        {
            var dataPool = _world.GetPool<BusinessDataComponent>();
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
                uiView.RegisterNewBusiness(entity, view, config);
            }
            
            /*for (int i = 1;  i < businessConfigs.Length+1; i++)
            {
                var business = Instantiate(businessCard, scrollViewContent.transform);
                var businessView = business.GetComponent<BusinessCardView>();
                businessView.Init(_world, i);
                uiView.RegisterNewBusiness(i, businessView, businessConfigs[i-1]);
            }*/
        }

        private void Start()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
            
            _systems
                .Add(new GameInitSystem(playerData, businessConfigs))
                .Add(new IncomeSystem())
                .Add(new UpdateUISystem(uiView))
                .Init();
            
            CreateBusinessCards();

        }
        
        private void Update()
        {
            _systems?.Run();
        }

        private void OnDestroy()
        {
            _systems?.Destroy();
            _world?.Destroy();
        }
    }
}