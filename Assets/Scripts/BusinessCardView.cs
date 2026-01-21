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
        
        [SerializeField] private TextMeshProUGUI upgrade2Title;
        [SerializeField] private TextMeshProUGUI upgrade2Description;
        [SerializeField] private TextMeshProUGUI upgrade2Cost;
        
        private EcsWorld _world;
        private int _businessEntity;

        private void Start()
        {
            levelUpButton.onClick.AddListener(LevelUpClicked);
        }

        private void OnDestroy()
        {
            levelUpButton.onClick.RemoveListener(LevelUpClicked);
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
            Debug.Log(requestLevelUp.BusinessEntity + "Clicked!");
        }

        public void UpdateUIInfo(BusinessDataComponent data)
        {
            level.text = data.Level.ToString();
            businessBaseIncome.text = $"{data.CurrentIncome}$";
            businessLevelUpCost.text = $"{data.CurrentLevelUpCost}$";
        }

        public void SetStartConfig(BusinessConfig config)
        {
            businessName.text = config.name;
            businessBaseIncome.text = $"{config.baseIncome}$";
            businessLevelUpCost.text = $"{config.baseCost}$";
            
            upgrade1Title.text = config.upgrade1Name;
            upgrade1Description.text = $"Income +{config.upgrade1Modificator * 100}%";
            upgrade1Cost.text = $"Cost to you {config.upgrade1Cost}$";
            
            upgrade2Title.text = config.upgrade2Name;
            upgrade2Description.text = $"Income +{config.upgrade2Modificator * 100}%";
            upgrade2Cost.text = $"Cost to you {config.upgrade2Cost}$";
        }

        public void UpdateProgressBar(float currentProgress)
        {
            progressBar.fillAmount = currentProgress;
        }
    }
}