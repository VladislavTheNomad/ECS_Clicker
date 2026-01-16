using Leopotam.EcsLite;
using UnityEngine;

namespace ECSTest
{
    public class PlayerInputSystem : IEcsRunSystem
    {
        
        public void Run(IEcsSystems systems)
        {
            /*var world = systems.GetWorld();

            var inputPool = world.GetPool<InputComponent>();
            var filter = world.Filter<InputComponent>().End();
            
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            
            Vector2 direction = new Vector2(x, y).normalized;
            
            foreach (var entity in filter)
            {
                ref var input = ref inputPool.Get(entity);
                input.Direction = direction;
            } */
        }
    }
}