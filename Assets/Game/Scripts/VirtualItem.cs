using UnityEngine;


public class VirtualItem : MonoBehaviour
{
    public Item Item { get => item; }
    public SpriteRenderer SpriteRenderer { get => spriteRenderer; }

    [SerializeField] private Item item;
    [SerializeField] private SpriteRenderer spriteRenderer;
}