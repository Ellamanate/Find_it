using System.Collections.Generic;
using UnityEngine;


public class UI : Singleton<UI>
{
    [SerializeField] private ItemUI itemUI;
    [SerializeField] private RectTransform itemsPanel;
    private Dictionary<string, ItemUI> panelPool = new Dictionary<string, ItemUI>();

    public void AddItemInPanel(Item item)
    {
        if (item.MyLevel == GameManager.Instance.CurrentLevel)
        {
            if (panelPool.ContainsKey(item.ObjectName))
            {
                panelPool[item.ObjectName].ItemsNumber += 1;
            }
            else
            {
                ItemUI panelElement = Instantiate(itemUI, itemsPanel);
                panelElement.Init(1, item.Sprite, item);
                panelPool.Add(item.ObjectName, panelElement);
            }
        }
    }

    public void UpdateItemsPanel(Level level)
    {
        foreach (Item item in level.FindObjects)
        {
            if (!panelPool.ContainsKey(item.ObjectName))
            {
                ItemUI panelElement = Instantiate(itemUI, itemsPanel);
                panelElement.Init(level.TryGetItemsNumber(item), item.Sprite, item);
                panelPool.Add(item.ObjectName, panelElement);
            }
        }
    }

    private void Awake()
    {
        UpdateItemsPanel(GameManager.Instance.CurrentLevel);
    }

    private void OnEnable()
    {
        Events.OnItemFinded.Subscribe(OnItemFinded);
        Events.OnLevelChanged.Subscribe(OnLevelChanged);
    }

    private void OnDisable()
    {
        Events.OnItemFinded.UnSubscribe(OnItemFinded);
        Events.OnLevelChanged.UnSubscribe(OnLevelChanged);
    }

    private void OnLevelChanged(Level newLevel)
    {
        foreach (ItemUI item in panelPool.Values)
            Destroy(item.gameObject);

        panelPool.Clear();

        UpdateItemsPanel(newLevel);
    }

    private void OnItemFinded(Item item)
    {
        if (item.MyLevel != null)
        {
            foreach (ItemUI itemUI in panelPool.Values)
            {
                if (itemUI.Item.ObjectName == item.ObjectName)
                {
                    itemUI.ItemsNumber = item.MyLevel.TryGetItemsNumber(item);
                    break;
                }
            }
        }
    }
}