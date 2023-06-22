using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Unity.App.Mediation;
using Components.Maze;
using MazeGenerator;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using Vector2 = MazeGenerator.Vector2;

namespace Modules.Maze.Impl
{
    public sealed class MazePresentationController : IMazePresentationController
    {
        [Inject] public IEventDispatcher Dispatcher { get; set; }
        
        private MazePresentationView _mazeView;
        private IMaze _maze;
        private ObjectPool<MazeCellView> _cellViewPool;

        private MazeCellView _loadedCell;

        [PostConstruct]
        private void OnPostConstruct()
        {
            Dispatcher.AddListener(MazeGeneratorEvents.OnMazeGenerated, maze => _maze = maze);
            
            Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/Cell/MazeCellView.prefab").Completed += handle =>
            {
                _loadedCell = handle.Result.GetComponent<MazeCellView>();
                _cellViewPool = new ObjectPool<MazeCellView>(CreateCellView, null, OnReleaseCellView);
            };
        }
        
        [OnDestroy]
        private void OnDestroy()
        {
            Dispatcher.RemoveListener(MazeGeneratorEvents.OnMazeGenerated, maze => _maze = maze);
            _cellViewPool.Dispose();
        }

        public void RegisterMazeView(MazePresentationView view)
        {
            if(_mazeView != null)
                return;
            _mazeView = view;
        }
        
        public Vector2 GetCellByWorldPos(Vector3 worldPos) => _mazeView.GetCellByWorldPos(worldPos);
        public Vector3 GetWorldPosByCell(Vector2 cell) => _mazeView.GetWorldPosByCell(cell);
        public Vector3 GetWorldPosByCell(int width, int length) => _mazeView.GetWorldPosByCell(width, length);
        public Vector3 GetEntrancePos() => _mazeView.GetWorldPosByCell(_maze.Entrance);
        public Vector3 GetExitPos() => _mazeView.GetWorldPosByCell(_maze.Exit);

        public MazeCellView GetCell() => _cellViewPool.Get();
        public void ReleaseCell(MazeCellView view) => _cellViewPool.Release(view);
        
        private void OnReleaseCellView(MazeCellView view)
        {
            view.SetState(CellType.AllWalls);
            view.gameObject.SetActive(false);
        }

        private MazeCellView CreateCellView()
        {
            var cellView = Object.Instantiate(_loadedCell);
            cellView.gameObject.SetActive(false);
            
            return cellView;
        }
    }
}