using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class MainMenu : MonoBehaviour
{
    [Header("Gamefeel values")]
    [Tooltip("Temps à attendre pour considérer que le joueur a arrêté de tourner la roue.")]
    [SerializeField] private float stopScrollWaitingTime = 0.75f;
    [SerializeField] private float animationTime = 0.5f;
    [SerializeField] private float scrollScale = 0.01f;
    [SerializeField] private AnimationCurve translationCurve, opacityCurve;

    [Header("Objects and animations")]
    [SerializeField] private float selectedTextSize;
    [SerializeField] private float unselectedTextSize;
    [SerializeField] private string[] selectableTexts;
    [SerializeField] private GameObject textOptionPrefab;
    [SerializeField] private RectTransform wheelImage, carouselBox;
    [SerializeField] private RectTransform[] textPositions;

    private float scrollInput = 0f;
    private float previousScrollInput;
    private bool canScroll = true;
    private int selectedOption = 1; // 0 = Options, 1 = Play, 2 = Controls;
    private List<GameObject> onScreenOptions;

    private void Start()
    {
        Initialize();
    }

    public void OnScroll(InputAction.CallbackContext context)
    {
        Scroll(context.ReadValue<Vector2>().y * scrollScale);
    }
    private void Initialize()
    {
        onScreenOptions = new List<GameObject>();
        for (int i = 0; i < 5; i++)
        {
            GameObject textOption = Instantiate(textOptionPrefab, carouselBox);

            if (i != 2)
                textOption.GetComponent<TextMeshProUGUI>().fontSize = unselectedTextSize;
            else
                textOption.GetComponent<TextMeshProUGUI>().fontSize = selectedTextSize;

            if (i == 0 || i == 4)
            {
                textOption.SetActive(false);
                textOption.transform.SetParent(textPositions[Mathf.Clamp(i, 0, 2)]);
                textOption.transform.localRotation = Quaternion.Euler(0, 0, 0);
                textOption.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                textOption.GetComponent<TextMeshProUGUI>().text = selectableTexts[i == 0 ? 2 : 0];
                textOption.GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 0);
            }

            onScreenOptions.Add(textOption);
        }

        for (int i = 1; i < 4; i++)
        {
            RectTransform rectTransform = onScreenOptions[i].GetComponent<RectTransform>();
            onScreenOptions[i].GetComponent<TextMeshProUGUI>().text = selectableTexts[i - 1];
            rectTransform.SetParent(textPositions[i - 1]);
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        scrollInput = 0;
        canScroll = true;
        selectedOption = 1; // Play
    }
    private void Scroll(float delta)
    {
        if (!canScroll)
            return;

        scrollInput += delta;
        if(scrollInput < 0)
        {
            scrollInput = 1;
            ChangeOptionValues(-1);
        }
        else if(scrollInput > 1)
        {
            scrollInput = 0;
            ChangeOptionValues(1);
        }

        ScrollMenu();
        ChangeWheelRotation(delta * 0.1f);
    }
    private void ChangeWheelRotation(float delta)
    {
        wheelImage.rotation *= Quaternion.Euler(0, 0, delta * 1000);
    }

    private void ScrollMenu()
    {
        for(int i = 0; i < 5; i++)
        {
            RectTransform rectTransform = onScreenOptions[i].GetComponent<RectTransform>();
            TextMeshProUGUI tmpro = onScreenOptions[i].GetComponent<TextMeshProUGUI>();

            rectTransform.SetParent(carouselBox);

            switch (i)
            {
                case 0:
                    tmpro.color = new Color(1, 1, 1, 1 - scrollInput);
                    break;

                case 1:
                    rectTransform.anchoredPosition = Vector2.Lerp(textPositions[1].anchoredPosition, textPositions[0].anchoredPosition, scrollInput);
                    rectTransform.localRotation = Quaternion.Lerp(textPositions[1].localRotation, textPositions[0].localRotation, scrollInput);
                    tmpro.fontSize = Mathf.Lerp(unselectedTextSize, selectedTextSize, 1 - scrollInput);
                    break;

                case 2:
                    if(previousScrollInput - scrollInput < 0)
                    {
                        rectTransform.anchoredPosition = Vector2.Lerp(textPositions[1].anchoredPosition, textPositions[0].anchoredPosition, scrollInput);
                        rectTransform.localRotation = Quaternion.Lerp(textPositions[1].localRotation, textPositions[0].localRotation, scrollInput);
                        tmpro.fontSize = Mathf.Lerp(selectedTextSize, unselectedTextSize, scrollInput);
                    }
                    else if (previousScrollInput - scrollInput > 0)
                    {
                        rectTransform.anchoredPosition = Vector2.Lerp(textPositions[2].anchoredPosition, textPositions[1].anchoredPosition, scrollInput);
                        rectTransform.localRotation = Quaternion.Lerp(textPositions[2].localRotation, textPositions[1].localRotation, scrollInput);
                        tmpro.fontSize = Mathf.Lerp(selectedTextSize, unselectedTextSize, 1 - scrollInput);
                    }
                    break;

                case 3:
                    rectTransform.anchoredPosition = Vector2.Lerp(textPositions[2].anchoredPosition, textPositions[1].anchoredPosition, scrollInput);
                    rectTransform.localRotation = Quaternion.Lerp(textPositions[2].localRotation, textPositions[1].localRotation, scrollInput);
                    tmpro.fontSize = Mathf.Lerp(unselectedTextSize, selectedTextSize, scrollInput);
                    break;

                case 4:
                    tmpro.color = new Color(1, 1, 1, scrollInput);
                    break;
            }
        }
        
        previousScrollInput = scrollInput;
    }

    private void ChangeOptionValues(int delta)
    {
        if (delta > 0)
        {
            onScreenOptions.Add(onScreenOptions[0]);
            onScreenOptions.Remove(onScreenOptions[0]);
        }
        else
        {
            onScreenOptions.Insert(0, onScreenOptions[4]);
            onScreenOptions.Remove(onScreenOptions[5]);
        }


    }
}
