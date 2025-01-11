using UnityEngine.Events;
using UnityEngine.UI;

namespace ET
{
    [UIComponent]
    public class UIDropdown : Entity, IAwake
    {
        public Dropdown dropdown;
        public UnityAction<int> onValueChanged;
    }
}