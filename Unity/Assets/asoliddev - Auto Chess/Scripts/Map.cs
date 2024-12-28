using UnityEngine;

public class Map: MonoBehaviour
{
    public static int GRIDTYPE_OWN_INVENTORY = 0;
    public static int GRIDTYPE_OPONENT_INVENTORY = 1;
    public static int GRIDTYPE_HEXA_MAP = 2;

    public static int hexMapSizeX = 7;
    public static int hexMapSizeZ = 8;
    public static int inventorySize = 9;

    public Plane m_Plane;

    public Transform ownInventoryStartPosition;
    public Transform oponentInventoryStartPosition;
    public Transform mapStartPosition;

    public GameObject squareIndicator;
    public GameObject hexaIndicator;

    public Color indicatorDefaultColor;
    public Color indicatorActiveColor;

    void Start()
    {
        CreateGridPosition();
        CreateIndicators();
        HideIndicators();

        m_Plane = new Plane(Vector3.up, Vector3.zero);

        this.SendMessage("OnMapReady", SendMessageOptions.DontRequireReceiver);
    }

    [HideInInspector]
    public Vector3[] ownInventoryGridPositions;

    [HideInInspector]
    public Vector3[] oponentInventoryGridPositions;

    [HideInInspector]
    public Vector3[,] mapGridPositions;

    private void CreateGridPosition()
    {
        ownInventoryGridPositions = new Vector3[inventorySize];
        oponentInventoryGridPositions = new Vector3[inventorySize];
        mapGridPositions = new Vector3[hexMapSizeX, hexMapSizeZ];

        for (int i = 0; i < inventorySize; i++)
        {
            float offsetX = i * -2.5f;

            Vector3 position = GetMapHitPoint(ownInventoryStartPosition.position + new Vector3(offsetX, 0, 0));

            ownInventoryGridPositions[i] = position;
        }

        for (int i = 0; i < inventorySize; i++)
        {
            float offsetX = i * -2.5f;

            Vector3 position = GetMapHitPoint(oponentInventoryStartPosition.position + new Vector3(offsetX, 0, 0));

            oponentInventoryGridPositions[i] = position;
        }

        for (int x = 0; x < hexMapSizeX; x++)
        {
            for (int z = 0; z < hexMapSizeZ; z++)
            {
                int rowOffset = z % 2;

                float offsetX = x * -3f + rowOffset * 1.5f;
                float offsetZ = z * -2.5f;

                Vector3 position = GetMapHitPoint(mapStartPosition.position + new Vector3(offsetX, 0, offsetZ));

                mapGridPositions[x, z] = position;
            }
        }
    }

    [HideInInspector]
    public GameObject[] ownIndicatorArray;

    [HideInInspector]
    public GameObject[] oponentIndicatorArray;

    [HideInInspector]
    public GameObject[,] mapIndicatorArray;

    [HideInInspector]
    public TriggerInfo[] ownTriggerArray;

    [HideInInspector]
    public TriggerInfo[,] mapGridTriggerArray;

    private GameObject indicatorContainer;

    private void CreateIndicators()
    {
        indicatorContainer = new GameObject();
        indicatorContainer.name = "IndicatorContainer";

        GameObject triggerContainer = new GameObject();
        triggerContainer.name = "TriggerContainer";

        ownIndicatorArray = new GameObject[inventorySize];
        oponentIndicatorArray = new GameObject[inventorySize];
        mapIndicatorArray = new GameObject[hexMapSizeX, hexMapSizeZ / 2];

        ownTriggerArray = new TriggerInfo[inventorySize];
        mapGridTriggerArray = new TriggerInfo[hexMapSizeX, hexMapSizeZ / 2];

        for (int i = 0; i < inventorySize; i++)
        {
            GameObject indicatorGO = Instantiate(squareIndicator);

            indicatorGO.transform.position = ownInventoryGridPositions[i];

            indicatorGO.transform.parent = indicatorContainer.transform;

            ownIndicatorArray[i] = indicatorGO;

            GameObject trigger = CreateBoxTrigger(GRIDTYPE_OWN_INVENTORY, i);

            trigger.transform.parent = triggerContainer.transform;

            trigger.transform.position = ownInventoryGridPositions[i];

            ownTriggerArray[i] = trigger.GetComponent<TriggerInfo>();
        }

        /*
        for (int i = 0; i < inventorySize; i++)
        {
            GameObject indicatorGO = Instantiate(squareIndicator);

            indicatorGO.transform.position = oponentInventoryGridPositions[i];

            indicatorGO.transform.parent = indicatorContainer.transform;

            oponentIndicatorArray[i] = indicatorGO;


        }
        */

        for (int x = 0; x < hexMapSizeX; x++)
        {
            for (int z = 0; z < hexMapSizeZ / 2; z++)
            {
                GameObject indicatorGO = Instantiate(hexaIndicator);

                indicatorGO.transform.position = mapGridPositions[x, z];

                indicatorGO.transform.parent = indicatorContainer.transform;

                mapIndicatorArray[x, z] = indicatorGO;

                GameObject trigger = CreateSphereTrigger(GRIDTYPE_HEXA_MAP, x, z);

                trigger.transform.parent = triggerContainer.transform;

                trigger.transform.position = mapGridPositions[x, z];

                mapGridTriggerArray[x, z] = trigger.GetComponent<TriggerInfo>();
            }
        }
    }

    public Vector3 GetMapHitPoint(Vector3 p)
    {
        Vector3 newPos = p;

        RaycastHit hit;

        if (Physics.Raycast(newPos + new Vector3(0, 10, 0), Vector3.down, out hit, 15))
        {
            newPos = hit.point;
        }

        return newPos;
    }

    private GameObject CreateBoxTrigger(int type, int x)
    {
        GameObject trigger = new GameObject();

        BoxCollider collider = trigger.AddComponent<BoxCollider>();

        collider.size = new Vector3(2, 0.5f, 2);

        collider.isTrigger = true;

        TriggerInfo trigerInfo = trigger.AddComponent<TriggerInfo>();
        trigerInfo.gridType = type;
        trigerInfo.gridX = x;

        trigger.layer = LayerMask.NameToLayer("Triggers");

        return trigger;
    }

    private GameObject CreateSphereTrigger(int type, int x, int z)
    {
        GameObject trigger = new GameObject();

        SphereCollider collider = trigger.AddComponent<SphereCollider>();

        collider.radius = 1.4f;

        collider.isTrigger = true;

        TriggerInfo trigerInfo = trigger.AddComponent<TriggerInfo>();
        trigerInfo.gridType = type;
        trigerInfo.gridX = x;
        trigerInfo.gridZ = z;

        trigger.layer = LayerMask.NameToLayer("Triggers");

        return trigger;
    }

    public GameObject GetIndicatorFromTriggerInfo(TriggerInfo triggerinfo)
    {
        GameObject triggerGo = null;

        if (triggerinfo.gridType == GRIDTYPE_OWN_INVENTORY)
        {
            triggerGo = ownIndicatorArray[triggerinfo.gridX];
        }
        else if (triggerinfo.gridType == GRIDTYPE_OPONENT_INVENTORY)
        {
            triggerGo = oponentIndicatorArray[triggerinfo.gridX];
        }
        else if (triggerinfo.gridType == GRIDTYPE_HEXA_MAP)
        {
            triggerGo = mapIndicatorArray[triggerinfo.gridX, triggerinfo.gridZ];
        }

        return triggerGo;
    }

    public void resetIndicators()
    {
        for (int x = 0; x < hexMapSizeX; x++)
        {
            for (int z = 0; z < hexMapSizeZ / 2; z++)
            {
                mapIndicatorArray[x, z].GetComponent<MeshRenderer>().material.color = indicatorDefaultColor;
            }
        }

        for (int x = 0; x < 9; x++)
        {
            ownIndicatorArray[x].GetComponent<MeshRenderer>().material.color = indicatorDefaultColor;
            // oponentIndicatorArray[x].GetComponent<MeshRenderer>().material.color = indicatorDefaultColor;
        }
    }

    public void ShowIndicators()
    {
        indicatorContainer.SetActive(true);
    }

    public void HideIndicators()
    {
        indicatorContainer.SetActive(false);
    }
}