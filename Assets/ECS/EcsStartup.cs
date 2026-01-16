using ECSTest.ECS.Data;
using Leopotam.EcsLite;
using UnityEngine;

namespace ECSTest.ECS
{
    public class EcsStartup : MonoBehaviour
    {
        [SerializeField] private PlayerInitData playerData;
        [SerializeField] private BusinessConfig[] businessConfigs;
        
        private EcsSystems _systems;
        private EcsWorld _world;
        

        private void Start()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);

            _systems
                .Add(new GameInitSystem(playerData, businessConfigs))
                .Add(new IncomeSystem())
                .Init();
        }
        
        private void Update()
        {
            _systems?.Run();
        }

        private void OnDestroy()
        {
            _systems.Destroy();
            _world.Destroy();
        }
    }
}