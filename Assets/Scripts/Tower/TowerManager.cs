using System;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TowerManager : MonoBehaviour
{
    public GameObject[] towerPrefabs;
    public Tilemap gridTilemap;
    public Tilemap highlightTilemap;
    public Tile highlightGreen;
    public Tile highlightRed;
    public GameObject towerSelectionPopup;
    public Tilemap roadTilemap;

    private GameObject selectedTower;
    private GameObject towerPreview;
    public int[] towerCosts;
    public bool isPopupActive = false;
    public static TowerManager Instance { get; private set; } // Singleton instance
    private void Awake()
    {
        // Đảm bảo chỉ có một instance của GoldManager
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        towerSelectionPopup.SetActive(isPopupActive);
    }

    void Update()
    {
        if (selectedTower != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            Vector3Int cellPosition = gridTilemap.WorldToCell(mousePos);
            Vector3 snapPosition = gridTilemap.GetCellCenterWorld(cellPosition);

            highlightTilemap.ClearAllTiles();

            if (CanPlaceTower(cellPosition))
            {
                highlightTilemap.SetTile(cellPosition, highlightGreen);
            }
            else
            {
                highlightTilemap.SetTile(cellPosition, highlightRed);
            }

            if (towerPreview == null)
            {
                towerPreview = Instantiate(selectedTower, snapPosition, Quaternion.identity);
                ShooterAbstract previewShooter = towerPreview.GetComponentInChildren<ShooterAbstract>();
                if (previewShooter != null)
                {
                    previewShooter.SetPlaced(false);
                }
                SetTowerAlpha(towerPreview, 0.5f);
            }
            else
            {
                towerPreview.transform.position = snapPosition;
            }

            if (Input.GetMouseButtonDown(0) && CanPlaceTower(cellPosition))
            {
                int towerIndex = Array.IndexOf(towerPrefabs, selectedTower);
                if (GoldManage.Instance.CanAfford(towerCosts[towerIndex]))
                {
                    GoldManage.Instance.SpendGold(towerCosts[towerIndex]);
                    GameObject towerInstance = Instantiate(selectedTower, snapPosition, Quaternion.identity);
                    ShooterAbstract shooter = towerInstance.GetComponentInChildren<ShooterAbstract>();
                    Debug.Log(towerInstance);
                    Debug.Log(shooter);
                    // 🔥 Tìm Shooter trong các con của towerInstance

                    if (shooter != null)
                    {
                        shooter.SetPlaced(true);
                    }

                    Destroy(towerPreview);
                    towerPreview = null;
                    selectedTower = null;
                    highlightTilemap.ClearAllTiles();
                }
                else
                {
                    Debug.Log("Not enough gold!");
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                CancelTowerPlacement();
            }
        }
    }

    public void ShowTowerSelection()
    {
        isPopupActive = !isPopupActive;
        towerSelectionPopup.SetActive(isPopupActive);
    }

    public void SelectTower(int index)
    {
        if (GoldManage.Instance.CanAfford(towerCosts[index]))
        {
            selectedTower = towerPrefabs[index];
            ShowTowerSelection();
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }

    private bool CanPlaceTower(Vector3Int cellPosition)
    {
        TileBase roadTile = roadTilemap.GetTile(cellPosition);
        TileBase backgroundTile = gridTilemap.GetTile(cellPosition);

        if (backgroundTile == null || roadTile != null)
        {
            return false;
        }

        Vector2 worldPos = gridTilemap.GetCellCenterWorld(cellPosition);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(worldPos, new Vector2(0, 0), 0);

        foreach (var collider in colliders)
        {
            GameObject rootObj = collider.transform.root.gameObject; // Lấy GameObject gốc

            if (rootObj != towerPreview)
            {
                Debug.Log($"❌ Không thể đặt tại {cellPosition} - Có vật thể {rootObj.name} ở đây");
                return false;
            }
        }

        Debug.Log($"✅ Có thể đặt tại {cellPosition}");
        return true;
    }

    private void CancelTowerPlacement()
    {
        selectedTower = null;
        if (towerPreview != null) Destroy(towerPreview);
        highlightTilemap.ClearAllTiles();
    }

    private void SetTowerAlpha(GameObject tower, float alpha)
    {
        foreach (SpriteRenderer sr in tower.GetComponentsInChildren<SpriteRenderer>())
        {
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
        }
    }
}
