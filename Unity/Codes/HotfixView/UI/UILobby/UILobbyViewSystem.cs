using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    [UISystem]
    [FriendClass(typeof (UILobbyView))]
    [FriendClassAttribute(typeof (ET.UITransform))]
    public class UILobbyViewOnCreateSystem: OnCreateSystem<UILobbyView>
    {
        public override async void OnCreate(UILobbyView self)
        {
            self.EnterBtn = self.AddUIComponent<UIButton>("Panel/EnterMap");
            self.EnterBtn.SetOnClick(self.OnEnterBtnClick);
            self.ReturnLoginBtn = self.AddUIComponent<UIButton>("Panel/ReturnLogin");
            self.ReturnLoginBtn.SetOnClickAsync(self.OnReturnLoginBtnClick);
            // Panel/EnterChessMap
            self.EnterChessMapBtn = self.AddUIComponent<UIButton>("Panel/EnterChessMap");
            self.EnterChessMapBtn.SetOnClickAsync(self.OnEnterChessMapBtnClick);

            // Panel/StartMatch
            self.StartMatchBtn = self.AddUIComponent<UIButton>("Panel/StartMatch");
            self.StartMatchBtn.SetOnClickAsync(self.OnStartMatchBtnClick);

            self.BtnBag = self.AddUIComponent<UIButton>("Panel/BrnBag");
            self.BtnBag.SetOnClickAsync(self.OnBtnBagClick);

            self.ExercisesBtn = self.AddUIComponent<UIButton>("Panel/Exercises");
            self.ExercisesBtn.SetOnClick(self.OnExercisesBtnClick);

            self.PointItem = self.AddUIComponent<UIItem, int>("Panel/PointItem", ItemDefine.PointId);

            // Panel/GalLevel
            const string path = "Panel/GalLevel";
            GameObject go = self.GetGameObject();
            Transform ts = go.transform.Find(path);
            self.galLevelTs = ts;
            int childCount = ts.childCount;
            self.galBtns = new List<UIButton>(childCount);
            for (int i = 0; i < childCount; i++)
            {
                int j = i;
                UIButton uiButton = self.AddUIComponent<UIButton>($"{path}/Level_{i.ToString()}");
                self.galBtns.Add(uiButton);
                uiButton.SetOnClick(() =>
                {
                    GalConfig config = GalConfigCategory.Instance.Get(j + 1);
                    string chapterName = config.ChapterName;

                    GalGameEngineComponent.Instance.PlayChapterByName(chapterName, isOver => { OnPlayOver().Coroutine(); }).Coroutine();
                });

                async ETTask OnPlayOver()
                {
                    if (j + 1 == self.nextGalId)
                    {
                        self.nextGalId = await GalHelper.PassGal(self.ZoneScene());
                        self.RefreshGalBtns();
                    }
                }
            }

            ts.gameObject.SetActive(false);
            int nextGalId = await GalHelper.GetNextGalId(self.ZoneScene());
            self.nextGalId = nextGalId;
            self.RefreshGalBtns();
        }
    }

    [FriendClass(typeof (UILobbyView))]
    public static class UILobbyViewSystem
    {
        public static void OnEnterBtnClick(this UILobbyView self)
        {
            GalGameEngineComponent.Instance.PlayChapterByName("StartChapter").Coroutine();
            // GuidanceComponent.Instance.NoticeEvent("Click_EnterMap");
        }

        public static void OnExercisesBtnClick(this UILobbyView self)
        {
            UIManagerComponent.Instance.OpenWindow<UIExerciseView>(UIExerciseView.PrefabPath).Coroutine();
        }

        public static void RefreshGalBtns(this UILobbyView self)
        {
            int nextGalId = self.nextGalId;
            if (nextGalId != -1)
            {
                self.galLevelTs.gameObject.SetActive(true);
                for (int i = 0; i < self.galBtns.Count; i++)
                {
                    UIButton uiButton = self.galBtns[i];
                    if (i + 1 > nextGalId)
                    {
                        uiButton.SetEnabled(false);
                    }
                    else
                    {
                        uiButton.SetEnabled(true);
                    }
                }
            }
            else
            {
                self.galLevelTs.gameObject.SetActive(false);
            }
        }

        public static async ETTask OnReturnLoginBtnClick(this UILobbyView self)
        {
            int errorCode = await LoginHelper.Logout(self.ZoneScene());

            if (errorCode != ErrorCode.ERR_Success)
            {
                Log.Error("Logout error");
                return;
            }

            UIManagerComponent.Instance.OpenWindow<UILoginView>(UILoginView.PrefabPath).Coroutine();
            UIManagerComponent.Instance.DestroyWindow<UILobbyView>().Coroutine();
        }

        public static async ETTask OnEnterChessMapBtnClick(this UILobbyView self)
        {
            await EnterMapHelper.EnterChessMapAsync(self.ZoneScene());
        }

        public static async ETTask OnStartMatchBtnClick(this UILobbyView self)
        {
            Game.EventSystem.PublishAsync(new UIEventType.ShowDialog()
            {
                Text = "匹配中，请稍等...", OnCancel = () => { MatchHelper.LevelMatch(self.ZoneScene()).Coroutine(); },
            }).Coroutine();
            await MatchHelper.StartMatch(self.ZoneScene());
        }

        public static async ETTask OnBtnBagClick(this UILobbyView self)
        {
            await UIManagerComponent.Instance.OpenWindow<UIHero>(UIHero.PrefabPath);
        }

        public static void SetPoint(this UILobbyView self, int count)
        {
            self.PointItem.SetItemCount(count);
        }
    }
}