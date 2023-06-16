using UnityEngine;

namespace MazeGenerator
{
    public class MazeCellView : MonoBehaviour
    {
        [SerializeField] private Transform _plane;
        [SerializeField] private Transform _wallUp;
        [SerializeField] private Transform _wallDown;
        [SerializeField] private Transform _wallRight;
        [SerializeField] private Transform _wallLeft;

        /// <summary>Here lies default plane scale (x and z). For unity's plane number is 0.2f to fit. Used to multiply plane scale for cell size changes. </summary>
        [SerializeField] private Vector3 _defaultPlaneScale = new Vector3(0.2f, 1f, 0.2f);
        [SerializeField] private Vector3 _defaultWallScale = new Vector3(2, 2, 0.1f);

        public void SetSize(float size)
        {
            _plane.localScale = 
                new Vector3(_defaultPlaneScale.x * size, _defaultPlaneScale.y , _defaultPlaneScale.z * size);
            
            _wallUp.localScale = _wallDown.localScale = _wallRight.localScale = _wallLeft.localScale = 
                new Vector3(_defaultWallScale.x * size, _defaultWallScale.y, _defaultWallScale.z);

            _wallUp.localPosition = new Vector3(_wallUp.localPosition.x * size, _wallUp.localPosition.y, _wallUp.localPosition.z * size);
            _wallDown.localPosition = new Vector3(_wallDown.localPosition.x * size, _wallDown.localPosition.y, _wallDown.localPosition.z * size);
            _wallRight.localPosition = new Vector3(_wallRight.localPosition.x * size, _wallRight.localPosition.y, _wallRight.localPosition.z * size);
            _wallLeft.localPosition = new Vector3(_wallLeft.localPosition.x * size, _wallLeft.localPosition.y, _wallLeft.localPosition.z * size);
        }

        public void SetState(WallType activeWalls)
        {
            _wallUp.gameObject.SetActive(activeWalls.HasFlag(WallType.Up));
            _wallDown.gameObject.SetActive(activeWalls.HasFlag(WallType.Down));
            _wallRight.gameObject.SetActive(activeWalls.HasFlag(WallType.Right));
            _wallLeft.gameObject.SetActive(activeWalls.HasFlag(WallType.Left));
        }

        public void DestroyUnusedWalls()
        {
            if(!_wallUp.gameObject.activeSelf)
                TryDestroy(_wallUp.gameObject);
            if(!_wallDown.gameObject.activeSelf)
                TryDestroy(_wallDown.gameObject);
            if(!_wallRight.gameObject.activeSelf)
                TryDestroy(_wallRight.gameObject);
            if(!_wallLeft.gameObject.activeSelf)
                TryDestroy(_wallLeft.gameObject);
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