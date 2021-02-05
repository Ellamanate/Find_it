using System;
using UnityEngine;
using UnityEngine.UI;
using Unity.VectorGraphics;


public class ItemUI : MonoBehaviour
{
    public int ItemsNumber { get => itemsNumber; set => itemsNumber = ChangeItemsNumber(value); }
    public Text TextField { get => textField; }
    public SVGImage Image { get => image; }
    public Item Item { get => item; }

    [SerializeField] private int itemsNumber;
    [SerializeField] private Text textField;
    [SerializeField] private SVGImage image;
    [SerializeField] private Item item;

    public void Init(int number, Sprite initSprite, Item initItem)
    {
        ItemsNumber = number;
        image.sprite = initSprite;
        item = initItem;
    }

    private int ChangeItemsNumber(int number)
    {
        if (number >= 0)
            textField.text = "x" + number.ToString();
        else
            throw new Exception("ItemsNumber less than zero");

        return number;
    }
}