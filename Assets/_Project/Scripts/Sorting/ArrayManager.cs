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
            Debug.Log(
                $"SCENE USES LEVEL DATA: {levelData.name} | ARRAY LENGTH: {levelData.inputArray.Length}"
            );

            SpawnTiles();
            sortingRoom?.Initialize(this);
            GameEvents.AlgorithmStarted(levelData.algorithm.ToString());
        }

        private void SpawnTiles()
        {
            Tiles.Clear();

            if (levelData == null)
            {
                Debug.LogError("ArrayManager: Δεν έχει οριστεί LevelData.");
                return;
            }

            if (monsterTilePrefab == null)
            {
                Debug.LogError("ArrayManager: Δεν έχει οριστεί Monster Tile Prefab.");
                return;
            }

            if (tilesParent == null)
            {
                Debug.LogError("ArrayManager: Δεν έχει οριστεί Tiles Parent.");
                return;
            }

            // Καθαρίζει όσα tiles υπάρχουν ήδη μέσα στο TilesParent.
            for (int i = tilesParent.childCount - 1; i >= 0; i--)
            {
                Destroy(tilesParent.GetChild(i).gameObject);
            }

            int count = levelData.inputArray.Length;
            float spacing = levelData.tileSpacing;

            // Αυτό κεντράρει αυτόματα το array γύρω από το Tile Start Position X.
            float totalWidth = (count - 1) * spacing;
            float startX = levelData.tileStartPosition.x - totalWidth / 2f;
            float y = levelData.tileStartPosition.y;

            Debug.Log($"Spawning {count} tiles from {levelData.name}");

            for (int i = 0; i < count; i++)
            {
                Vector3 pos = new Vector3(
                    startX + i * spacing,
                    y,
                    0);

                GameObject obj = Instantiate(
                    monsterTilePrefab,
                    pos,
                    Quaternion.identity,
                    tilesParent);

                MonsterTile tile = obj.GetComponent<MonsterTile>();

                if (tile == null)
                {
                    Debug.LogError("ArrayManager: Το prefab δεν έχει MonsterTile component.");
                    continue;
                }

                tile.Initialize(levelData.inputArray[i], i, this);
                Tiles.Add(tile);
            }
        }

        public void OnTileInteracted(MonsterTile tile)
        {
            Debug.Log("ArrayManager πήρε tile: " + tile.Value);
            sortingRoom?.HandleTileInteraction(tile);
        }

        public void SwapTiles(int indexA, int indexB)
        {
            if (indexA == indexB)
                return;

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
            {
                if (Tiles[i].Value > Tiles[i + 1].Value)
                    return false;
            }

            return true;
        }
    }
}