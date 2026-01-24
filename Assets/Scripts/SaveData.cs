using System;

namespace ECSTest
{
    [Serializable]
    public class SaveData
    {
        public int Balance;
        public BusinessSaveData[] BusinessSaveDataArray;
    }
}