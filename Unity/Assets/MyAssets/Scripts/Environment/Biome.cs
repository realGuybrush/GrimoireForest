using System;
using UnityEngine;

[Serializable]
public class Biome
{
    private BiomeType type;
    private SVector3 center;
    private int radius;
    public Biome(BiomeType Type, Vector3 Center, int Radius)
    {
        type = Type;
        center = new SVector3(Center);
        radius = Radius;
    }

    public int X => (int)center.x;
    public int Y => (int)center.y;
    public Vector3 Center => center.ToV3();
    public int Radius => radius;
}
