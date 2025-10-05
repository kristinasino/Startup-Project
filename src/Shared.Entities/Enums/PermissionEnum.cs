namespace Shared.Entities.Enums
{
    public enum PermissionEnum
    {
        //UserManagement
        UserView,
        UserCreate,
        UserEdit,
        UserDelete,
        
        //RoleManagement
        RoleView,
        RoleCreate,
        RoleEdit,
        RoleDelete,
        
        //SystemLogs
        ErrorLogs,
        Audit,

        //LocationManagement
        LocationInsert,

        //TenantManagement
        TenantInsert,
        
        //RecycleFormManagement
        RecycleFormView,
        RecycleFormCreate,
        RecycleFormUpdate,
        
        //TonnageFormManagement
        TonnageFormView,
        TonnageFormCreate,
        TonnageFormUpdate,
        
        //Reports
        TonnageReport,
        
        //Contract
        CustomerContractInsert,
        
        //CustomerEquipment
        CustomerLocationEquipmentInsert
    }
}