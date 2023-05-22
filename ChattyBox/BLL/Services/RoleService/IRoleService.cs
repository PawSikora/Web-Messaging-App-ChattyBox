namespace BLL.Services.RoleService
{
    public interface IRoleService
    {
        void CreateRole(string name);
        void DeleteRole(int id);
        string GetRole(int id);
    }
}
