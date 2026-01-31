using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 素材的视觉表现组件
/// </summary>
public class MaterialVisual : MonoBehaviour
{
    private MaterialData materialData;
    private Image image;
    private RectTransform rectTransform;

    private void Awake()
    {
        image = GetComponent<Image>();
        if (image == null)
        {
            image = gameObject.AddComponent<Image>();
        }
        rectTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// 设置素材数据和外观
    /// </summary>
    public void SetMaterial(MaterialData data, Color color)
    {
        materialData = data;
        
        if (image != null)
        {
            image.color = color;
            
            // 根据形状设置sprite或形状
            SetShape(data.shape);
        }
    }

    /// <summary>
    /// 设置形状
    /// </summary>
    private void SetShape(MaterialShape shape)
    {
        // 这里可以使用不同的sprite或通过代码绘制形状
        // 简化版本：使用不同的sprite或通过Image的sprite属性
        // 实际项目中可以创建对应的sprite资源
        
        // 临时方案：通过改变RectTransform的scale来模拟不同形状
        switch (shape)
        {
            case MaterialShape.Triangle:
                // 三角形：可以旋转矩形或使用三角形sprite
                transform.rotation = Quaternion.identity;
                rectTransform.localScale = Vector3.one;
                break;
            case MaterialShape.Ellipse:
                // 椭圆形：保持圆形或椭圆形
                rectTransform.localScale = new Vector3(1.2f, 0.8f, 1f);
                break;
            case MaterialShape.Rectangle:
                // 长方形：保持矩形
                rectTransform.localScale = new Vector3(1.5f, 1f, 1f);
                break;
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

