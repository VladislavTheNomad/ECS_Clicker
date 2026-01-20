using TMPro;
using UnityEngine;

namespace ECSTest
{
    public class BusinessCardView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI level;

        public void SetLevel(int newLevel) => level.text = newLevel.ToString();
    }
}