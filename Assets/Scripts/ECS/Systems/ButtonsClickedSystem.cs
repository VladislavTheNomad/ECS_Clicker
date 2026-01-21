using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECSTest.ECS.Data;
using Leopotam.EcsLite;

namespace ECSTest
{
    public class ButtonsClickedSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var buttonClickedEventPool = world.GetPool<LevelUpEvent>();
            var businessDataPool = world.GetPool<BusinessDataComponent>();
            var cfgPool = world.GetPool<BusinessConfigReference>();
            var dirtyPool = world.GetPool<BusinessUiDirtyTag>();
            var filterLevelUp = world.Filter<LevelUpEvent>().End();

            foreach (var entity in filterLevelUp)
            {
                int businessEntity = buttonClickedEventPool.Get(entity).BusinessEntity;

                if(businessDataPool.Has(businessEntity))
                {
                    ref var businessData = ref businessDataPool.Get(businessEntity);
                    var businessCfgRef = cfgPool.Get(businessEntity);

                    businessData.Level += 1;
                    businessData.CurrentIncome = businessCfgRef.Config.baseIncome * businessData.Level;

                    if (!dirtyPool.Has(businessEntity))
                    dirtyPool.Add(businessEntity);

                    Debug.Log($"Business {businessEntity} leveled up to {businessData.Level}");

                }

                world.DelEntity(entity);
            }
        }
    }
}
