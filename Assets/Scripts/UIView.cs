using System.Collections.Generic;
using ECSTest.ECS.Data;
using TMPro;
using UnityEngine;

namespace ECSTest
{
    public class UIView : MonoBehaviour
    {
        [SerializeField] private TMP_Text balanceText;
        
        private readonly Dictionary<int, BusinessCardView> _businessDict = new Dictionary<int, BusinessCardView>();
        private readonly Dictionary<int, BusinessConfig> _businessConfDict = new Dictionary<int, BusinessConfig>();

        public void RegisterNewBusiness(int entityID, BusinessCardView businessCardView, BusinessConfig config)
        {
            _businessDict[entityID] = businessCardView;
            _businessConfDict[entityID] = config;
            businessCardView.SetStartConfig(config);
        }
        
        public void SetBalance(int newBalance) =>  balanceText.text = $"{newBalance}$";

        public void UpdateUI(int entity, BusinessDataComponent data)
        {
            if (_businessDict.TryGetValue(entity, out var businessCardView))
            {
                Debug.Log($"UIView: Setting business {entity}");
                businessCardView.UpdateUIInfo(data);
            }
        }

        public void UpdateProgressBar(int entity, float currentProgress)
        {
            if (_businessDict.TryGetValue(entity, out var businessCardView))
            {
                businessCardView.UpdateProgressBar(currentProgress);
            }
        }
    }
}