using System;
using UnityEngine;

[Serializable]
public enum MaterialShape
{
    Triangle,    // 三角形
    Ellipse,     // 椭圆形
    Rectangle    // 长方形
}

[Serializable]
public enum MaterialColor
{
    Red,         // 红
    Orange,      // 橙
    Yellow,      // 黄
    Green,       // 绿
    Cyan,        // 青
    Blue,        // 蓝
    Purple       // 紫
}

[Serializable]
public enum MaskPosition
{
    LeftEye,     // 左眼
    RightEye,    // 右眼
    LeftEar,     // 左耳
    RightEar,    // 右耳
    Nose,        // 鼻子
    Mouth,       // 嘴巴
    LeftFace,    // 左面纹
    RightFace,   // 右面纹
    Forehead     // 额饰
}

[Serializable]
public class MaterialData
{
    public MaterialShape shape;
    public MaterialColor color;
    public int id; // 用于区分相同形状和颜色的两张素材

    public MaterialData(MaterialShape shape, MaterialColor color, int id)
    {
        this.shape = shape;
        this.color = color;
        this.id = id;
    }

    public override bool Equals(object obj)
    {
        if (obj is MaterialData other)
        {
            return shape == other.shape && color == other.color && id == other.id;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return shape.GetHashCode() ^ color.GetHashCode() ^ id.GetHashCode();
    }
}

