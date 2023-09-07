using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private float wheelScale, wheelImageSpinSpeed;
    [SerializeField] private GameObject menuButtonPrefab;
    [SerializeField] private RectTransform carouselBoxTransform, wheelImageTransform;
    [SerializeField] private ButtonFunction[] buttonProperties;
    [SerializeField] private RectTransform[] menuButtonsPositions;
    [SerializeField] private float selectedFontSize, unselectedFontSize;

    private Button[] buttons;
    private float currentScrollValue = 0;
    private int currentSelectedOption;

    private void Awake()
    {
        if(buttonProperties.Length != menuButtonsPositions.Length)
        {
            return;
        }

        InitialiseButtons();
    }

    private void InitialiseButtons()
    {
        buttons = new Button[buttonProperties.Length];

        for (int i = 0; i < buttonProperties.Length; i++)
        {
            GameObject go = Instantiate(menuButtonPrefab, carouselBoxTransform);
            RectTransform rectTransform = go.GetComponent<RectTransform>();
            TextMeshProUGUI tmpro = go.GetComponent<TextMeshProUGUI>();
            Button button = go.GetComponent<Button>();

            rectTransform.anchoredPosition = menuButtonsPositions[i].anchoredPosition;
            tmpro.fontSize = buttonProperties[i].startsAsSelected ? selectedFontSize : unselectedFontSize;
            tmpro.text = buttonProperties[i].buttonText;
            if (buttonProperties[i].startsAsSelected)
            {
                tmpro.color = Color.red;
                currentSelectedOption = i;
            }

            switch (buttonProperties[i].buttonFunction)
            {
                case BUTTON_FUNCTION.QUIT:
                    button.onClick.AddListener(() => QuitGame());
                    break;

                case BUTTON_FUNCTION.OPTIONS:
                    button.onClick.AddListener(() => ToggleOptionsScreens());
                    break;

                case BUTTON_FUNCTION.PLAY:
                    button.onClick.AddListener(() => StartGame());
                    break;

                case BUTTON_FUNCTION.CONTROLS:
                    button.onClick.AddListener(() => ToggleControlsScreens());
                    break;
            }
            buttons[i] = button;
        }
    }

    public void MenuScroll(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        if (value == 0f)
            return;

        //value /= Mathf.Abs(value);
        currentScrollValue += value * wheelScale;
        if(currentScrollValue > 1)
        {
            UnselectOption(currentSelectedOption);
            currentSelectedOption = (int)Mathf.Repeat(currentSelectedOption + 1, buttonProperties.Length);
            SelectOption(currentSelectedOption);
            currentScrollValue = 0;
        }
        else if (currentScrollValue < -1)
        {
            UnselectOption(currentSelectedOption);
            currentSelectedOption = (int)Mathf.Repeat(currentSelectedOption - 1, buttonProperties.Length);
            SelectOption(currentSelectedOption);
            currentScrollValue = 0;
        }

        RollWheel(value);
    }

    private void RollWheel(float value)
    {
        wheelImageTransform.localRotation *= Quaternion.Euler(0, 0, -value * wheelImageSpinSpeed);
    }

    private void UnselectOption(int index)
    {
        TextMeshProUGUI tmpro = buttons[index].GetComponent<TextMeshProUGUI>();
        tmpro.fontSize = unselectedFontSize;
        tmpro.color = Color.white;
    }

    private void SelectOption(int index)
    {
        TextMeshProUGUI tmpro = buttons[index].GetComponent<TextMeshProUGUI>();
        tmpro.fontSize = selectedFontSize;
        tmpro.color = Color.red;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void StartGame()
    {
        Debug.Log("The Game Starts.");
    }

    public void ToggleControlsScreens()
    {
        Debug.Log("ouverture du screen de controles.");
    }

    public void ToggleOptionsScreens()
    {
        Debug.Log("ouverture du screen des options.");
    }
}

[System.Serializable]
struct ButtonFunction
{
    public string buttonText;
    public BUTTON_FUNCTION buttonFunction;
    public bool startsAsSelected;
}

enum BUTTON_FUNCTION
{
    QUIT,
    OPTIONS,
    PLAY,
    CONTROLS
}
