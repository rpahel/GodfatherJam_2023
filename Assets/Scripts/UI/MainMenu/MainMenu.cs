using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("Gamefeel values")]
    [Tooltip("Temps à attendre pour considérer que le joueur a arrêté de tourner la roue.")]
    [SerializeField] private float stopScrollWaitingTime = 0.75f;
    [SerializeField] private float animationTime = 0.5f;
    [SerializeField] private AnimationCurve translationCurve, opacityCurve;

    [Header("Objects and animations")]
    [SerializeField] private float selectedTextSize;
    [SerializeField] private float unselectedTextSize;
    [SerializeField] private string[] selectableTexts;
    [SerializeField] private GameObject textOptionPrefab;
    [SerializeField] private RectTransform wheelImage, carouselBox;
    [SerializeField] private RectTransform[] textPositions;

    private float scrollInput = 0f;
    private bool canScroll = true;
    private int selectedOption = 1; // 0 = Quit, 1 = Play, 2 = Controls;
    private List<GameObject> onScreenOptions;

    private void Start()
    {
        onScreenOptions = new List<GameObject>();
        for(int i = 0; i < 5; i++)
        {
            GameObject textOption = Instantiate(textOptionPrefab, carouselBox);

            if(i != 2)
                textOption.GetComponent<TextMeshProUGUI>().fontSize = unselectedTextSize;
            else
                textOption.GetComponent<TextMeshProUGUI>().fontSize = selectedTextSize;

            if (i == 0 || i == 4)
            {
                textOption.SetActive(false);
                textOption.transform.SetParent(textPositions[Mathf.Clamp(i, 0, 2)]);
                //textOption.transform.anchoredPosition = Vector2.zero;
                textOption.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }

           onScreenOptions.Add(textOption);
        }

        for(int i = 1; i < 4; i++)
        {
            RectTransform rectTransform = onScreenOptions[i].GetComponent<RectTransform>();
            onScreenOptions[i].GetComponent<TextMeshProUGUI>().text = selectableTexts[i - 1];
            rectTransform.SetParent(textPositions[i - 1]);
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        
    }
}
