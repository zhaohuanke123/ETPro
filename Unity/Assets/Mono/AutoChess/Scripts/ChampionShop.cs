using UnityEngine;

public class ChampionShop: MonoBehaviour
{
    public GamePlayController gamePlayController;
    public GameData gameData;

    private Champion[] availableChampionArray;

    private void Start()
    {
        RefreshShop(true);
    }

    public void BuyLvl()
    {
        gamePlayController.Buylvl();
    }

    public void RefreshShop(bool isFree)
    {
        if (gamePlayController.currentGold < 2 && isFree == false)
            return;

        availableChampionArray = new Champion[5];

        for (int i = 0; i < availableChampionArray.Length; i++)
        {
            Champion champion = GetRandomChampionInfo();

            availableChampionArray[i] = champion;

            // uIController.LoadShopItem(champion, i);

            // uIController.ShowShopItems();
        }

        if (isFree == false)
            gamePlayController.currentGold -= 2;

        // uIController.UpdateUI();
    }

    public void OnChampionFrameClicked(int index)
    {
        bool isSucces = gamePlayController.BuyChampionFromShop(availableChampionArray[index]);

        // if(isSucces)
        //     uIController.HideChampionFrame(index);
    }

    public Champion GetRandomChampionInfo()
    {
        int rand = Random.Range(0, gameData.championsArray.Length);

        return gameData.championsArray[rand];
    }
}