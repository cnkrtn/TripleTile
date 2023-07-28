using System.Collections.Generic;

[System.Serializable]
public class SerializableLevel
{
    public int LevelIndex;
    public List<SerializableGrid> Grids;

    public SerializableLevel(int levelIndex, List<SerializableGrid> grids)
    {
        LevelIndex = levelIndex;
        Grids = grids;
    }
}