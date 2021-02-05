using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Level
{
    public GameObject LevelArea { get => levelArea; }
    public IReadOnlyCollection<Item> FindObjects { get => findObjects.AsReadOnly(); }
    public IReadOnlyCollection<Item> Pool { get => pool.AsReadOnly(); }

    [SerializeField] private GameObject levelArea;
    [SerializeField] private List<Item> findObjects = new List<Item>();
    [SerializeField] private List<Item> pool = new List<Item>();
    private Dictionary<string, int> objectsCollection = new Dictionary<string, int>();

    public void RegisterInPool(Item item)
    {
        pool.Add(item);

        if (objectsCollection.ContainsKey(item.ObjectName))
            objectsCollection[item.ObjectName] += 1;
        else
            objectsCollection.Add(item.ObjectName, 1);
        
        UI.Instance.AddItemInPanel(item);
    }

    public void RemoveFromPool(Item item)
    {
        pool.Remove(item);
        objectsCollection[item.ObjectName] -= 1;
    }

    public int TryGetItemsNumber(Item item)
    {
        if (!objectsCollection.ContainsKey(item.ObjectName))
            objectsCollection.Add(item.ObjectName, 0);

        return objectsCollection[item.ObjectName];

    }
}