using System.ComponentModel;

namespace ERP_API.Enums
{
    using System.ComponentModel;

    public enum Permission
    {
        // ===== CATEGORY =====
        [Description("View Categories")]
        Category_View,

        [Description("Add Category")]
        Category_Add,

        [Description("Edit Category")]
        Category_Edit,

        [Description("Delete Category")]
        Category_Delete,

        // ===== WAREHOUSE =====
        [Description("View Warehouses")]
        Warehouse_View,

        [Description("Add Warehouse")]
        Warehouse_Add,

        [Description("Edit Warehouse")]
        Warehouse_Edit,

        [Description("Delete Warehouse")]
        Warehouse_Delete,

        // ===== CUSTOMER =====
        [Description("View Customers")]
        Customer_View,

        [Description("Add Customer")]
        Customer_Add,

        [Description("Edit Customer")]
        Customer_Edit,

        [Description("Delete Customer")]
        Customer_Delete,

        // ===== SUPPLIER =====
        [Description("View Suppliers")]
        Supplier_View,

        [Description("Add Supplier")]
        Supplier_Add,

        [Description("Edit Supplier")]
        Supplier_Edit,

        [Description("Delete Supplier")]
        Supplier_Delete,

        // ===== ROLE =====
        [Description("View Roles")]
        Role_View,

        [Description("Add Role")]
        Role_Add,

        [Description("Edit Role")]
        Role_Edit,

        [Description("Delete Role")]
        Role_Delete,

        // ===== ACCOUNT =====
        [Description("View Accounts")]
        Account_View,

        [Description("Add Account")]
        Account_Add,

        [Description("Edit Account")]
        Account_Edit,

        [Description("Delete Account")]
        Account_Delete
    }

}
