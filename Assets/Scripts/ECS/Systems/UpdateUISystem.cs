using Leopotam.EcsLite;

namespace ECSTest
{
    public class UpdateUISystem : IEcsRunSystem
    {
        private readonly UIView _uiView;

        public UpdateUISystem(UIView  uiView)
        {
            _uiView = uiView;
        }
        
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            
            var evtPoolBalance = world.GetPool<BalanceChangedEvent>();
            var filterBalance =  world.Filter<BalanceChangedEvent>().End();

            foreach (var entity in filterBalance)
            {
                _uiView.SetBalance(evtPoolBalance.Get(entity).NewBalance);
                world.DelEntity(entity);
            }

            var businessDataPool = world.GetPool<BusinessDataComponent>();
            var dirtyPool = world.GetPool<BusinessUiDirtyTag>();
            var dirtyFilter =  world.Filter<BusinessUiDirtyTag>().Inc<BusinessDataComponent>().End();

            foreach (var entity in dirtyFilter)
            {
                _uiView.SetBusinessLevel(entity, businessDataPool.Get(entity).Level);
                
                dirtyPool.Del(entity);
            }
        }
    }
}