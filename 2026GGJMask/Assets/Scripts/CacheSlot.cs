using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 单个缓存槽
/// </summary>
public class CacheSlot : MonoBehaviour, IPointerClickHandler
{
    private MaterialData materialData;
    private Image slotImage;
    private Image materialImage;
    private MaterialLibrary materialLibrary;

    private void Awake()
    {
        slotImage = GetComponent<Image>();
        if (slotImage == null)
        {
            slotImage = gameObject.AddComponent<Image>();
        }

        // 创建素材显示图像
        GameObject imgObj = new GameObject("MaterialImage");
        imgObj.transform.SetParent(transform);
        imgObj.transform.localPosition = Vector3.zero;
        imgObj.transform.localScale = Vector3.one;
        materialImage = imgObj.AddComponent<Image>();
        RectTransform rect = imgObj.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
        materialImage.enabled = false;
    }

    private void Start()
    {
        materialLibrary = FindObjectOfType<MaterialLibrary>();
    }

    /// <summary>
    /// 初始化缓存槽
    /// </summary>
    public void Initialize(int index)
    {
        materialData = null;
        UpdateVisual();
    }

    /// <summary>
    /// 设置素材
    /// </summary>
    public void SetMaterial(MaterialData material)
    {
        materialData = material;
        UpdateVisual();
    }

    /// <summary>
    /// 清除素材
    /// </summary>
    public void ClearMaterial()
    {
        materialData = null;
        UpdateVisual();
    }

    /// <summary>
    /// 更新视觉表现
    /// </summary>
    private void UpdateVisual()
    {
        if (materialImage != null)
        {
            materialImage.enabled = materialData != null;
            
            if (materialData != null && materialLibrary != null)
            {
                Color color = materialLibrary.GetColor(materialData.color);
                materialImage.color = color;
            }
        }
    }

    /// <summary>
    /// 点击处理（取回素材）
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (materialData != null && GameManager.Instance != null)
        {
            GameManager.Instance.RetrieveFromCache(materialData);
        }
    }

    /// <summary>
    /// 获取素材数据
    /// </summary>
    public MaterialData GetMaterialData()
    {
        return materialData;
    }
}

