using System;
using Build1.PostMVC.Core.MVCS.Mediation;
using Build1.PostMVC.Unity.App.Mediation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Event = Build1.PostMVC.Core.MVCS.Events.Event;
using Vector2 = MazeGenerator.Vector2;

namespace Components.Screens.Main
{
    [Mediator(typeof(MainScreenMediator))]
    public sealed class MainScreenView : UnityViewDispatcher
    {
        public readonly Event                          OnNewMaze           = new(typeof(MainScreenView), nameof(OnNewMaze));
        public readonly Event                          OnSaveMaze          = new(typeof(MainScreenView), nameof(OnSaveMaze));
        public readonly Event                          OnToCoordinates     = new(typeof(MainScreenView), nameof(OnToCoordinates));
        public readonly Event                          OnToEntrance        = new(typeof(MainScreenView), nameof(OnToEntrance));
        public readonly Event                          OnToExit            = new(typeof(MainScreenView), nameof(OnToExit));
        public readonly Event                          OnToRandom          = new(typeof(MainScreenView), nameof(OnToRandom));
        
        [Header("For maze generation")] 
        [SerializeField] private TMP_InputField _widthInputField;
        [SerializeField] private TMP_InputField _lengthInputField;
        [SerializeField] private Button _newMazeButton;
        [SerializeField] private Button _saveMazeButton;

        [Header("For Pathfinding")] 
        [SerializeField] private TMP_InputField _xPathfindInput;
        [SerializeField] private TMP_InputField _yPathfindInput;
        [SerializeField] private Button _toCoordinatesButton;
        [SerializeField] private Button _toEntranceButton;
        [SerializeField] private Button _toExitButton;
        [SerializeField] private Button _toRandomButton;

        [Start]
        public void OnStart()
        {
            BindUnityEvent(_newMazeButton.onClick, OnNewMaze);
            BindUnityEvent(_saveMazeButton.onClick, OnSaveMaze);
            BindUnityEvent(_toCoordinatesButton.onClick, OnToCoordinates);
            BindUnityEvent(_toEntranceButton.onClick, OnToEntrance);
            BindUnityEvent(_toExitButton.onClick, OnToExit);
            BindUnityEvent(_toRandomButton.onClick, OnToRandom);
        }
        
        [OnDestroy]
        private void OnDestroying()
        {
            UnbindAllUnityEvents();
        }

        public void SetState(MainScreenState state)
        {
            switch (state)
            {
                case MainScreenState.None:
                    break;
                
                case MainScreenState.EnableAll:
                    _newMazeButton.interactable = true;
                    _saveMazeButton.interactable = true;
                    _toCoordinatesButton.interactable = true;
                    _toEntranceButton.interactable = true;
                    _toExitButton.interactable = true;
                    _toRandomButton.interactable = true;
                    break;
                
                case MainScreenState.DisableAll:
                    _newMazeButton.interactable = false;
                    _saveMazeButton.interactable = false;
                    _toCoordinatesButton.interactable = false;
                    _toEntranceButton.interactable = false;
                    _toExitButton.interactable = false;
                    _toRandomButton.interactable = false;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public Vector2 GetMazeGenData()
        {
            Vector2 vector2;

            if (int.TryParse(_widthInputField.text, out var width) &&
                int.TryParse(_lengthInputField.text, out var length))
                vector2 = new Vector2() { Y = length, X = width };
            else
                vector2 = new Vector2() { Y = 10, X = 10 };

            return vector2;
        }

        public Vector2 GetPathfindingData()
        {
            Vector2 vector2;

            if (int.TryParse(_xPathfindInput.text, out var width) &&
                int.TryParse(_yPathfindInput.text, out var length))
                vector2 = new Vector2() { Y = length, X = width };
            else
                vector2 = new Vector2() { Y = 10, X = 10 };

            return vector2;
        }
    }
}