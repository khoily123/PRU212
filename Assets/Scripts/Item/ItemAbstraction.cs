using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemAbstraction : MonoBehaviour
{
    protected float durability = 2.0f; // default durability(độ bền-thời gian cần để phá hủy)
    protected int gold = 2; // default gold(vàng nhận được khi phá hủy)
    // Start is called before the first frame update

    public Image timerImage; // Hình ảnh đồng hồ
    private float timeLeft; // Thời gian còn lại
    void Start()
    {
        timerImage.gameObject.SetActive(false); // Khởi tạo đồng hồ ẩn
        UpdateTimerImage();
    }
    void UpdateTimerImage()
    {
        if (timerImage != null)
        {
            // Chỉ định vị trí mới cho timerImage dựa trên vị trí thế giới của đối tượng + offset
            Vector3 worldPos = transform.position + new Vector3(0, 1.5f, 0); // Giả sử offset 1.5 units lên trên
            timerImage.transform.position = worldPos;
        }
    }

    public void StartDestructionTimer()
    {
        if(GoldManage.Instance.CanAfford(gold))
        {
            GoldManage.Instance.SpendGold(gold);
            timeLeft = durability;
            timerImage.gameObject.SetActive(true);
            timerImage.fillAmount = 1.0f;
            InvokeRepeating("UpdateTimer", 0, Time.deltaTime);
        }
        else
        {
            Debug.Log("Not enough gold to destroy item");
        }
    }

    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerImage.fillAmount = timeLeft / durability;
            UpdateTimerImage();
        }
    }
    void DestroyItem()
    {
        timerImage.gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void UpdateTimer()
    {
        if (timeLeft <= 0)
        {
            //CancelInvoke("UpdateTimer");
            DestroyItem();
        }
        else
        {
            Invoke("UpdateTimer", Time.deltaTime); // Tiếp tục cập nhật
        }
    }

}
