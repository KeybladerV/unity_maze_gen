using Components.Maze;
using UnityEngine;
using Vector2 = MazeGenerator.Vector2;

namespace Modules.Maze
{
    public interface IMazePresentationController
    {
        void RegisterMazeView(MazePresentationView view);
        Vector3 GetWorldPosByCell(Vector2 cell);
        Vector3 GetWorldPosByCell(int width, int length);

        Vector2 GetCellByWorldPos(Vector3 worldPos);

        MazeCellView GetCell();
        void ReleaseCell(MazeCellView view);
    }
}