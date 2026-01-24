using ECSTest.ECS.Data;
using Leopotam.EcsLite;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ECSTest
{
    public class BusinessCardView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI businessName;
        [SerializeField] private Image progressBar;
        [SerializeField] private TextMeshProUGUI businessBaseIncome;
        
        [SerializeField] private TextMeshProUGUI businessLevelUpCost;
        [SerializeField] private TextMeshProUGUI level;
        [SerializeField] private Button levelUpButton;
        
        [SerializeField] private TextMeshProUGUI upgrade1Title;
        [SerializeField] private TextMeshProUGUI upgrade1Description;
        [SerializeField] private TextMeshProUGUI upgrade1Cost;
        [SerializeField] private Button upgrade1Button;
        
        [SerializeField] private TextMeshProUGUI upgrade2Title;
        [SerializeField] private TextMeshProUGUI upgrade2Description;
        [SerializeField] private TextMeshProUGUI upgrade2Cost;
        [SerializeField] private Button upgrade2Button;
        
        private EcsWorld _world;
        private int _businessEntity;

        private void Start()
        {
            levelUpButton.onClick.AddListener(LevelUpClicked);
            upgrade1Button.onClick.AddListener(Upgrade1Clicked);
            upgrade2Button.onClick.AddListener(Upgrade2Clicked);
        }

        private void OnDestroy()
        {
            levelUpButton.onClick.RemoveListener(LevelUpClicked);
            upgrade1Button.onClick.RemoveListener(Upgrade1Clicked);
            upgrade2Button.onClick.RemoveListener(Upgrade2Clicked);
        }

        public void Init(EcsWorld world, int businessEntity)
        {
            _world =  world;
            _businessEntity = businessEntity;
        }

        private void LevelUpClicked()
        {
            int entity = _world.NewEntity();
            var levelUpEventPool = _world.GetPool<LevelUpEvent>();
            ref var requestLevelUp = ref levelUpEventPool.Add(entity);
            requestLevelUp.BusinessEntity = _businessEntity;
        }

        private void Upgrade1Clicked() => UpgradeClicked(1);
        private void Upgrade2Clicked() => UpgradeClicked(2);
        
        private void UpgradeClicked(int upgradeIndex)
        {
            int entity = _world.NewEntity();
            var upgradeEventPool = _world.GetPool<UpgradeEvent>();
            ref var requestUpgrade = ref upgradeEventPool.Add(entity);
            requestUpgrade.BusinessEntity = _businessEntity;
            requestUpgrade.upgradeIndex = upgradeIndex;
        }

        public void UpdateUIInfo(BusinessDataComponent data)
        {
            level.text = data.Level.ToString();
            businessBaseIncome.text = $"{data.CurrentIncome}$";
            businessLevelUpCost.text = $"{data.CurrentLevelUpCost}$";

            if (data.HasUpgrade1)
            {
                upgrade1Cost.text = "SOLD!";
                upgrade1Button.interactable = false;
            }

            if (data.HasUpgrade2)
            {
                upgrade2Cost.text = "SOLD!";
                upgrade2Button.interactable = false;
            }
        }

        public void SetStartConfig(BusinessNames names, BusinessConfig config)
        {
            businessName.text = names.businessName;
            businessBaseIncome.text = $"{config.baseIncome}$";
            businessLevelUpCost.text = $"{config.baseCost}$";
            
            upgrade1Title.text = names.upgrade1Name;
            upgrade1Description.text = $"Income +{config.upgrade1Modificator * 100}%";
            upgrade1Cost.text = $"Cost to you {config.upgrade1Cost}$";
            
            upgrade2Title.text = names.upgrade2Name;
            upgrade2Description.text = $"Income +{config.upgrade2Modificator * 100}%";
            upgrade2Cost.text = $"Cost to you {config.upgrade2Cost}$";
        }

        public void UpdateProgressBar(float currentProgress)
        {
            progressBar.fillAmount = currentProgress;
        }
    }
}