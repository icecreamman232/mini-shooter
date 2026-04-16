using Shinrai.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shinrai.UI
{
    public class PlayerHealthBar : MonoBehaviour
    {
        [SerializeField] private Image _healthBar;
        [SerializeField] private TextMeshProUGUI _healthText;

        private void Awake()
        {
            EventBus.Subscribe<PlayerHealthChangedEvent>(OnUpdateHealthBar);
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<PlayerHealthChangedEvent>(OnUpdateHealthBar);
        }

        private void OnUpdateHealthBar(PlayerHealthChangedEvent e)
        {
            _healthBar.fillAmount = Util.Remap(e.CurrentHealth, 0, e.MaxHealth, 0, 1);
            _healthText.text = $"{e.CurrentHealth:F1}/{e.MaxHealth:F1}";
        }
    }
}
