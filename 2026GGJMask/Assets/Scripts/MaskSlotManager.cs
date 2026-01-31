using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 面具槽管理器
/// </summary>
public class MaskSlotManager : MonoBehaviour
{
    [Header("面具槽引用")]
    public MaskSlot[] maskSlots;

    private Dictionary<MaskPosition, MaskSlot> slotDictionary;
    private Dictionary<MaskPosition, MaterialData> placedMaterials;

    /// <summary>
    /// 初始化面具槽
    /// </summary>
    public void Initialize()
    {
        slotDictionary = new Dictionary<MaskPosition, MaskSlot>();
        placedMaterials = new Dictionary<MaskPosition, MaterialData>();

        foreach (MaskSlot slot in maskSlots)
        {
            if (slot != null)
            {
                slotDictionary[slot.position] = slot;
                slot.Initialize();
            }
        }
    }

    /// <summary>
    /// 放置素材到指定位置
    /// </summary>
    public bool PlaceMaterial(MaskPosition position, MaterialData material)
    {
        if (!slotDictionary.ContainsKey(position))
        {
            Debug.LogError($"未找到位置：{position}");
            return false;
        }

        MaskSlot slot = slotDictionary[position];
        
        // 检查位置是否已被占用
        if (placedMaterials.ContainsKey(position))
        {
            Debug.LogWarning($"位置 {position} 已被占用！");
            return false;
        }

        // 放置素材
        if (slot.PlaceMaterial(material))
        {
            placedMaterials[position] = material;
            return true;
        }

        return false;
    }

    /// <summary>
    /// 移除指定位置的素材
    /// </summary>
    public bool RemoveMaterial(MaskPosition position)
    {
        if (!placedMaterials.ContainsKey(position))
        {
            return false;
        }

        MaskSlot slot = slotDictionary[position];
        if (slot.RemoveMaterial())
        {
            placedMaterials.Remove(position);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 检查面具是否填充完成
    /// </summary>
    public bool IsMaskComplete()
    {
        // 检查所有9个位置是否都已填充
        return placedMaterials.Count == 9;
    }

    /// <summary>
    /// 重置面具槽
    /// </summary>
    public void Reset()
    {
        foreach (var slot in slotDictionary.Values)
        {
            slot.RemoveMaterial();
        }
        placedMaterials.Clear();
    }

    /// <summary>
    /// 获取已放置的素材
    /// </summary>
    public Dictionary<MaskPosition, MaterialData> GetPlacedMaterials()
    {
        return new Dictionary<MaskPosition, MaterialData>(placedMaterials);
    }
}

