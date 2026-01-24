using ECSTest.ECS.Data;
using Leopotam.EcsLite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ECSTest
{
    public class BusinessCardView : MonoBehaviour
    {
        private const int Upgrade1Index = 1;
        private const int Upgrade2Index = 2;

        [SerializeField] private TextMeshProUGUI _businessName;
        [SerializeField] private Image _progressBar;
        [SerializeField] private TextMeshProUGUI _businessBaseIncome;
        
        [SerializeField] private TextMeshProUGUI _businessLevelUpCost;
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private Button _levelUpButton;
        
        [SerializeField] private TextMeshProUGUI _upgrade1Title;
        [SerializeField] private TextMeshProUGUI _upgrade1Description;
        [SerializeField] private TextMeshProUGUI _upgrade1Cost;
        [SerializeField] private Button _upgrade1Button;
        
        [SerializeField] private TextMeshProUGUI _upgrade2Title;
        [SerializeField] private TextMeshProUGUI _upgrade2Description;
        [SerializeField] private TextMeshProUGUI _upgrade2Cost;
        [SerializeField] private Button _upgrade2Button;
        
        private EcsWorld _world;
        private int _businessEntity;

        private void OnEnable()
        {
            _levelUpButton.onClick.AddListener(LevelUpClicked);
            _upgrade1Button.onClick.AddListener(Upgrade1Clicked);
            _upgrade2Button.onClick.AddListener(Upgrade2Clicked);
        }

        private void OnDisable()
        {
            _levelUpButton.onClick.RemoveListener(LevelUpClicked);
            _upgrade1Button.onClick.RemoveListener(Upgrade1Clicked);
            _upgrade2Button.onClick.RemoveListener(Upgrade2Clicked);
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

        private void Upgrade1Clicked() => UpgradeClicked(Upgrade1Index);
        private void Upgrade2Clicked() => UpgradeClicked(Upgrade2Index);
        
        private void UpgradeClicked(int upgradeIndex)
        {
            int entity = _world.NewEntity();
            var upgradeEventPool = _world.GetPool<UpgradeEvent>();
            ref var requestUpgrade = ref upgradeEventPool.Add(entity);
            requestUpgrade.BusinessEntity = _businessEntity;
            requestUpgrade.UpgradeIndex = upgradeIndex;
        }

        public void UpdateUIInfo(BusinessDataComponent data)
        {
            _level.text = data.Level.ToString();
            _businessBaseIncome.text = $"{data.CurrentIncome}$";
            _businessLevelUpCost.text = $"{data.CurrentLevelUpCost}$";

            if (data.HasUpgrade1)
            {
                _upgrade1Cost.text = "SOLD!";
                _upgrade1Button.interactable = false;
            }

            if (data.HasUpgrade2)
            {
                _upgrade2Cost.text = "SOLD!";
                _upgrade2Button.interactable = false;
            }
        }

        public void SetStartConfig(BusinessNames names, BusinessConfig config)
        {
            _businessName.text = names.businessName;
            _businessBaseIncome.text = $"{config.baseIncome}$";
            _businessLevelUpCost.text = $"{config.baseCost}$";
            
            _upgrade1Title.text = names.upgrade1Name;
            _upgrade1Description.text = $"Income +{config.upgrade1Modificator * 100}%";
            _upgrade1Cost.text = $"Cost to you {config.upgrade1Cost}$";
            
            _upgrade2Title.text = names.upgrade2Name;
            _upgrade2Description.text = $"Income +{config.upgrade2Modificator * 100}%";
            _upgrade2Cost.text = $"Cost to you {config.upgrade2Cost}$";
        }

        public void UpdateProgressBar(float currentProgress)
        {
            _progressBar.fillAmount = currentProgress;
        }
    }
}