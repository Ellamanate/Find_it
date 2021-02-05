using UnityEngine;


public class Item : MonoBehaviour
{
    public string ObjectName { get => objectName; }
    public Sprite Sprite { get => sprite; }
    public Level MyLevel { get => myLevel; }
    public SpriteRenderer SpriteRenderer { get => spriteRenderer; }
    public bool Finded { get => finded; set => finded = OnFind(value);  }

    [SerializeField] private string objectName;
    [SerializeField] private Sprite sprite;
    [SerializeField] private Level myLevel;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private bool finded;

    private void OnEnable()
    {
        myLevel = WhereAmI();

        if (myLevel != null)
            myLevel.RegisterInPool(this);
    }

    private void OnDisable()
    {
        if (myLevel != null)
            myLevel.RemoveFromPool(this);
    }

    private void OnMouseDown()
    {
        if (MoveCamera.Instance.ClickAbility)
        {
            Finded = true;
            Events.OnItemFinded.Publish(this);
        }
    }

    private Level WhereAmI()
    {
        RaycastHit2D[] allHits = Physics2D.RaycastAll(transform.position, Vector2.zero);

        if (allHits.Length != 0)
        {
            foreach (RaycastHit2D hit in allHits)
            {
                foreach (Level level in GameManager.Instance.Levels)
                {
                    if (hit.collider.gameObject == level.LevelArea)
                        return level;
                }
            }
        }

        return null;
    }

    private bool OnFind(bool find)
    {
        gameObject.SetActive(!find);
        return find;
    }
}
