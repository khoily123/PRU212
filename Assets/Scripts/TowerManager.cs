﻿using UnityEngine;
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

    void Start()
    {
        towerSelectionPopup.SetActive(false); // Ẩn popup khi game bắt đầu
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
                GameObject towerInstance = Instantiate(selectedTower, snapPosition, Quaternion.identity);

                // 🔥 Tìm Shooter trong các con của towerInstance
                ShooterAbstract shooter = towerInstance.GetComponentInChildren<ShooterAbstract>();

                if (shooter != null)
                {
                    shooter.SetPlaced(true); // ✅ Chỉ kích hoạt phần bắn
                }

                Destroy(towerPreview);
                towerPreview = null;
                selectedTower = null;
                highlightTilemap.ClearAllTiles();
            }

            if (Input.GetMouseButtonDown(1)) // Nhấn chuột phải để hủy chọn
            {
                CancelTowerPlacement();
            }
        }
    }

    public void ShowTowerSelection()
    {
        towerSelectionPopup.SetActive(true); // Hiển thị popup
    }

    public void SelectTower(int index)
    {
        selectedTower = towerPrefabs[index];
        towerSelectionPopup.SetActive(false); // Ẩn popup sau khi chọn
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
