using System;
namespace ET
{
    [UIComponent]
    public class UIUpdateView : Entity,IAwake,IOnCreate,IOnEnable<Action>
    {
        public static string PrefabPath => "UI/UIUpdate/Prefabs/UIUpdateView.prefab";
        public readonly int BTN_CANCEL = 1;
        public readonly int BTN_CONFIRM = 2;

        public UISlider Slider;
        public UIProgressCircle ProgressCircle;
        public UIText TitleText;

        public UIMsgBoxWin.MsgBoxPara Para { get; private set; } = new UIMsgBoxWin.MsgBoxPara();
        
        public float LastProgress;

        public Action OnOver;
        public bool ForceUpdate;

        public YooAsset.PatchDownloaderOperation Downloader;
        public int StaticVersion;
    }
}
