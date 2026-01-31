using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 单个面具槽
/// </summary>
public class MaskSlot : MonoBehaviour, IDropHandler
{
    [Header("槽位设置")]
    public MaskPosition position;
    
    [Header("UI引用")]
    public Image slotImage;
    public Image materialImage;
    public Text labelText;

    private MaterialData currentMaterial;
    private MaterialLibrary materialLibrary;

    private void Awake()
    {
        if (slotImage == null)
            slotImage = GetComponent<Image>();
        
        if (materialImage == null)
        {
            GameObject imgObj = new GameObject("MaterialImage");
            imgObj.transform.SetParent(transform);
            imgObj.transform.localPosition = Vector3.zero;
            imgObj.transform.localScale = Vector3.one;
            materialImage = imgObj.AddComponent<Image>();
            RectTransform rect = imgObj.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;
        }

        if (labelText == null)
        {
            GameObject textObj = new GameObject("LabelText");
            textObj.transform.SetParent(transform);
            labelText = textObj.AddComponent<Text>();
            labelText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            labelText.alignment = TextAnchor.MiddleCenter;
            RectTransform rect = textObj.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;
        }
    }

    private void Start()
    {
        materialLibrary = FindObjectOfType<MaterialLibrary>();
        UpdateLabel();
    }

    /// <summary>
    /// 初始化槽位
    /// </summary>
    public void Initialize()
    {
        currentMaterial = null;
        UpdateVisual();
        UpdateLabel();
    }

    /// <summary>
    /// 放置素材
    /// </summary>
    public bool PlaceMaterial(MaterialData material)
    {
        if (currentMaterial != null)
        {
            return false; // 位置已被占用
        }

        currentMaterial = material;
        UpdateVisual();
        return true;
    }

    /// <summary>
    /// 移除素材
    /// </summary>
    public bool RemoveMaterial()
    {
        if (currentMaterial == null)
        {
            return false;
        }

        currentMaterial = null;
        UpdateVisual();
        return true;
    }

    /// <summary>
    /// 更新视觉表现
    /// </summary>
    private void UpdateVisual()
    {
        if (materialImage != null)
        {
            materialImage.enabled = currentMaterial != null;
            
            if (currentMaterial != null && materialLibrary != null)
            {
                Color color = materialLibrary.GetColor(currentMaterial.color);
                materialImage.color = color;
                // 这里可以设置形状sprite
            }
        }
    }

    /// <summary>
    /// 更新标签
    /// </summary>
    private void UpdateLabel()
    {
        if (labelText != null)
        {
            string positionName = GetPositionName(position);
            labelText.text = positionName;
            labelText.color = currentMaterial == null ? Color.white : Color.gray;
        }
    }

    /// <summary>
    /// 获取位置名称
    /// </summary>
    private string GetPositionName(MaskPosition pos)
    {
        switch (pos)
        {
            case MaskPosition.LeftEye: return "左眼";
            case MaskPosition.RightEye: return "右眼";
            case MaskPosition.LeftEar: return "左耳";
            case MaskPosition.RightEar: return "右耳";
            case MaskPosition.Nose: return "鼻子";
            case MaskPosition.Mouth: return "嘴巴";
            case MaskPosition.LeftFace: return "左面纹";
            case MaskPosition.RightFace: return "右面纹";
            case MaskPosition.Forehead: return "额饰";
            default: return "未知";
        }
    }

    /// <summary>
    /// 拖拽放置处理
    /// </summary>
    public void OnDrop(PointerEventData eventData)
    {
        DraggableMaterial draggable = eventData.pointerDrag?.GetComponent<DraggableMaterial>();
        if (draggable != null)
        {
            MaterialData material = draggable.GetMaterialData();
            if (material != null && GameManager.Instance != null)
            {
                GameManager.Instance.PlaceMaterial(position);
            }
        }
    }

    /// <summary>
    /// 获取当前位置
    /// </summary>
    public MaskPosition GetPosition()
    {
        return position;
    }

    /// <summary>
    /// 检查是否为空
    /// </summary>
    public bool IsEmpty()
    {
        return currentMaterial == null;
    }
}

