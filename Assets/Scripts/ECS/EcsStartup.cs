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

        private void Awake()
        {
            for (int i = 0;  i < businessConfigs.Length; i++)
            {
                var business = Instantiate(businessCard, scrollViewContent.transform);
                var businessView = business.GetComponent<BusinessCardView>();
                uiView.RegisterNewBusiness(i, businessView);
            }
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