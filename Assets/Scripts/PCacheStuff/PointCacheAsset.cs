using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Copy.VFX.Utils {
    [CreateAssetMenu(menuName = "pCache")]
    public class PointCacheAsset : ScriptableObject {
        public int PointCount;
        public Texture2D[] surfaces;
    }
}
