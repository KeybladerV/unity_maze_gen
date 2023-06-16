using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ButtonType
{
    NewMaze,
    SolveMaze
}

public abstract class ButtonData
{
}

public class NewMazeButtonData : ButtonData
{
    public int Width { get; set; }
    public int Length { get; set; }
}

public class UIView : MonoBehaviour
{
    [SerializeField] private TMP_InputField _widthInputField;
    [SerializeField] private TMP_InputField _lengthInputField;
    [SerializeField] private Button _newMazeButton;
    [SerializeField] private Button _solveMazeButton;

    public event Action<ButtonType, ButtonData> OnButtonClicked;

    public void SetSolveButtonInteractable(bool b) => _solveMazeButton.interactable = b;

    public void SetGenerateButtonInteractable(bool b) => _newMazeButton.interactable = b;

    private void Awake()
    {
        _newMazeButton.onClick.AddListener(OnGenerateButtonClick);
        _solveMazeButton.onClick.AddListener(OnSolveButtonClick);
    }

    private void OnDestroy()
    {
        _newMazeButton.onClick.RemoveListener(OnGenerateButtonClick);
        _solveMazeButton.onClick.RemoveListener(OnSolveButtonClick);
    }

    private void OnSolveButtonClick() => OnButtonClicked?.Invoke(ButtonType.SolveMaze, null);

    private void OnGenerateButtonClick()
    {
        NewMazeButtonData newMazeButtonData;
        
        if (int.TryParse(_widthInputField.text, out var width) && int.TryParse(_lengthInputField.text, out var length))
            newMazeButtonData = new NewMazeButtonData() { Length = length, Width = width };
        else
            newMazeButtonData = new NewMazeButtonData() { Length = 10, Width = 10 };
        
        OnButtonClicked?.Invoke(ButtonType.NewMaze, newMazeButtonData);
    }
}