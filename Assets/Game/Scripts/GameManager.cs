using System.Collections.Generic;
using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    public IReadOnlyCollection<Level> Levels { get => levels.AsReadOnly(); }
    public Level CurrentLevel { get => currentLevel; }

    [SerializeField] private List<Level> levels = new List<Level>();
    [SerializeField] Transform itemsParent;
    private List<Item> findedItems = new List<Item>();
    private Level currentLevel;
    private int currentLevelIndex;

    public void OnLevelChanged(Level newLevel)
    {
        currentLevel = newLevel;
        currentLevelIndex = levels.IndexOf(currentLevel);
    }

    public void Save()
    {
        Item[] items = FindObjectsOfType<Item>();
        List<ItemState> itemStates = new List<ItemState>();

        foreach (Item item in items)
        {
            ItemState itemState = new ItemState
            {
                Name = item.ObjectName,
                Position = item.transform.position,
                Scale = item.transform.lossyScale,
                OrderInLayer = item.SpriteRenderer.sortingOrder
            };

            itemStates.Add(itemState);
        }

        GameState gameState = new GameState(currentLevelIndex, itemStates);
        SaveLoad<GameState>.Save(gameState);
    }

    private void LoadData()
    {
        GameState loads = SaveLoad<GameState>.Load();

        if (loads != null)
        {
            currentLevelIndex = loads.CurrentLevelIndex;
            currentLevel = levels[currentLevelIndex];

            foreach (ItemState itemState in loads.ItemStates)
            {
                foreach (Item item in Resources.LoadAll("Items", typeof(Item)))
                {
                    if (itemState.Name == item.ObjectName)
                    {
                        Item newItem = Instantiate(item, itemState.Position, new Quaternion(), itemsParent);
                        newItem.SpriteRenderer.sortingOrder = itemState.OrderInLayer;
                        newItem.transform.localScale = itemState.Scale;
                        break;
                    }
                }
            }

            VirtualItem[] virtualItems = FindObjectsOfType<VirtualItem>();

            foreach (VirtualItem virtualItem in virtualItems)
                Destroy(virtualItem.gameObject);

            Debug.Log("Load");
        }
        else
        {
            NewGame();
        }
    }

    private void Awake()
    {
        if (PlayerPrefs.GetInt("Load") == 1 ? true : false)
            LoadData();
        else
            NewGame();
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

    private void NewGame()
    {
        currentLevelIndex = 0;
        currentLevel = levels[currentLevelIndex];

        VirtualItem[] virtualItems = FindObjectsOfType<VirtualItem>();

        foreach (VirtualItem virtualItem in virtualItems)
        {
            Item newItem = Instantiate(virtualItem.Item, virtualItem.transform.position, new Quaternion(), itemsParent);
            newItem.transform.localScale = virtualItem.transform.lossyScale;
            newItem.SpriteRenderer.sortingOrder = virtualItem.SpriteRenderer.sortingOrder;
            Destroy(virtualItem.gameObject);
        }

        Debug.Log("NewGame");
    }

    private void OnItemFinded(Item item)
    {
        findedItems.Add(item);

        if (currentLevel.Pool.Count == 0)
            MoveCamera.Instance.OpenLevel(levels[currentLevelIndex + 1]);
    }

    private void OnApplicationQuit()
    {
        Save();
    }
}

[System.Serializable]
public class ItemState
{
    public int OrderInLayer;
    public Vector3 Position;
    public Vector3 Scale;
    public string Name;
}