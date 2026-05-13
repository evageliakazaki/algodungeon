using UnityEngine;

namespace AlgoDungeon.Core
{
    public class ProgressBootstrapper : MonoBehaviour
    {
        private void Awake()
        {
            GameProgressManager.UnlockLevel("Selection_Level_01");
        }
    }
}
