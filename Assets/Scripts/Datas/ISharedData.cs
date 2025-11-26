using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Datas
{
    public interface ISharedData
    {
        bool Lock { get; set; }
        GameEvent Event { get; set; }
        GameStatus Status { get; set; }

        Unit Destination {  get; set; }
        Cell Target {  get; set; }
    }
}