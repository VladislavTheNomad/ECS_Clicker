using UnityEngine;

namespace ECSTest.ECS.Data
{
    [CreateAssetMenu(fileName = "BusinessConfig", menuName =  "ECS/Data Business Config")]
    public class BusinessConfig : ScriptableObject
    {
        public float timeToIncome;
        public int baseCost;
        public int baseIncome;
        public float upgrade1Modificator;
        public int upgrade1Cost;
        public float upgrade2Modificator;
        public int upgrade2Cost;
    }
}