using ECSTest.ECS.Data;
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
            
            var businessDataPool = world.GetPool<BusinessDataComponent>();
            
            var evtPoolBalance = world.GetPool<BalanceChangedEvent>();
            var filterBalance =  world.Filter<BalanceChangedEvent>().End();

            foreach (var entity in filterBalance)
            {
                _uiView.SetBalance(evtPoolBalance.Get(entity).NewBalance);
                world.DelEntity(entity);
            }
            
            var filterBusinessData = world.Filter<BusinessDataComponent>().End();

            foreach (var entity in filterBusinessData)
            {
                if (businessDataPool.Get(entity).Level > 0)
                {
                    _uiView.UpdateProgressBar(entity,  businessDataPool.Get(entity).ProgressTime);
                }
            }
            
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