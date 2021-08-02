using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(MapEntitiesSetuper))]
public class DistanceMapCalculator : MonoBehaviour
{
    private MapEntitiesSetuper _setuper;
    private Map _map;
    private PathFinder _pathFinder;

    private void OnEnable()
    {
        _setuper = GetComponent<MapEntitiesSetuper>();
        _map = GetComponent<Map>();
        _pathFinder = GetComponent<PathFinder>();
        
         _setuper.OnNewTarget += CalculateDistance;
    }

    private void OnDisable()
    {
        _setuper.OnNewTarget -= CalculateDistance;
    }

    private void CalculateDistance(Cell cell)
    {
        var cells = _map.Cells;
        var task = Task.Run(() => GoalDistanceCalculation(cells, cell.Position));
        var pathTask = task.ContinueWith((x) => _pathFinder.CalculatePath(_map,cell));
        var vectorFieldTask = pathTask.ContinueWith(x => _pathFinder.GenerateVectorField(_map));

        _map.VectorField = vectorFieldTask.Result;
        _map.DrawVectorField();
    }


    private void GoalDistanceCalculation(Cell[,] cells, Vector3 fromPosition)
    {
        int xSize = cells.GetLength(0);
        int zSize = cells.GetLength(1);

        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < zSize; j++)
            {
                if (cells[i, j].isWall)
                {
                    cells[i, j].GoalDistance = float.MaxValue;
                }
                else
                {
                    var cellPosition = cells[i, j].Position;
                    cells[i, j].GoalDistance = Vector3.Distance(fromPosition, cellPosition);
                    cells[i, j].CellColor = _map.CalculateDistanceColor(cells[i, j].GoalDistance);
                }
            } 
        }
        
        MainThreadWorker.MainThreadActions.Enqueue(() =>
        {
            for (int i = 0; i < xSize; i++)
            {
                for (int j = 0; j < zSize; j++)
                {
                    cells[i, j].ApplyMaterialColor();
                }
            }
        });
    }

    private void DirectionsCalculation(Cell[,] cells)
    {
        int xSize = cells.GetLength(0);
        int zSize = cells.GetLength(1);
        
        for (int x = 0; x < xSize; x++)
        {
            for (int z = 0; z < zSize; z++)
            {
                // TODO: continue to calculate vector feild for cells
            } 
        }
    }

}
