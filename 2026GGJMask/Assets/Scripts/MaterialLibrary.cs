using UnityEngine;

/// <summary>
/// 素材库管理器（用于显示素材的视觉表现）
/// </summary>
public class MaterialLibrary : MonoBehaviour
{
    [Header("素材预制体")]
    public GameObject materialPrefab;

    [Header("颜色映射")]
    public Color[] colorMap = new Color[7]; // 对应7种颜色

    private void Start()
    {
        InitializeColorMap();
    }

    /// <summary>
    /// 初始化颜色映射
    /// </summary>
    private void InitializeColorMap()
    {
        if (colorMap.Length < 7)
        {
            colorMap = new Color[7];
        }

        // 红橙黄绿青蓝紫
        colorMap[0] = Color.red;
        colorMap[1] = new Color(1f, 0.5f, 0f); // 橙
        colorMap[2] = Color.yellow;
        colorMap[3] = Color.green;
        colorMap[4] = Color.cyan; // 青
        colorMap[5] = Color.blue;
        colorMap[6] = new Color(0.5f, 0f, 0.5f); // 紫
    }

    /// <summary>
    /// 创建素材视觉对象
    /// </summary>
    public GameObject CreateMaterialVisual(MaterialData materialData, Transform parent)
    {
        if (materialPrefab == null)
        {
            Debug.LogError("素材预制体未设置！");
            return null;
        }

        GameObject materialObj = Instantiate(materialPrefab, parent);
        MaterialVisual visual = materialObj.GetComponent<MaterialVisual>();
        
        if (visual == null)
        {
            visual = materialObj.AddComponent<MaterialVisual>();
        }

        visual.SetMaterial(materialData, GetColor(materialData.color));
        return materialObj;
    }

    /// <summary>
    /// 获取颜色
    /// </summary>
    public Color GetColor(MaterialColor materialColor)
    {
        int index = (int)materialColor;
        if (index >= 0 && index < colorMap.Length)
        {
            return colorMap[index];
        }
        return Color.white;
    }

    /// <summary>
    /// 获取形状名称
    /// </summary>
    public string GetShapeName(MaterialShape shape)
    {
        switch (shape)
        {
            case MaterialShape.Triangle:
                return "三角形";
            case MaterialShape.Ellipse:
                return "椭圆形";
            case MaterialShape.Rectangle:
                return "长方形";
            default:
                return "未知";
        }
    }

    /// <summary>
    /// 获取颜色名称
    /// </summary>
    public string GetColorName(MaterialColor color)
    {
        switch (color)
        {
            case MaterialColor.Red:
                return "红";
            case MaterialColor.Orange:
                return "橙";
            case MaterialColor.Yellow:
                return "黄";
            case MaterialColor.Green:
                return "绿";
            case MaterialColor.Cyan:
                return "青";
            case MaterialColor.Blue:
                return "蓝";
            case MaterialColor.Purple:
                return "紫";
            default:
                return "未知";
        }
    }
}

