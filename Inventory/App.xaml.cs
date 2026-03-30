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
        public static string SupabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InVraG91c3FtbXlwYXFqcWRlZ2J0Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NzQ4MjQ2NzgsImV4cCI6MjA5MDQwMDY3OH0.N0MiLiJrAFlotusHG0vJ5qsxIiSzQw6Ibf3FpkgPswI";
    }
}