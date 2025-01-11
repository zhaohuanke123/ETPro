using UnityEngine.UI;

namespace ET
{
    [UIComponent]
    public class UIText : Entity,IAwake,IOnCreate,IOnCreate<string>,IOnEnable,II18N
    {
        public Text text;
        public I18NText i18nCompTouched;
        public string textKey;
        public object[] keyParams;
    }
}
