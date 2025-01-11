using UnityEngine.UI;

namespace ET
{
    [UIComponent]
    public class UIRawImage: Entity,IAwake,IOnCreate,IOnCreate<string>,IOnEnable
    {
        public string spritePath;
        public RawImage image;
        public BgRawAutoFit bgRawAutoFit;
        public bool grayState;
    }
}
