namespace System
{
    public class GlobalConstants
    {
        public const string CustomerUserRoleName = "CustomerUser";
        public const string StoreAdminRoleName = "StoreAdmin";
        public const string CategoryNewKeyword = "New";
        public const string CategoryOnSalesKeyword = "On Sales!";
        public const string CategoryStoreKeyword = "Shop by Store";
        public const string CustomerSwaggerSecurityDefinitionKey = "CustomerServicesOAuth2";
        public const string StoreSwaggerSecurityDefinitionKey = "StoreServicesOAuth2";
        public static readonly string[] ApiScopes = new[] { "customer_services", "store_services" };


    }
}
