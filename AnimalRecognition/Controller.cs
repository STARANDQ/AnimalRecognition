namespace AnimalRecognition
{
    public class Controller
    {
        //SQL
        public static string dataSource = "LAPTOPSTARCHUK\\MSSQLSERVER01";
        public static string catalog = "Animal";
        public static string security = "True";
        public static string table = "AnimalRen";

        public static int countCheckBox = 35;

        
        public static string connectSql = "Data Source = " + dataSource + "; Initial Catalog = " + catalog + "; Integrated Security = " + security;

    }
}