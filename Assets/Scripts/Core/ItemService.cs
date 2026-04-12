using UnityEngine;

namespace Shinrai.Core
{
    public class ItemService : MonoBehaviour, IGameService, IBootStrap
    {
        public void Install()
        {
            ServiceLocator.RegisterService(this);
        }

        public void Uninstall()
        {
            ServiceLocator.UnregisterService<ItemService>();
        }
    }
}
