using System.IO;
using Leopotam.EcsLite;
using UnityEngine;

namespace ECSTest
{
    
    public class SaveService 
    {
        private readonly string _filePath;
        private readonly EcsWorld _world;
        private readonly int _numberOfBusinesses;

        public SaveService(EcsWorld world, int numberOfBusinesses, string filePath = "save.json")
        {
            _world =  world;
            _numberOfBusinesses = numberOfBusinesses;
            _filePath = Path.Combine(Application.persistentDataPath, filePath);
        }
        
        public bool TryLoad(out SaveData saveData)
        {
            saveData = null;

            if (!File.Exists(_filePath))
            {
                return false;
            }
            
            string json = File.ReadAllText(_filePath);
            saveData = JsonUtility.FromJson<SaveData>(json);
            
            return saveData != null;
        }
        
        public void SaveGame()
        {
            if (_world == null) return;

            var save = new SaveData()
            {
                balance = ReadPlayerBalance(),
                businessSaveData = ReadBusinessSaveData(),
            };
            
            string json = JsonUtility.ToJson(save);
            File.WriteAllText(_filePath, json);
        }

        private BusinessSaveData[] ReadBusinessSaveData()
        {
            var result = new BusinessSaveData[_numberOfBusinesses];
            
            var businessPool = _world.GetPool<BusinessDataComponent>();
            var businessFilter = _world.Filter<BusinessDataComponent>().End();

            foreach (var entity in businessFilter)
            {
                var data = businessPool.Get(entity);
                int index = data.Index;

                if (index < 0 || index >= result.Length)
                {
                    continue;
                }

                result[index] = new BusinessSaveData()
                {
                    level = data.Level,
                    currentIncome = data.CurrentIncome,
                    hasUpgrade1 = data.HasUpgrade1,
                    hasUpgrade2 = data.HasUpgrade2,
                    expiredTime = data.ExpiredTime,
                    progressTime = data.ProgressTime,
                };
            }
            
            for (int i = 0; i < result.Length; i++)
            {
                if (result[i] == null)
                {
                    result[i] = new BusinessSaveData();
                }
            }
            
            return result;
        }

        private int ReadPlayerBalance()
        {
            var playerPool = _world.GetPool<PlayerDataComponent>();
            var playerFilter = _world.Filter<PlayerDataComponent>().End();

            foreach (var entity in playerFilter)
            {
                return playerPool.Get(entity).Balance;
            }

            return -1;
        }
    }
}