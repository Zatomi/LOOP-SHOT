using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpecialBulletUI : MonoBehaviour
{
    [Header("’e‚ÌƒAƒCƒRƒ“")]
    public Image[] bulletIcons = new Image[5];
    public Image normalBulletIcon;

    [Header("˜g‚Ì‰æ‘œ")]
    public Image[] slotFrames = new Image[6];

    [Header("“Áê’e‚Ì‰æ‘œ")]
    public Sprite explosiveIcon;
    public Sprite piercingIcon;
    public Sprite splitIcon;
    public Sprite lightningIcon;

    [Header("‘I‘ğ’†‚Ì˜g‚ÌF")]
    public Color selectedColor = Color.yellow;
    public Color normalColor = Color.white;

    public void UpdateSlots(List<SpecialBulletType> bullets)
    {
        for (int i = 0; i < bulletIcons.Length; i++)
        {
            if (bulletIcons[i] == null)
                continue;

            if (i < bullets.Count)
            {
                bulletIcons[i].sprite = GetIcon(bullets[i]);
                bulletIcons[i].enabled =
                    bulletIcons[i].sprite != null;
            }
            else
            {
                bulletIcons[i].enabled = false;
            }
        }
    }

    public void UpdateSelection(int selectedIndex)
    {
        for (int i = 0; i < slotFrames.Length; i++)
        {
            if (slotFrames[i] == null)
                continue;

            slotFrames[i].color =
                i == selectedIndex ? selectedColor : normalColor;
        }
    }

    Sprite GetIcon(SpecialBulletType type)
    {
        switch (type)
        {
            case SpecialBulletType.Explosive:
                return explosiveIcon;

            case SpecialBulletType.Piercing:
                return piercingIcon;

            case SpecialBulletType.Split:
                return splitIcon;

            case SpecialBulletType.Lightning:
                return lightningIcon;

            default:
                return null;
        }
    }
}