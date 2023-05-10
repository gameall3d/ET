using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    [DisallowMultipleComponent]
    [AddComponentMenu("")]
    public class PoolObject : MonoBehaviour
    {
        public string PoolName;
        //defines whether the object is waiting in pool or is in use
        public bool IsPooled;
    }
}
