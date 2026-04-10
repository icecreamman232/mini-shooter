using Shinrai.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Shinrai.UI
{
    public class PlayerHealthBar : MonoBehaviour
    {
        [SerializeField] private Image _healthBar;

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
        }
    }
}
