using System;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TowerManager : MonoBehaviour
{
    public GameObject[] towerPrefabs; // Mảng chứa prefab Tower
    public Tilemap gridTilemap; // Tilemap của map
    public Tilemap highlightTilemap; // Tilemap hiển thị vùng đặt (màu xanh/đỏ)
    public Tile highlightGreen; // Tile highlight xanh (được đặt)
    public Tile highlightRed; // Tile highlight đỏ (không được đặt)
    public GameObject towerSelectionPopup; // Popup UI chọn Tower
    public Tilemap roadTilemap;
    private GameObject selectedTower; // Tower đang chọn
    private GameObject towerPreview; // Hiển thị trước khi đặt
    public int[] towerCosts; //giá của các tháp
    public bool isPopupActive = false;

    void Start()
    {
        towerSelectionPopup.SetActive(isPopupActive); // Ẩn popup khi game bắt đầu
    }

    void Update()
    {
        if (selectedTower != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            // Lấy vị trí ô tilemap
            Vector3Int cellPosition = gridTilemap.WorldToCell(mousePos);
            Vector3 snapPosition = gridTilemap.GetCellCenterWorld(cellPosition);

            // Xóa highlight cũ
            highlightTilemap.ClearAllTiles();

            if (CanPlaceTower(cellPosition))
            {
                highlightTilemap.SetTile(cellPosition, highlightGreen); // Hiển thị ô xanh
            }
            else
            {
                highlightTilemap.SetTile(cellPosition, highlightRed); // Hiển thị ô đỏ
            }

            if (towerPreview == null)
            {
                towerPreview = Instantiate(selectedTower, snapPosition, Quaternion.identity);
                ShooterAbstract previewShooter = towerPreview.GetComponentInChildren<ShooterAbstract>();
                if (previewShooter != null)
                {
                    previewShooter.SetPlaced(false);
                }
                SetTowerAlpha(towerPreview, 0.5f); // Đặt alpha thấp để làm preview
            }
            else
            {
                towerPreview.transform.position = snapPosition;
            }

            if (Input.GetMouseButtonDown(0) && CanPlaceTower(cellPosition))
            {

                if (GoldManage.Instance.CanAfford(towerCosts[Array.IndexOf(towerPrefabs, selectedTower)]))
                {
                    GoldManage.Instance.SpendGold(towerCosts[Array.IndexOf(towerPrefabs, selectedTower)]);
                    GameObject towerInstance = Instantiate(selectedTower, snapPosition, Quaternion.identity);
                    ShooterAbstract shooter = towerInstance.GetComponentInChildren<ShooterAbstract>();
                    // 🔥 Tìm Shooter trong các con của towerInstance

                    if (shooter != null)
                    {
                        shooter.SetPlaced(true); // ✅ Chỉ kích hoạt phần bắn
                    }

                    Destroy(towerPreview);
                    towerPreview = null;
                    selectedTower = null;
                    highlightTilemap.ClearAllTiles();
                }
                else
                {
                    Debug.Log("Not enough gold!");
                    return;
                }

                
            }

            if (Input.GetMouseButtonDown(1)) // Nhấn chuột phải để hủy chọn
            {
                CancelTowerPlacement();
            }
        }
    }

    public void ShowTowerSelection()
    {
        if(isPopupActive)
        {
            isPopupActive = false;
            towerSelectionPopup.SetActive(isPopupActive);
        }
        else
        {
            isPopupActive = true;
            towerSelectionPopup.SetActive(isPopupActive);
        }
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
            return;
        }
        
    }

    private bool CanPlaceTower(Vector3Int cellPosition)
    {
        TileBase roadTile = roadTilemap.GetTile(cellPosition);  // Kiểm tra xem có tile đường ở vị trí này không
        TileBase backgroundTile = gridTilemap.GetTile(cellPosition); // Kiểm tra xem có tile nền ở vị trí này không

        return backgroundTile != null && roadTile == null; // Chỉ cho phép đặt tháp nếu có tile nền và không có tile đường
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
