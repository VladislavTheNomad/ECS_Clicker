using UnityEngine;

namespace ECSTest.ECS.Data
{
    [CreateAssetMenu(fileName = "PlayerInitData", menuName = "ECS Data")]
    public class PlayerInitData : ScriptableObject
    {
        public int defaultBalance;
    }
}