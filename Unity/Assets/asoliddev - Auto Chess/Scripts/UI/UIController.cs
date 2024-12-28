using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Updates and controls UI elements
/// </summary>
public class UIController: MonoBehaviour
{
    // public ChampionShop championShop;
    // public GamePlayController gamePlayController;

    public GameObject[] championsFrameArray;
    public GameObject[] bonusPanels;

    public Text timerText;
    public Text championCountText;
    public Text goldText;
    public Text hpText;

    public GameObject shop;
    public GameObject restartButton;
    public GameObject placementText;
    public GameObject gold;
    public GameObject bonusContainer;
    public GameObject bonusUIPrefab;

    public void OnChampionClicked()
    {
        string name = EventSystem.current.currentSelectedGameObject.transform.parent.name;

        string defaultName = "champion container_";
        int championFrameIndex = int.Parse(name.Substring(defaultName.Length, 1));

        // championShop.OnChampionFrameClicked(championFrameIndex);
    }

    public void Refresh_Click()
    {
        // championShop.RefreshShop(false);   
    }

    public void BuyXP_Click()
    {
        // championShop.BuyLvl();
    }

    public void Restart_Click()
    {
        // gamePlayController.RestartGame();
    }

    public void HideChampionFrame(int index)
    {
        championsFrameArray[index].transform.Find("champion").gameObject.SetActive(false);
    }

    public void ShowShopItems()
    {
        //unhide all champion frames
        for (int i = 0; i < championsFrameArray.Length; i++)
        {
            championsFrameArray[i].transform.Find("champion").gameObject.SetActive(true);
        }
    }

    public void LoadShopItem(Champion champion, int index)
    {
        Transform championUI = championsFrameArray[index].transform.Find("champion");
        Transform top = championUI.Find("top");
        Transform bottom = championUI.Find("bottom");
        Transform type1 = top.Find("type 1");
        Transform type2 = top.Find("type 2");
        Transform name = bottom.Find("Name");
        Transform cost = bottom.Find("Cost");
        Transform icon1 = top.Find("icon 1");
        Transform icon2 = top.Find("icon 2");

        name.GetComponent<Text>().text = champion.uiname;
        cost.GetComponent<Text>().text = champion.cost.ToString();
        type1.GetComponent<Text>().text = champion.type1.displayName;
        type2.GetComponent<Text>().text = champion.type2.displayName;
        icon1.GetComponent<Image>().sprite = champion.type1.icon;
        icon2.GetComponent<Image>().sprite = champion.type2.icon;
    }

    public void UpdateUI()
    {
        // goldText.text = gamePlayController.currentGold.ToString();
        // championCountText.text = gamePlayController.currentChampionCount + " / " + gamePlayController.currentChampionLimit;
        // hpText.text = "HP " + gamePlayController.currentHP;

        foreach (GameObject go in bonusPanels)
        {
            go.SetActive(false);
        }

        //if not null
        // if (gamePlayController.championTypeCount != null)
        // {
        //     int i = 0;
        //     //iterate bonuses
        //     foreach (KeyValuePair<ChampionType, int> m in gamePlayController.championTypeCount)
        //     {
        //         //Now you can access the key and value both separately from this attachStat as:
        //         GameObject bonusUI = bonusPanels[i];
        //         bonusUI.transform.SetParent(bonusContainer.transform);
        //         bonusUI.transform.Find("icon").GetComponent<Image>().sprite = m.Key.icon;
        //         bonusUI.transform.Find("name").GetComponent<Text>().text = m.Key.displayName;
        //         bonusUI.transform.Find("count").GetComponent<Text>().text = m.Value + " / " + m.Key.championBonus.championCount;
        //
        //         bonusUI.SetActive(true);
        //
        //         i++;   
        //     }
        // }
    }

    public void UpdateTimerText()
    {
        // timerText.text = gamePlayController.timerDisplay.ToString();
    }

    public void SetTimerTextActive(bool b)
    {
        timerText.gameObject.SetActive(b);

        placementText.SetActive(b);
    }

    public void ShowLossScreen()
    {
        SetTimerTextActive(false);
        shop.SetActive(false);
        gold.SetActive(false);

        restartButton.SetActive(true);
    }

    public void ShowGameScreen()
    {
        SetTimerTextActive(true);
        shop.SetActive(true);
        gold.SetActive(true);

        restartButton.SetActive(false);
    }
}