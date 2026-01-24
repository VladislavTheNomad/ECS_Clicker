using System.Collections.Generic;
using ECSTest.ECS.Data;
using TMPro;
using UnityEngine;

namespace ECSTest
{
    public class UIView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _balanceText;
        
        private readonly Dictionary<int, BusinessCardView> _businessDict = new Dictionary<int, BusinessCardView>();

        public void RegisterNewBusiness(int entity, BusinessCardView businessCardView, BusinessConfig config, BusinessNames businessNames)
        {
            _businessDict[entity] = businessCardView;
            businessCardView.SetStartConfig(businessNames, config);
        }
        
        public void SetBalance(int newBalance) =>  _balanceText.text = $"{newBalance}$";

        public void UpdateUI(int entity, BusinessDataComponent data)
        {
            if (_businessDict.TryGetValue(entity, out var businessCardView))
            {
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