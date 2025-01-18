﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChampionController: MonoBehaviour
{
    public static int TEAMID_PLAYER = 0;
    public static int TEAMID_AI = 1;

    public GameObject levelupEffectPrefab;
    public GameObject projectileStart;

    [HideInInspector]
    public int gridType = 0;

    [HideInInspector]
    public int gridPositionX = 0;

    [HideInInspector]
    public int gridPositionZ = 0;

    [HideInInspector]
    public int teamID = 0;

    // [HideInInspector]
    // public Champion champion;

    [HideInInspector]
    public float maxHealth = 0;

    [HideInInspector]
    public float currentHealth = 0;

    [HideInInspector]
    public float currentDamage = 0;

    [HideInInspector]
    public int lvl = 1;

    private Map map;
    private GamePlayController gamePlayController;
    private AIopponent aIopponent;
    private ChampionAnimation championAnimation;
    private WorldCanvasController worldCanvasController;

    private NavMeshAgent navMeshAgent;

    private Vector3 gridTargetPosition;

    private bool _isDragged = false;

    [HideInInspector]
    public bool isAttacking = false;

    [HideInInspector]
    public bool isDead = false;

    private bool isInCombat = false;
    private float combatTimer = 0;

    private bool isStuned = false;
    private float stunTimer = 0;

    private List<Effect> effects;

    public void Init(Champion _champion, int _teamID)
    {
        // champion = _champion;
        teamID = _teamID;

        map = GameObject.Find("Scripts").GetComponent<Map>();
        aIopponent = GameObject.Find("Scripts").GetComponent<AIopponent>();
        gamePlayController = GameObject.Find("Scripts").GetComponent<GamePlayController>();
        worldCanvasController = GameObject.Find("Scripts").GetComponent<WorldCanvasController>();
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        championAnimation = this.GetComponent<ChampionAnimation>();

        navMeshAgent.enabled = false;

        // maxHealth = champion.health;
        // currentHealth = champion.health;
        // currentDamage = champion.damage;

        worldCanvasController.AddHealthBar(this.gameObject);

        effects = new List<Effect>();
    }

    private void Update()
    {
        if (_isDragged)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float enter = 100.0f;
            if (map.m_Plane.Raycast(ray, out enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);

                Vector3 p = new Vector3(hitPoint.x, 1.0f, hitPoint.z);

                this.transform.position = Vector3.Lerp(this.transform.position, p, 0.1f);
            }
        }
        else
        {
            // if (gamePlayController.currentGameStage == GameStage.Preparation)
            // {
                float distance = Vector3.Distance(gridTargetPosition, this.transform.position);

                if (distance > 0.25f)
                {
                    this.transform.position = Vector3.Lerp(this.transform.position, gridTargetPosition, 0.1f);
                }
                else
                {
                    this.transform.position = gridTargetPosition;
                }
            // }
        }

        // if (isInCombat && isStuned == false)
        // {
        //     if (target == null)
        //     {
        //         combatTimer += Time.deltaTime;
        //         if (combatTimer > 0.5f)
        //         {
        //             combatTimer = 0;
        //
        //             TryAttackNewTarget();
        //         }
        //     }
        //
        //     if (target != null)
        //     {
        //         this.transform.LookAt(target.transform, Vector3.up);
        //
        //         if (target.GetComponent<ChampionController>().isDead == true)
        //         {
        //             target = null;
        //             navMeshAgent.isStopped = true;
        //         }
        //         else
        //         {
        //             if (isAttacking == false)
        //             {
        //                 float distance = Vector3.Distance(this.transform.position, target.transform.position);
        //
        //                 if (distance < champion.attackRange)
        //                 {
        //                     DoAttack();
        //                 }
        //                 else
        //                 {
        //                     navMeshAgent.destination = target.transform.position;
        //                 }
        //             }
        //         }
        //     }
        // }
        //
        // if (isStuned)
        // {
        //     stunTimer -= Time.deltaTime;
        //
        //     if (stunTimer < 0)
        //     {
        //         isStuned = false;
        //
        //         championAnimation.IsAnimated(true);
        //
        //         if (target != null)
        //         {
        //             navMeshAgent.destination = target.transform.position;
        //
        //             navMeshAgent.isStopped = false;
        //         }
        //     }
        // }
    }

    public bool IsDragged
    {
        get
        {
            return _isDragged;
        }
        set
        {
            _isDragged = value;
        }
    }

    public void Reset()
    {
        this.gameObject.SetActive(true);

        // maxHealth = champion.health * lvl;
        // currentHealth = champion.health * lvl;
        isDead = false;
        isInCombat = false;
        target = null;
        isAttacking = false;

        SetWorldPosition();
        SetWorldRotation();

        foreach (Effect e in effects)
        {
            e.Remove();
        }

        effects = new List<Effect>();
    }

    public void SetGridPosition(int _gridType, int _gridPositionX, int _gridPositionZ)
    {
        gridType = _gridType;
        gridPositionX = _gridPositionX;
        gridPositionZ = _gridPositionZ;

        gridTargetPosition = GetWorldPosition();
    }

    public Vector3 GetWorldPosition()
    {
        Vector3 worldPosition = Vector3.zero;

        if (gridType == Map.GRIDTYPE_OWN_INVENTORY)
        {
            worldPosition = map.ownInventoryGridPositions[gridPositionX];
        }
        else if (gridType == Map.GRIDTYPE_HEXA_MAP)
        {
            worldPosition = map.mapGridPositions[gridPositionX, gridPositionZ];
        }

        return worldPosition;
    }

    public void SetWorldPosition()
    {
        navMeshAgent.enabled = false;

        Vector3 worldPosition = GetWorldPosition();

        this.transform.position = worldPosition;

        gridTargetPosition = worldPosition;
    }

    public void SetWorldRotation()
    {
        Vector3 rotation = Vector3.zero;

        if (teamID == 0)
        {
            rotation = new Vector3(0, 200, 0);
        }
        else if (teamID == 1)
        {
            rotation = new Vector3(0, 20, 0);
        }

        this.transform.rotation = Quaternion.Euler(rotation);
    }

    public void UpgradeLevel()
    {
        // lvl++;
        //
        // float newSize = 1;
        // maxHealth = champion.health;
        // currentHealth = champion.health;
        //
        // if (lvl == 2)
        // {
        //     newSize = 1.5f;
        //     // maxHealth = champion.health * 2;
        //     // currentHealth = champion.health * 2;
        //     // currentDamage = champion.damage * 2;
        // }
        //
        // if (lvl == 3)
        // {
        //     newSize = 2f;
        //     // maxHealth = champion.health * 3;
        //     // currentHealth = champion.health * 3;
        //     // currentDamage = champion.damage * 3;
        // }
        //
        // this.transform.localScale = new Vector3(newSize, newSize, newSize);
        //
        // GameObject levelupEffect = Instantiate(levelupEffectPrefab);
        //
        // levelupEffect.transform.position = this.transform.position;
        //
        // Destroy(levelupEffect, 1.0f);
    }

    private GameObject target;

    private GameObject FindTarget()
    {
        GameObject closestEnemy = null;
        float bestDistance = 1000;

        if (teamID == TEAMID_PLAYER)
        {
            for (int x = 0; x < Map.hexMapSizeX; x++)
            {
                for (int z = 0; z < Map.hexMapSizeZ / 2; z++)
                {
                    if (aIopponent.gridChampionsArray[x, z] != null)
                    {
                        ChampionController championController = aIopponent.gridChampionsArray[x, z].GetComponent<ChampionController>();

                        if (championController.isDead == false)
                        {
                            float distance = Vector3.Distance(this.transform.position, aIopponent.gridChampionsArray[x, z].transform.position);

                            if (distance < bestDistance)
                            {
                                bestDistance = distance;
                                closestEnemy = aIopponent.gridChampionsArray[x, z];
                            }
                        }
                    }
                }
            }
        }
        else if (teamID == TEAMID_AI)
        {
            for (int x = 0; x < Map.hexMapSizeX; x++)
            {
                for (int z = 0; z < Map.hexMapSizeZ / 2; z++)
                {
                    if (gamePlayController.gridChampionsArray[x, z] != null)
                    {
                        ChampionController championController = gamePlayController.gridChampionsArray[x, z].GetComponent<ChampionController>();

                        if (championController.isDead == false)
                        {
                            float distance = Vector3.Distance(this.transform.position,
                                gamePlayController.gridChampionsArray[x, z].transform.position);

                            if (distance < bestDistance)
                            {
                                bestDistance = distance;
                                closestEnemy = gamePlayController.gridChampionsArray[x, z];
                            }
                        }
                    }
                }
            }
        }

        return closestEnemy;
    }

    private void TryAttackNewTarget()
    {
        target = FindTarget();

        if (target != null)
        {
            navMeshAgent.destination = target.transform.position;

            navMeshAgent.isStopped = false;
        }
    }

    public void OnCombatStart()
    {
        IsDragged = false;

        this.transform.position = gridTargetPosition;

        if (gridType == Map.GRIDTYPE_HEXA_MAP)
        {
            isInCombat = true;

            navMeshAgent.enabled = true;

            TryAttackNewTarget();
        }
    }

    private void DoAttack()
    {
        isAttacking = true;

        navMeshAgent.isStopped = true;

        championAnimation.DoAttack(true);
    }

    public void OnAttackAnimationFinished()
    {
        isAttacking = false;

        // if (target != null)
        // {
        //     ChampionController targetChamoion = target.GetComponent<ChampionController>();
        //
        //     List<ChampionBonus> activeBonuses = null;
        //
        //     if (teamID == TEAMID_PLAYER)
        //         activeBonuses = gamePlayController.activeBonusList;
        //     else if (teamID == TEAMID_AI)
        //         activeBonuses = aIopponent.activeBonusList;
        //
        //     float d = 0;
        //     foreach (ChampionBonus b in activeBonuses)
        //     {
        //         d += b.ApplyOnAttack(this, targetChamoion);
        //     }
        //
        //     bool isTargetDead = targetChamoion.OnGotHit(d + currentDamage);
        //
        //     if (isTargetDead)
        //         TryAttackNewTarget();
        //
        //     if (champion.attackProjectile != null && projectileStart != null)
        //     {
        //         GameObject projectile = Instantiate(champion.attackProjectile);
        //         projectile.transform.position = projectileStart.transform.position;
        //
        //         projectile.GetComponent<Projectile>().Init(target);
        //     }
        // }
    }

    public bool OnGotHit(float damage)
    {
        List<ChampionBonus> activeBonuses = null;

        if (teamID == TEAMID_PLAYER)
            activeBonuses = gamePlayController.activeBonusList;
        else if (teamID == TEAMID_AI)
            activeBonuses = aIopponent.activeBonusList;

        foreach (ChampionBonus b in activeBonuses)
        {
            damage = b.ApplyOnGotHit(this, damage);
        }

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            this.gameObject.SetActive(false);
            isDead = true;

            aIopponent.OnChampionDeath();
            gamePlayController.OnChampionDeath();
        }

        worldCanvasController.AddDamageText(this.transform.position + new Vector3(0, 2.5f, 0), damage);

        return isDead;
    }

    public void OnGotStun(float duration)
    {
        isStuned = true;
        stunTimer = duration;

        championAnimation.IsAnimated(false);

        navMeshAgent.isStopped = true;
    }

    public void OnGotHeal(float f)
    {
        currentHealth += f;
    }

    public void AddEffect(GameObject effectPrefab, float duration)
    {
        if (effectPrefab == null)
            return;

        bool foundEffect = false;
        foreach (Effect e in effects)
        {
            if (effectPrefab == e.effectPrefab)
            {
                e.duration = duration;
                foundEffect = true;
            }
        }

        if (foundEffect == false)
        {
            Effect effect = this.gameObject.AddComponent<Effect>();
            effect.Init(effectPrefab, this.gameObject, duration);
            effects.Add(effect);
        }
    }

    public void RemoveEffect(Effect effect)
    {
        effects.Remove(effect);
        effect.Remove();
    }
}