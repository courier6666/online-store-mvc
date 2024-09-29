using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Utils
{
    public static class EnumValuesGetter
    {
        public static string[] GetAllValues<TEnum>() where TEnum : struct
        {
            return Enum.
                GetValues(typeof(TEnum)).
                Cast<TEnum>().
                Select(s => s.ToString()).
                ToArray();
        }
    }
}
