using UnityEngine;

public class InputController: MonoBehaviour
{
    private void Start()
    {
        this.mainCamera = Camera.main;
        rayCastStartPosition = new Vector3(0, 20, 0);
    }

    private void Update()
    {
        triggerInfo = null;
        Map.Instance.resetIndicators();

        RaycastHit hit;

        Ray ray = this.mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100f, triggerLayer, QueryTriggerInteraction.Collide))
        {
            triggerInfo = hit.collider.gameObject.GetComponent<TriggerInfo>();
            // Debug.Log($"this.triggerInfo : {this.triggerInfo}");

            if (triggerInfo != null)
            {
                GameObject indicator = Map.Instance.GetIndicatorFromTriggerInfo(triggerInfo);

                indicator.GetComponent<MeshRenderer>().material.color = Map.Instance.indicatorActiveColor;
            }
            else
            {
                Map.Instance.resetIndicators();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            gamePlayController.StartDrag(this.triggerInfo);
        }

        if (Input.GetMouseButtonUp(0))
        {
            gamePlayController.StopDrag(this.triggerInfo);
        }

        mousePosition = Input.mousePosition;
    }

    public GamePlayController gamePlayController;

    public LayerMask triggerLayer;

    private Vector3 rayCastStartPosition;

    private Vector3 mousePosition;

    [HideInInspector]
    public TriggerInfo triggerInfo = null;

    private Camera mainCamera;
}