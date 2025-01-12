namespace ET
{
    [UIComponent]
	public class UILobbyView : Entity,IAwake,IOnCreate,IOnEnable
    {
        public static string PrefabPath => "UI/UILobby/Prefabs/UILobbyView.prefab";
        public UIButton EnterBtn;
        public UIButton ReturnLoginBtn;
        public UIButton EnterChessMapBtn;
    }
}