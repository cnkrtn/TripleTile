using System.Collections.Generic;
using UnityEngine;

namespace InGame
{
    public class ShuffleLogic : MonoBehaviour
    {
        public List<RectTransform> slotsToShuffle;
        [SerializeField] private List<RectTransform> allSlots;

        public void AddToListToShuffle()
        {
            foreach (var parent in allSlots)
            {
                for (int i = 0; i < parent.transform.childCount; i++)
                {
                    var child = (RectTransform)parent.transform.GetChild(i);
                    if (child.childCount > 0)
                    {
                        slotsToShuffle.Add(child);
                    }
                }
            }
        }


        public void Shuffle()
        {
            slotsToShuffle.Clear();
            AddToListToShuffle();
            List<RectTransform> stonesToShuffle = new();
            foreach (var slot in slotsToShuffle)
            {
                var stone = (RectTransform)slot.transform.GetChild(0);
                stone.SetParent(null);
                stonesToShuffle.Add(stone);
            }

            ShuffleList(stonesToShuffle);


            for (int i = 0; i < slotsToShuffle.Count; i++)
            {
                stonesToShuffle[i].transform.SetParent(slotsToShuffle[i]);
            }

            foreach (var stone in stonesToShuffle)
            {
                var gridStone = stone.GetComponent<GridStone>();
                gridStone.cellsToCheck.Clear();
                gridStone.stonesToCheck.Clear();
                gridStone.AddToCellsToCheck();
                gridStone.IsItClickable();
                stone.localPosition = Vector3.zero;
            }
        }

        private void ShuffleList(List<RectTransform> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = Random.Range(0, n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}