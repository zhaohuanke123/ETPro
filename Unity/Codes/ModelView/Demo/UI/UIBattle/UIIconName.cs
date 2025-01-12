using UnityEngine.UI;

namespace ET
{
    [UIComponent]
    public class UIIconName: Entity, IAwake, IDestroy, IOnCreate, IOnEnable
    {
        public UIImage Icon;
        public UIText Name;
    }
}