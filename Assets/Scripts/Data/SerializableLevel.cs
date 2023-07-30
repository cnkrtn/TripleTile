using System.Collections.Generic;

namespace Data
{
    [System.Serializable]
    public class SerializableLevel
    {
        
        public List<SerializableGrid> Grids;

        public SerializableLevel( List<SerializableGrid> grids)
        {
            
            Grids = grids;
        }
    }
}