using SmileService.Models;

namespace SmileService
{
    public static class AppSession
    {
        // Здесь будет храниться залогиненный сотрудник
        public static User CurrentUser { get; set; }

        // Быстрая проверка прав для удобства в коде
        public static bool IsAdmin => CurrentUser?.Role == "Admin";
        public static bool IsMaster => CurrentUser?.Role == "Master";
        public static bool IsReceptionist => CurrentUser?.Role == "Receptionist";
        public static bool IsStorekeeper => CurrentUser?.Role == "Storekeeper";
    }
}