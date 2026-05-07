using System;
using UnityEngine;

namespace AlgoDungeon.Core
{
    public static class GameEvents
    {
        // Sorting events
        public static event Action<int, int> OnTilesCompared;    // (indexA, indexB)
        public static event Action<int, int> OnTilesSwapped;     // (indexA, indexB)
        public static event Action<int> OnTileMarkedSorted;       // (index)
        public static event Action<int> OnRoomCompleted;          // (stars 1-3)
        public static event Action<string> OnAlgorithmStarted;    // (algorithmName)

        // Player events
        public static event Action<Vector2> OnPlayerMoved;

        // Trigger methods (καλούνται από όπου χρειάζεται)
        public static void TilesCompared(int a, int b) => OnTilesCompared?.Invoke(a, b);
        public static void TilesSwapped(int a, int b) => OnTilesSwapped?.Invoke(a, b);
        public static void TileMarkedSorted(int i) => OnTileMarkedSorted?.Invoke(i);
        public static void RoomCompleted(int stars) => OnRoomCompleted?.Invoke(stars);
        public static void AlgorithmStarted(string name) => OnAlgorithmStarted?.Invoke(name);
        public static void PlayerMoved(Vector2 pos) => OnPlayerMoved?.Invoke(pos);
    }
}



