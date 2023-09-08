using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private float wheelScale, wheelImageSpinSpeed, timeToWaitOnControlsScreen;
    [SerializeField] private GameObject menuButtonPrefab, controlsScreen;
    [SerializeField] private RectTransform carouselBoxTransform, wheelImageTransform;
    [SerializeField] private ButtonFunction[] buttonProperties;
    [SerializeField] private RectTransform[] menuButtonsPositions;
    [SerializeField] private float selectedButtonSize, unselectedButtonSize;

    private Button[] buttons;
    private float currentScrollValue = 0;
    private int currentSelectedOption;
    private Coroutine startCoroutine;

    private void Awake()
    {
        if(buttonProperties.Length != menuButtonsPositions.Length)
        {
            return;
        }

        InitialiseButtons();

        Cursor.visible = false;
    }

    private void InitialiseButtons()
    {
        buttons = new Button[buttonProperties.Length];

        for (int i = 0; i < buttonProperties.Length; i++)
        {
            GameObject go = Instantiate(menuButtonPrefab, carouselBoxTransform);
            RectTransform rectTransform = go.GetComponent<RectTransform>();
            Image imageRenderer = go.GetComponent<Image>();
            Button button = go.GetComponent<Button>();

            imageRenderer.sprite = buttonProperties[i].buttonSprite;
            imageRenderer.SetNativeSize();
            rectTransform.localScale = buttonProperties[i].startsAsSelected ? selectedButtonSize * Vector2.one : unselectedButtonSize * Vector2.one;
            rectTransform.anchoredPosition = menuButtonsPositions[i].anchoredPosition;

            if (buttonProperties[i].startsAsSelected)
            {
                currentSelectedOption = i;
            }

            switch (buttonProperties[i].buttonFunction)
            {
                case BUTTON_FUNCTION.QUIT:
                    button.onClick.AddListener(() => QuitGame());
                    break;

                case BUTTON_FUNCTION.VOLUME:
                    button.onClick.AddListener(() => ToggleVolume());
                    break;

                case BUTTON_FUNCTION.PLAY:
                    button.onClick.AddListener(() => StartGame());
                    break;
            }
            buttons[i] = button;
        }
    }

    public void Select(InputAction.CallbackContext context)
    {
        if (controlsScreen.activeSelf)
            return;

        buttons[currentSelectedOption].onClick.Invoke();
    }

    public void MenuScroll(InputAction.CallbackContext context)
    {
        if (controlsScreen.activeSelf)
            return;

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
        RectTransform rectTransform = buttons[index].GetComponent<RectTransform>();
        rectTransform.localScale = Vector2.one * unselectedButtonSize;
    }

    private void SelectOption(int index)
    {
        RectTransform rectTransform = buttons[index].GetComponent<RectTransform>();
        rectTransform.localScale = Vector2.one * selectedButtonSize;
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
        if (startCoroutine != null)
            return;

        startCoroutine = StartCoroutine(GameStartCoroutine());
    }

    private IEnumerator GameStartCoroutine()
    {
        controlsScreen.SetActive(true);
        yield return new WaitForSeconds(timeToWaitOnControlsScreen);
        // TODO : Passer à la premiere scene
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        Debug.Log("Start Game");
    }

    public void ToggleVolume()
    {
        Debug.Log("Activation / désactivation du son");
        Image image = buttons[1].GetComponent<Image>();
        image.sprite = (image.sprite == buttonProperties[1].buttonSprite ? buttonProperties[1].altSprite : buttonProperties[1].buttonSprite);
    }
}

[System.Serializable]
struct ButtonFunction
{
    public Sprite buttonSprite;
    public Sprite altSprite;
    public BUTTON_FUNCTION buttonFunction;
    public bool startsAsSelected;
}

enum BUTTON_FUNCTION
{
    QUIT,
    VOLUME,
    PLAY
}
