using System.Windows;

namespace Inventory
{
    public partial class App : Application
    {
        // Local SQL Server connection string
        public static string LocalConnectionString =
            "Server=LAPTOP-G4J9KNE9;Database=InventoryDB;Trusted_Connection=True;TrustServerCertificate=True;";

        // Supabase credentials
        public static string SupabaseUrl = "https://ukhousqmmypaqjqdegbt.supabase.co";
        public static string SupabaseKey = "your-anon-key-here";
    }
}