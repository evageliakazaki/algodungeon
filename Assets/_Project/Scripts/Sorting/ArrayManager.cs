using System.Collections.Generic;
using UnityEngine;
using AlgoDungeon.Data;
using AlgoDungeon.Core;

namespace AlgoDungeon.Sorting
{
    public class ArrayManager : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private LevelData levelData;
        [SerializeField] private GameObject monsterTilePrefab;
        [SerializeField] private Transform tilesParent;

        [Header("References")]
        [SerializeField] private SortingRoomBase sortingRoom;

        public List<MonsterTile> Tiles { get; private set; } = new List<MonsterTile>();
        public LevelData Data => levelData;

        private void Start()
        {
            SpawnTiles();
            sortingRoom?.Initialize(this);
            GameEvents.AlgorithmStarted(levelData.algorithm.ToString());
        }

        private void SpawnTiles()
        {
            for (int i = 0; i < levelData.inputArray.Length; i++)
            {
                Vector3 pos = new Vector3(
                    levelData.tileStartPosition.x + i * levelData.tileSpacing,
                    levelData.tileStartPosition.y,
                    0);

                GameObject obj = Instantiate(monsterTilePrefab, pos, Quaternion.identity, tilesParent);
                MonsterTile tile = obj.GetComponent<MonsterTile>();
                tile.Initialize(levelData.inputArray[i], i, this);
                Tiles.Add(tile);
            }
        }

        // Καλείται από MonsterTile.OnInteract
        public void OnTileInteracted(MonsterTile tile)
        {
            sortingRoom?.HandleTileInteraction(tile);
        }

        // Φυσική ανταλλαγή δύο tiles
        public void SwapTiles(int indexA, int indexB)
        {
            if (indexA == indexB) return;

            MonsterTile tileA = Tiles[indexA];
            MonsterTile tileB = Tiles[indexB];

            Vector3 posA = tileA.transform.position;
            Vector3 posB = tileB.transform.position;

            StartCoroutine(tileA.MoveTo(posB, 0.3f));
            StartCoroutine(tileB.MoveTo(posA, 0.3f));

            tileA.CurrentIndex = indexB;
            tileB.CurrentIndex = indexA;
            Tiles[indexA] = tileB;
            Tiles[indexB] = tileA;

            GameEvents.TilesSwapped(indexA, indexB);
        }

        public bool IsSorted()
        {
            for (int i = 0; i < Tiles.Count - 1; i++)
                if (Tiles[i].Value > Tiles[i + 1].Value) return false;
            return true;
        }
    }
}
