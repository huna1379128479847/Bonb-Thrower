using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BombThrower
{
    public class SummonPoint : MonoBehaviour
    {
        public void Start()
        {
            HolderSummonController.instance.SetPoint(gameObject);
        }
    }
}
