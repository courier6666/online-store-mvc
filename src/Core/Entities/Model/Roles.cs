using System.Text;

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
            foreach (var field in roleType.GetFields())
            {
                if (field.IsLiteral && !field.IsInitOnly && field.FieldType == typeof(string))
                    yield return field.GetRawConstantValue() as string ?? "";
            }
        }
        public static RolesStringBuilder RoleBuilder()
        {
            return new RolesStringBuilder();
        }
        public class RolesStringBuilder
        {
            StringBuilder rolesString;
            public RolesStringBuilder()
            {
                rolesString = new StringBuilder();
            }
            public RolesStringBuilder User()
            {
                rolesString.Append($"{Roles.User}, ");
                return this;
            }
            public RolesStringBuilder Admin()
            {
                rolesString.Append($"{Roles.Admin}, ");
                return this;
            }
            public void Clear()
            {
                this.rolesString.Clear();
            }
            public string Build()
            {
                if (rolesString.Length == 0)
                {
                    return string.Empty;
                }

                rolesString.Remove(rolesString.Length - 2, 1);
                var res = rolesString.ToString();
                this.Clear();
                return rolesString.ToString();
            }
            public override string ToString()
            {
                return this.Build();
            }
            public static implicit operator string(RolesStringBuilder rolesStringBuilder)
            {
                return rolesStringBuilder.Build();
            }
        }
    }
}
