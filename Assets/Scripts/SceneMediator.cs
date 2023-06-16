using System;
using UnityEngine;

namespace MazeGenerator
{
    public class SceneMediator : MonoBehaviour
    {
        [SerializeField] private UIView uiView;
        [SerializeField] private MazeGeneration _mazeGeneration;
        [SerializeField] private CharacterView _characterView;
        
        private void Awake()
        {
            uiView.OnButtonClicked += OnButtonClicked;
            _mazeGeneration.OnMazeGenerated += OnMazeGenerated;
            _characterView.OnMoveStateChanged += OnMoveStateChanged;
            
            _characterView.SetMazeView(_mazeGeneration.CurrentMazeView);
            _characterView.SetPositionToEntrance();
        }
        
        private void OnDestroy()
        {
            uiView.OnButtonClicked -= OnButtonClicked;
            _mazeGeneration.OnMazeGenerated -= OnMazeGenerated;
            _characterView.OnMoveStateChanged -= OnMoveStateChanged;
        }

        private void OnMoveStateChanged(bool isMoving)
        {
            uiView.SetSolveButtonInteractable(!isMoving);
            uiView.SetGenerateButtonInteractable(!isMoving);
        }

        private void OnMazeGenerated()
        {
            _characterView.SetMazeView(_mazeGeneration.CurrentMazeView);
            _characterView.SetPositionToEntrance();
            uiView.SetGenerateButtonInteractable(true);
        }

        private void OnButtonClicked(ButtonType buttonType, ButtonData buttonData)
        {
            switch (buttonType)
            {
                case ButtonType.NewMaze:
                    var newMazeButtonData = (NewMazeButtonData) buttonData;
                    _mazeGeneration.GenerateAndDrawMaze(newMazeButtonData.Width, newMazeButtonData.Length);
                    break;
                case ButtonType.SolveMaze:
                    _characterView.MoveTo(_mazeGeneration.CurrentMazeView.Maze.Exit);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(buttonType), buttonType, null);
            }
        }
    }
}