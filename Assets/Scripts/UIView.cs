using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ECSTest
{
    public class UIView : MonoBehaviour
    {
        [SerializeField] private TMP_Text balanceText;
        
        private readonly Dictionary<int, BusinessCardView> _businessDict = new Dictionary<int, BusinessCardView>();

        public void RegisterNewBusiness(int entityID, BusinessCardView businessCardView)
        {
            _businessDict[entityID+1] = businessCardView;
        }
        
        public void SetBalance(int newBalance) =>  balanceText.text = $"{newBalance}$";

        public void SetBusinessLevel(int entity, int newLevel)
        {
            if (_businessDict.TryGetValue(entity, out var businessCardView))
            {
                businessCardView.SetLevel(newLevel);
            }
        }
    }
}