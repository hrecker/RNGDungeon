using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastforwardButton : MonoBehaviour
{
    private Sprite fastforwardSprite;
    public Sprite playSprite;
    private Image uiImage;

    public BattleController battleController;
    private bool isPlaySpriteActive;

    void Start()
    {
        uiImage = GetComponent<Image>();
        fastforwardSprite = uiImage.sprite;
    }

    public void Toggle()
    {
        battleController.ToggleFastforward();
        if (isPlaySpriteActive)
        {
            uiImage.sprite = fastforwardSprite;
        }
        else
        {
            uiImage.sprite = playSprite;
        }
        isPlaySpriteActive = !isPlaySpriteActive;
    }
}
