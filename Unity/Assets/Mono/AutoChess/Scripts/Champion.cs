using UnityEngine;

[CreateAssetMenu(fileName = "DefaultChampion", menuName = "AutoChess/Champion", order = 1)]
public class Champion: ScriptableObject
{
	public GameObject prefab;

	public GameObject attackProjectile;

	public string uiname;

	public int cost;

	public ChampionType type1;

	public ChampionType type2;

	public float health = 100;

	public float damage = 10;

	public float attackRange = 1;
}
