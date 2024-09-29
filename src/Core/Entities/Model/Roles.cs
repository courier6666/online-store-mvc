using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Entities.Model
{
    /// <summary>
    /// ONLY public const fields allowed of type string
    /// </summary>
    public static class Roles
    {
        public const string User = "user";
        public const string Admin = "admin";
        public static IEnumerable<string> GetAllRoles()
        {
            Type roleType = typeof(Roles);
            foreach(var field in roleType.GetFields())
            {
                if(field.IsLiteral && !field.IsInitOnly && field.FieldType == typeof(string))
                    yield return field.GetRawConstantValue() as string ?? "";
            }
        }
    }
}
