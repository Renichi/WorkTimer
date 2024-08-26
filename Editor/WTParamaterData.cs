namespace WT
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu]
    public class WTParamaterData : ScriptableObject
    {
        [SerializeField] private double totalTime;
        public double TotalTime { get { return this.totalTime; } set { this.totalTime = value; } }
    }
}




