using System;
using System.Linq;
using UnityEngine;

public class MapEntitiesSetuper : MonoBehaviour
{
    public event Action<Cell> OnNewTarget;
    public event Action<Cell> OnWallCreate;
    
    private const float RaycastDistance = 100;
    
    [SerializeField] private LayerMask _mapLayerMask;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var cell = CheckForCell(ray);

            if (cell)
            {
                OnWallCreate?.Invoke(cell);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var cell = CheckForCell(ray);

            if (cell)
            {
                OnNewTarget?.Invoke(cell);
            }
        }
    }

    private Cell CheckForCell(Ray ray)
    {
        RaycastHit[] hits = new RaycastHit[2];
        int hitCount = Physics.RaycastNonAlloc(ray, hits, RaycastDistance, _mapLayerMask.value);

        var cell = hits.Select(x => x.collider.GetComponent<PathCellComponent>()).FirstOrDefault();

        return cell.Cell;
    }
}
