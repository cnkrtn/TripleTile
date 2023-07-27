using UnityEngine;

[System.Serializable]
public class Stone
{
    public int ID { get; private set; }
    public Color Color { get; set; }

    public Stone(int id, Color color)
    {
        ID = id;
        Color = color;
    }
}
