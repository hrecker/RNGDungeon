using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages sprites added to the map, including items, chests, and collectors
public class LevelSpriteController : MonoBehaviour
{
    public GameObject collectorPrefab;
    public GameObject itemPrefab;
    public GameObject lockedChestPrefab;
    public Sprite unlockedChestSprite;

    private List<GameObject> activeCollectors;
    private Dictionary<Vector2Int, GameObject> items;
    private Dictionary<Vector2Int, GameObject> chests;

    void Start()
    {
        activeCollectors = new List<GameObject>();
        items = new Dictionary<Vector2Int, GameObject>();
        chests = new Dictionary<Vector2Int, GameObject>();
        CurrentLevel.SetSpriteController(this);
    }

    public void DrawSprites(Tile[,] tiles)
    {
        // Clear out any old sprites
        foreach (GameObject collector in activeCollectors)
        {
            Destroy(collector);
        }
        foreach (GameObject item in items.Values)
        {
            Destroy(item);
        }
        foreach (GameObject chest in chests.Values)
        {
            Destroy(chest);
        }
        activeCollectors.Clear();
        items.Clear();
        chests.Clear();

        // Instantiate sprites
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                Object toInstantiate = GetTileContentsObject(tiles[x, y].tileContents);
                Transform instantiated = null;
                if (toInstantiate != null)
                {
                    instantiated = ((GameObject) Instantiate(toInstantiate,
                        new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity)).transform;
                }
                if (instantiated != null)
                {
                    TrackNewTileContents(tiles[x, y].tileContents, instantiated);
                }
            }
        }
    }

    private Object GetTileContentsObject(TileContents contents)
    {
        switch (contents)
        {
            case TileContents.ITEM:
                return itemPrefab;
            case TileContents.LOCKED_CHEST:
                return lockedChestPrefab;
            case TileContents.COLLECTOR:
                return collectorPrefab;
        }
        return null;
    }

    private void TrackNewTileContents(TileContents contents, Transform instantiated)
    {
        switch (contents)
        {
            case TileContents.ITEM:
                items.Add(new Vector2Int((int)instantiated.position.x,
                    (int)instantiated.position.y), instantiated.gameObject);
                break;
            case TileContents.LOCKED_CHEST:
                chests.Add(new Vector2Int((int)instantiated.position.x,
                    (int)instantiated.position.y), instantiated.gameObject);
                break;
            case TileContents.COLLECTOR:
                activeCollectors.Add(instantiated.gameObject);
                break;
        }
    }

    public void RemoveItem(Vector2Int pos)
    {
        if (items.ContainsKey(pos))
        {
            Destroy(items[pos]);
            items.Remove(pos);
        }
    }

    public void UnlockChest(Vector2Int pos)
    {
        if (chests.ContainsKey(pos))
        {
            chests[pos].GetComponent<SpriteRenderer>().sprite = unlockedChestSprite;
        }
    }

    public void RemoveCollector(Vector2Int pos)
    {
        GameObject toRemove = null;
        foreach (GameObject collector in activeCollectors)
        {
            Vector2Int collectorPos = new Vector2Int((int)collector.transform.position.x, 
                (int)collector.transform.position.y);
            if (collectorPos == pos)
            {
                toRemove = collector;
            }
        }
        if (toRemove != null)
        {
            Destroy(toRemove);
            activeCollectors.Remove(toRemove);
        }
    }
}
