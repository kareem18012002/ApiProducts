using NoRiskNoFun.Data;

namespace NoRiskNoFun.Authorization
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CheckPermissionAattribute : Attribute 
    {
        public CheckPermissionAattribute(Permissions Permission)
        {
            this.Permission = Permission;
        }

        public Permissions Permission { get; }
    }
}
