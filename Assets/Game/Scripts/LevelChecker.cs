using System.Collections;
using UnityEngine;


public class LevelChecker : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (Level level in GameManager.Instance.Levels)
        {
            if (collision.gameObject == level.LevelArea && level != GameManager.Instance.CurrentLevel)
            {
                Events.OnLevelChanged.Publish(level);
                break;
            }
        }
    }
}