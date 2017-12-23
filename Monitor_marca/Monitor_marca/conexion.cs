using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Monitor_marca
{
    class conexion
    {
        //MySqlConnection connection = new MySqlConnection();
                public static string servidor = ConfigurationManager.AppSettings["servidor"];
        public static string puerto = ConfigurationManager.AppSettings["puertobd"];
        public static string bd = ConfigurationManager.AppSettings["bd"];
        public static string usuario = ConfigurationManager.AppSettings["usuario"];
        public static string clave = ConfigurationManager.AppSettings["clave"];

       public static string clave1 = clave.Substring(2);
       public static string connectionstring = "Server=" + servidor + ";Port=3306;Database=" + bd + ";Uid=" + usuario + ";Pwd=" + clave1 + "";
       
        public static MySqlConnection ObtenerCOnexion()
        {



            MySqlConnection connection = new MySqlConnection(connectionstring);

            connection.Open();
            return connection;



        }
    }

}
