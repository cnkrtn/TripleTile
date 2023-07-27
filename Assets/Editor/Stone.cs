using UnityEngine;

[System.Serializable]
public class Stone
{
    public int ID { get; private set; }
    public Color Color { get; private set; }
    public Sprite Sprite { get; private set; }

    public Stone(int id, Color color, Sprite sprite)
    {
        ID = id;
        Color = color;
        Sprite = sprite;
    }
}