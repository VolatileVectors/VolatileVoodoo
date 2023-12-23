using System.Collections.Generic;
using UnityEngine;
using VolatileVoodoo.Values.Base;

namespace VolatileVoodoo.Values
{
    [CreateAssetMenu(fileName = "UintValue", menuName = "Voodoo/Values/UintValue")]
    public class UintValue : GenericValue<uint> { }

    public class MyCustom : List<Dictionary<float, int>> { }
}