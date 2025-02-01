using System;

namespace ET
{
    [UIComponent]
    public class UIBagItem : Entity, IAwake, IOnCreate, IOnEnable
    {
        public int itemId;
        public UIImage ItemIcon;
        public UIText ItemName;
        public UITextmesh ItemCount;
        public Action<int> OnClickCallback { get; set; }
        public UIButton Btn;
        public int index;
    }
} 