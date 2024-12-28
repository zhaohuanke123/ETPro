using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController: MonoBehaviour
{
    public GamePlayController gamePlayController;

    public Map map;

    public LayerMask triggerLayer;

    private Vector3 rayCastStartPosition;

    void Start()
    {
        rayCastStartPosition = new Vector3(0, 20, 0);
    }

    private Vector3 mousePosition;

    [HideInInspector]
    public TriggerInfo triggerInfo = null;

    void Update()
    {
        triggerInfo = null;
        map.resetIndicators();

        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100f, triggerLayer, QueryTriggerInteraction.Collide))
        {
            triggerInfo = hit.collider.gameObject.GetComponent<TriggerInfo>();

            if (triggerInfo != null)
            {
                GameObject indicator = map.GetIndicatorFromTriggerInfo(triggerInfo);

                indicator.GetComponent<MeshRenderer>().material.color = map.indicatorActiveColor;
            }
            else
                map.resetIndicators();
        }

        if (Input.GetMouseButtonDown(0))
        {
            gamePlayController.StartDrag();
        }

        if (Input.GetMouseButtonUp(0))
        {
            gamePlayController.StopDrag();
        }

        mousePosition = Input.mousePosition;
    }
}