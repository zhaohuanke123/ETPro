namespace ET
{
    [UIComponent]
    public class UITextmesh: Entity,IAwake,IOnCreate,IOnCreate<string>,IOnEnable,II18N
    {
        public TMPro.TMP_Text textmesh;

        public I18NText i18nCompTouched;
        public string textKey;
        public object[] keyParams;

    }
}
