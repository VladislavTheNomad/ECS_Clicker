using UnityEngine;

namespace ECSTest.ECS.Data
{
    [CreateAssetMenu(fileName = "BusinessConfig", menuName =  "ECS/Data Business Config")]
    public class BusinessConfig : ScriptableObject
    {
        public string name;
        public float timeToIncome;
        public int baseCost;
        public int baseIncome;
        public string upgrade1Name;
        public float upgrade1Modificator;
        public int upgrade1Cost;
        public string upgrade2Name;
        public float upgrade2Modificator;
        public int upgrade2Cost;
    }
}