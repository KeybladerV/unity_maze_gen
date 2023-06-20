using MazeGenerator;
using UnityEngine;

namespace Components.Maze
{
    public sealed class MazeCellView : MonoBehaviour
    {
        [SerializeField] private Transform _plane;
        [SerializeField] private Transform _wallUp;
        [SerializeField] private Transform _wallDown;
        [SerializeField] private Transform _wallRight;
        [SerializeField] private Transform _wallLeft;

        /// <summary>Here lies default plane scale (x and z). For unity's plane number is 0.2f to fit. Used to multiply plane scale for cell size changes. </summary>
        [SerializeField] private Vector3 _defaultPlaneScale = new(0.2f, 1f, 0.2f);
        [SerializeField] private Vector3 _defaultWallScale = new(2, 2, 0.1f);

        public void SetSize(float size)
        {
            _plane.localScale = 
                new Vector3(_defaultPlaneScale.x * size, _defaultPlaneScale.y , _defaultPlaneScale.z * size);
            
            _wallUp.localScale = _wallDown.localScale = _wallRight.localScale = _wallLeft.localScale = 
                new Vector3(_defaultWallScale.x * size, _defaultWallScale.y, _defaultWallScale.z);
            
            AdjustWallLocalPosition(_wallUp, size);
            AdjustWallLocalPosition(_wallDown, size);
            AdjustWallLocalPosition(_wallRight, size);
            AdjustWallLocalPosition(_wallLeft, size);
        }

        public void SetState(CellType activeCells)
        {
            _wallUp.gameObject.SetActive((activeCells & CellType.Up) != 0);
            _wallDown.gameObject.SetActive((activeCells & CellType.Down) != 0);
            _wallRight.gameObject.SetActive((activeCells & CellType.Right) != 0);
            _wallLeft.gameObject.SetActive((activeCells & CellType.Left) != 0);
        }
        
        private void AdjustWallLocalPosition(Transform wall, float size)
        {
            var localPosition = wall.localPosition;
            localPosition = new Vector3(localPosition.x * size, localPosition.y, localPosition.z * size);
            wall.localPosition = localPosition;
        }

        private void TryDestroy(GameObject gO)
        {
            if(Application.isPlaying)
                Destroy(gO);
            else
                DestroyImmediate(gO);
        }
    }
}