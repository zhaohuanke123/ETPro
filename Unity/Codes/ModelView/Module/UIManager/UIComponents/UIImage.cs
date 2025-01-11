using UnityEngine.UI;

namespace ET
{
    [UIComponent]
    public class UIImage:Entity,IAwake,IOnCreate,IOnCreate<string>,IOnEnable
    {
        public string spritePath;
        public Image image;
        public BgAutoFit bgAutoFit;
        public bool grayState;
    }
}
