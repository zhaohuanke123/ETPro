using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum GameStage
{
    Preparation,
    Combat,
    Loss
};

public class GamePlayController: MonoBehaviour
{
    public static GamePlayController Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = FindObjectOfType<GamePlayController>();
            }

            return s_instance;
        }
    }

    public static GamePlayController s_instance;

    // public Map map;
    public InputController inputController;

    public GameData gameData;

    // public UIController uIController;
    public AIopponent aIopponent;
    public ChampionShop championShop;

    [HideInInspector]
    public GameObject[] ownChampionInventoryArray;

    [HideInInspector]
    public GameObject[] oponentChampionInventoryArray;

    [HideInInspector]
    public GameObject[,] gridChampionsArray;

    public GameStage currentGameStage;
    public float timer = 0;

    public int PreparationStageDuration = 16;

    public int CombatStageDuration = 60;

    public int baseGoldIncome = 5;

    [HideInInspector]
    public int currentChampionLimit = 3;

    [HideInInspector]
    public int currentChampionCount = 0;

    [HideInInspector]
    public int currentGold = 5;

    [HideInInspector]
    public int currentHP = 100;

    [HideInInspector]
    public int timerDisplay = 0;

    public Dictionary<ChampionType, int> championTypeCount;
    public List<ChampionBonus> activeBonusList;

    public void Start()
    {
        currentGameStage = GameStage.Preparation;

        ownChampionInventoryArray = new GameObject[Map.inventorySize];
        oponentChampionInventoryArray = new GameObject[Map.inventorySize];
        gridChampionsArray = new GameObject[Map.hexMapSizeX, Map.hexMapSizeZ / 2];
    }

    public void Update()
    {
        // if (currentGameStage == GameStage.Preparation)
        // {
        //     timer += Time.deltaTime;
        //
        //     timerDisplay = (int)(PreparationStageDuration - timer);
        //
        //     // uIController.UpdateTimerText();
        //
        //     if (timer > PreparationStageDuration)
        //     {
        //         timer = 0;
        //
        //         this.OnGameStageComplicate();
        //     }
        // }
        // else if (currentGameStage == GameStage.Combat)
        // {
        //     timer += Time.deltaTime;
        //
        //     timerDisplay = (int)timer;
        //
        //     if (timer > CombatStageDuration)
        //     {
        //         timer = 0;
        //
        //         this.OnGameStageComplicate();
        //     }
        // }
    }

    public bool BuyChampionFromShop(Champion champion)
    {
        int emptyIndex = -1;
        for (int i = 0; i < ownChampionInventoryArray.Length; i++)
        {
            if (ownChampionInventoryArray[i] == null)
            {
                emptyIndex = i;
                break;
            }
        }

        return true;
    }

    public void TryUpgradeChampion(Champion champion)
    {
        // List<ChampionController> championList_lvl_1 = new List<ChampionController>();
        // List<ChampionController> championList_lvl_2 = new List<ChampionController>();
        //
        // for (int i = 0; i < ownChampionInventoryArray.Length; i++)
        // {
        //     if (ownChampionInventoryArray[i] != null)
        //     {
        //         ChampionController championController = ownChampionInventoryArray[i].GetComponent<ChampionController>();
        //
        //         if (championController.champion == champion)
        //         {
        //             if (championController.lvl == 1)
        //                 championList_lvl_1.Add(championController);
        //             else if (championController.lvl == 2)
        //                 championList_lvl_2.Add(championController);
        //         }
        //     }
        // }
        //
        // for (int x = 0; x < Map.hexMapSizeX; x++)
        // {
        //     for (int z = 0; z < Map.hexMapSizeZ / 2; z++)
        //     {
        //         if (gridChampionsArray[x, z] != null)
        //         {
        //             ChampionController championController = gridChampionsArray[x, z].GetComponent<ChampionController>();
        //
        //             if (championController.champion == champion)
        //             {
        //                 if (championController.lvl == 1)
        //                     championList_lvl_1.Add(championController);
        //                 else if (championController.lvl == 2)
        //                     championList_lvl_2.Add(championController);
        //             }
        //         }
        //     }
        // }
        //
        // if (championList_lvl_1.Count > 2)
        // {
        //     championList_lvl_1[2].UpgradeLevel();
        //
        //     RemoveChampionFromArray(championList_lvl_1[0].gridType, championList_lvl_1[0].gridPositionX, championList_lvl_1[0].gridPositionZ);
        //     RemoveChampionFromArray(championList_lvl_1[1].gridType, championList_lvl_1[1].gridPositionX, championList_lvl_1[1].gridPositionZ);
        //
        //     Destroy(championList_lvl_1[0].gameObject);
        //     Destroy(championList_lvl_1[1].gameObject);
        //
        //     if (championList_lvl_2.Count > 1)
        //     {
        //         championList_lvl_1[2].UpgradeLevel();
        //
        //         RemoveChampionFromArray(championList_lvl_2[0].gridType, championList_lvl_2[0].gridPositionX, championList_lvl_2[0].gridPositionZ);
        //         RemoveChampionFromArray(championList_lvl_2[1].gridType, championList_lvl_2[1].gridPositionX, championList_lvl_2[1].gridPositionZ);
        //
        //         Destroy(championList_lvl_2[0].gameObject);
        //         Destroy(championList_lvl_2[1].gameObject);
        //     }
        // }
        //
        // currentChampionCount = GetChampionCountOnHexGrid();

        // uIController.UpdateUI();
    }

    public GameObject draggedChampion = null;
    public TriggerInfo dragStartTrigger = null;

    public void StartDrag(TriggerInfo triggerInfo)
    {
        // if (currentGameStage != GameStage.Preparation)
        //     return;

        // TriggerInfo triggerInfo = inputController.triggerInfo;
        if (triggerInfo != null)
        {
            dragStartTrigger = triggerInfo;

            GameObject championGO = GetChampionFromTriggerInfo(triggerInfo);

            if (championGO != null)
            {
                Map.Instance.ShowIndicators();

                draggedChampion = championGO;

                //isDragging = true;

                championGO.GetComponent<ChampionController>().IsDragged = true;
            }
        }
    }

    public void StopDrag(TriggerInfo triggerInfo)
    {
        Map.Instance.HideIndicators();

        int championsOnField = GetChampionCountOnHexGrid();

        if (draggedChampion != null)
        {
            draggedChampion.GetComponent<ChampionController>().IsDragged = false;

            // TriggerInfo triggerinfo = inputController.triggerInfo;

            if (triggerInfo != null)
            {
                GameObject currentTriggerChampion = GetChampionFromTriggerInfo(triggerInfo);

                if (currentTriggerChampion != null)
                {
                    StoreChampionInArray(dragStartTrigger.gridType, dragStartTrigger.gridX, dragStartTrigger.gridZ, currentTriggerChampion);

                    StoreChampionInArray(triggerInfo.gridType, triggerInfo.gridX, triggerInfo.gridZ, draggedChampion);
                }
                else
                {
                    if (triggerInfo.gridType == Map.GRIDTYPE_HEXA_MAP)
                    {
                        if (championsOnField < currentChampionLimit || dragStartTrigger.gridType == Map.GRIDTYPE_HEXA_MAP)
                        {
                            RemoveChampionFromArray(dragStartTrigger.gridType, dragStartTrigger.gridX, dragStartTrigger.gridZ);

                            StoreChampionInArray(triggerInfo.gridType, triggerInfo.gridX, triggerInfo.gridZ, draggedChampion);

                            if (dragStartTrigger.gridType != Map.GRIDTYPE_HEXA_MAP)
                                championsOnField++;
                        }
                    }
                    else if (triggerInfo.gridType == Map.GRIDTYPE_OWN_INVENTORY)
                    {
                        RemoveChampionFromArray(dragStartTrigger.gridType, dragStartTrigger.gridX, dragStartTrigger.gridZ);

                        StoreChampionInArray(triggerInfo.gridType, triggerInfo.gridX, triggerInfo.gridZ, draggedChampion);

                        if (dragStartTrigger.gridType == Map.GRIDTYPE_HEXA_MAP)
                            championsOnField--;
                    }
                }
            }

            CalculateBonuses();

            currentChampionCount = GetChampionCountOnHexGrid();

            // uIController.UpdateUI();

            draggedChampion = null;
        }
    }

    public GameObject GetChampionFromTriggerInfo(TriggerInfo triggerinfo)
    {
        GameObject championGO = null;

        if (triggerinfo.gridType == Map.GRIDTYPE_OWN_INVENTORY)
        {
            championGO = ownChampionInventoryArray[triggerinfo.gridX];
        }
        else if (triggerinfo.gridType == Map.GRIDTYPE_OPONENT_INVENTORY)
        {
            championGO = oponentChampionInventoryArray[triggerinfo.gridX];
        }
        else if (triggerinfo.gridType == Map.GRIDTYPE_HEXA_MAP)
        {
            championGO = gridChampionsArray[triggerinfo.gridX, triggerinfo.gridZ];
        }

        return championGO;
    }

    public void StoreChampionInArray(int gridType, int gridX, int gridZ, GameObject champion)
    {
        ChampionController championController = champion.GetComponent<ChampionController>();
        championController.SetGridPosition(gridType, gridX, gridZ);

        if (gridType == Map.GRIDTYPE_OWN_INVENTORY)
        {
            ownChampionInventoryArray[gridX] = champion;
        }
        else if (gridType == Map.GRIDTYPE_HEXA_MAP)
        {
            gridChampionsArray[gridX, gridZ] = champion;
        }
    }

    public void RemoveChampionFromArray(int type, int gridX, int gridZ)
    {
        if (type == Map.GRIDTYPE_OWN_INVENTORY)
        {
            ownChampionInventoryArray[gridX] = null;
        }
        else if (type == Map.GRIDTYPE_HEXA_MAP)
        {
            gridChampionsArray[gridX, gridZ] = null;
        }
    }

    /// <summary>
    /// 计算在地图有多少个角色
    /// </summary>
    /// <returns></returns>
    public int GetChampionCountOnHexGrid()
    {
        int count = 0;
        for (int x = 0; x < Map.hexMapSizeX; x++)
        {
            for (int z = 0; z < Map.hexMapSizeZ / 2; z++)
            {
                if (gridChampionsArray[x, z] != null)
                {
                    count++;
                }
            }
        }

        return count;
    }

    public void CalculateBonuses()
    {
        // championTypeCount = new Dictionary<ChampionType, int>();
        //
        // for (int x = 0; x < Map.hexMapSizeX; x++)
        // {
        //     for (int z = 0; z < Map.hexMapSizeZ / 2; z++)
        //     {
        //         if (gridChampionsArray[x, z] != null)
        //         {
        //             Champion c = gridChampionsArray[x, z].GetComponent<ChampionController>().champion;
        //
        //             if (championTypeCount.ContainsKey(c.type1))
        //             {
        //                 int cCount = 0;
        //                 championTypeCount.TryGetValue(c.type1, out cCount);
        //
        //                 cCount++;
        //
        //                 championTypeCount[c.type1] = cCount;
        //             }
        //             else
        //             {
        //                 championTypeCount.Add(c.type1, 1);
        //             }
        //
        //             if (championTypeCount.ContainsKey(c.type2))
        //             {
        //                 int cCount = 0;
        //                 championTypeCount.TryGetValue(c.type2, out cCount);
        //
        //                 cCount++;
        //
        //                 championTypeCount[c.type2] = cCount;
        //             }
        //             else
        //             {
        //                 championTypeCount.Add(c.type2, 1);
        //             }
        //         }
        //     }
        // }
        //
        // activeBonusList = new List<ChampionBonus>();
        //
        // foreach (KeyValuePair<ChampionType, int> m in championTypeCount)
        // {
        //     ChampionBonus championBonus = m.Key.championBonus;
        //
        //     if (m.Value >= championBonus.championCount)
        //     {
        //         activeBonusList.Add(championBonus);
        //     }
        // }
    }

    public void ResetChampions()
    {
        for (int x = 0; x < Map.hexMapSizeX; x++)
        {
            for (int z = 0; z < Map.hexMapSizeZ / 2; z++)
            {
                if (gridChampionsArray[x, z] != null)
                {
                    ChampionController championController = gridChampionsArray[x, z].GetComponent<ChampionController>();

                    championController.Reset();
                }
            }
        }
    }

    public void OnGameStageComplicate()
    {
        aIopponent.OnGameStageComplate(currentGameStage);

        if (currentGameStage == GameStage.Preparation)
        {
            currentGameStage = GameStage.Combat;

            Map.Instance.HideIndicators();

            // uIController.SetTimerTextActive(false);

            if (draggedChampion != null)
            {
                draggedChampion.GetComponent<ChampionController>().IsDragged = false;
                draggedChampion = null;
            }

            for (int i = 0; i < ownChampionInventoryArray.Length; i++)
            {
                if (ownChampionInventoryArray[i] != null)
                {
                    ChampionController championController = ownChampionInventoryArray[i].GetComponent<ChampionController>();

                    championController.OnCombatStart();
                }
            }

            for (int x = 0; x < Map.hexMapSizeX; x++)
            {
                for (int z = 0; z < Map.hexMapSizeZ / 2; z++)
                {
                    if (gridChampionsArray[x, z] != null)
                    {
                        ChampionController championController = gridChampionsArray[x, z].GetComponent<ChampionController>();

                        championController.OnCombatStart();
                    }
                }
            }

            if (IsAllChampionDead())
                EndRound();
        }
        else if (currentGameStage == GameStage.Combat)
        {
            currentGameStage = GameStage.Preparation;

            // uIController.SetTimerTextActive(true);

            ResetChampions();

            for (int i = 0; i < gameData.championsArray.Length; i++)
            {
                TryUpgradeChampion(gameData.championsArray[i]);
            }

            currentGold += CalculateIncome();

            // uIController.UpdateUI();

            championShop.RefreshShop(true);

            if (currentHP <= 0)
            {
                currentGameStage = GameStage.Loss;
                // uIController.ShowLossScreen();
            }
        }
    }

    public int CalculateIncome()
    {
        int income = 0;

        //banked gold
        int bank = (int)(currentGold / 10);

        income += baseGoldIncome;
        income += bank;

        return income;
    }

    public void Buylvl()
    {
        if (currentGold < 4)
            return;

        if (currentChampionLimit < 9)
        {
            currentChampionLimit++;

            currentGold -= 4;

            // uIController.UpdateUI();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        // uIController.UpdateUI();
    }

    public void RestartGame()
    {
        for (int i = 0; i < ownChampionInventoryArray.Length; i++)
        {
            if (ownChampionInventoryArray[i] != null)
            {
                ChampionController championController = ownChampionInventoryArray[i].GetComponent<ChampionController>();

                Destroy(championController.gameObject);
                ownChampionInventoryArray[i] = null;
            }
        }

        for (int x = 0; x < Map.hexMapSizeX; x++)
        {
            for (int z = 0; z < Map.hexMapSizeZ / 2; z++)
            {
                if (gridChampionsArray[x, z] != null)
                {
                    ChampionController championController = gridChampionsArray[x, z].GetComponent<ChampionController>();

                    Destroy(championController.gameObject);
                    gridChampionsArray[x, z] = null;
                }
            }
        }

        currentHP = 100;
        currentGold = 5;
        currentGameStage = GameStage.Preparation;
        currentChampionLimit = 3;
        currentChampionCount = GetChampionCountOnHexGrid();

        // uIController.UpdateUI();

        aIopponent.Restart();

        // uIController.ShowGameScreen();
    }

    public void EndRound()
    {
        timer = CombatStageDuration - 3; //reduce timer so game ends fast
    }

    public void OnChampionDeath()
    {
        bool allDead = IsAllChampionDead();

        if (allDead)
            EndRound();
    }

    public bool IsAllChampionDead()
    {
        int championCount = 0;
        int championDead = 0;
        for (int x = 0; x < Map.hexMapSizeX; x++)
        {
            for (int z = 0; z < Map.hexMapSizeZ / 2; z++)
            {
                if (gridChampionsArray[x, z] != null)
                {
                    ChampionController championController = gridChampionsArray[x, z].GetComponent<ChampionController>();

                    championCount++;

                    if (championController.isDead)
                        championDead++;
                }
            }
        }

        if (championDead == championCount)
            return true;

        return false;
    }
}