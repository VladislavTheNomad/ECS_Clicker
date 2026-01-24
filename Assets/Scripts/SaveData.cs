using System;

namespace ECSTest
{
    [Serializable]
    public class SaveData
    {
        public int balance;
        public BusinessSaveData[] businessSaveData;
    }
}