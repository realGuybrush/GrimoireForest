using System;

[Serializable]
public class Biome
{
    private BiomeData _type;
    private SVector3 Center = new SVector3();
    private int radius;

    public Action OnGrow = delegate { };
    public Biome(BiomeData type)
    {
        _type = type;
    }

    public void TryGrow() {
        bool grow;
        //todo: make growing logic here
        grow = false;
        if(grow)
            OnGrow?.Invoke();
    }
}
