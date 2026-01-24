using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ECSTest
{
    [CreateAssetMenu(fileName = "BusinessName", menuName = "ECS/Data Business Names")]
    public class BusinessNames : ScriptableObject
    {
        public string businessName;
        public string upgrade1Name;
        public string upgrade2Name;
    }
}
