using System.Collections.Generic;
using UnityEngine;
using SuperScrollView;
using TMPro;

namespace ET
{
    [UISystem]
    [FriendClass(typeof (UIExerciseView))]
    public class UIExerciseViewOnCreateSystem: OnCreateSystem<UIExerciseView>
    {
        public override void OnCreate(UIExerciseView self)
        {
            self.RetBtn = self.AddUIComponent<UIButton>("Panel/RetBtn");
            self.TopList = self.AddUIComponent<UIEmptyGameobject>("Panel/TopList");
            self.ExcList = self.AddUIComponent<UILoopListView2>("Panel/Exerc/ExcList");
            self.ShowExecText = self.AddUIComponent<UITextmesh>("Panel/Exerc/Scroll View/Viewport/Content/UIText");
            self.PointText = self.AddUIComponent<UITextmesh>("Panel/PointText");
            self.SubmitBtn = self.AddUIComponent<UIButton>("Panel/Exerc/SubmitBtn");
            self.RetBtn.SetOnClick(self.OnClickRetBtn);
            self.ExcList.InitListView(0, self.GetExcListItemByIndex);
            self.SubmitBtn.SetOnClickAsync(self.OnClickSubmitBtn);

            Transform transform = self.TopList.GetGameObject().transform;
            List<ExerciseConfig> exerciseConfigs = ExerciseConfigCategory.Instance.GetAllList();
            for (int i = 0; i < exerciseConfigs.Count; i++)
            {
                Transform child = transform.GetChild(i);
                Transform find = child.Find("Content/Label_Button");
                TMP_Text text = find.GetComponent<TMP_Text>();
                text.text = exerciseConfigs[i].Name;
            }
        }
    }

    [ObjectSystem]
    [FriendClass(typeof (UIExerciseView))]
    public class UIExerciseViewLoadSystem: LoadSystem<UIExerciseView>
    {
        public override void Load(UIExerciseView self)
        {
            self.RetBtn.SetOnClick(self.OnClickRetBtn);
            self.ExcList.InitListView(0, self.GetExcListItemByIndex);
        }
    }

    [FriendClass(typeof (UIExerciseView))]
    public static class UIExerciseViewSystem
    {
        public static void OnClickRetBtn(this UIExerciseView self)
        {
            self.CloseSelf().Coroutine();
        }

        public static async ETTask OnClickSubmitBtn(this UIExerciseView self)
        {
            await ExecisesHelper.PassAnExec(self.ZoneScene());
        }

        public static void ShowExecPage(this UIExerciseView self, int index)
        {
            List<ExerciseConfig> configs = ExerciseConfigCategory.Instance.GetAllList();
            if (index >= configs.Count)
            {
                return;
            }

            int count = configs[index].PointCount;
            self.SetCurExecPointText($"当前题目奖励 ：{count.ToString()}");

            // TODO 更新题目界面
        }

        public static LoopListViewItem2 GetExcListItemByIndex(this UIExerciseView self, LoopListView2 listView, int index)
        {
            return null;
        }

        public static void SetExecText(this UIExerciseView self, string text)
        {
            self.ShowExecText.SetText(text);
        }

        public static void SetCurExecPointText(this UIExerciseView self, string text)
        {
            self.PointText.SetText(text);
        }
    }
}