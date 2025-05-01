namespace POC_001_HelloWorldREST
{
    /// <summary>
    /// Clase de configuración que representa los parámetros necesarios para conectarse a una base de datos MongoDB.
    /// Esta clase es utilizada para mapear las secciones correspondientes en el archivo de configuración `appsettings.json`.
    /// 
    /// Principios SOLID aplicados:
    /// - SRP (Principio de Responsabilidad Única): esta clase tiene una única responsabilidad: almacenar los valores de configuración de MongoDB.
    /// </summary>
    public class MongoDbSettings
    {
        /// <summary>
        /// Cadena de conexión completa hacia el servidor MongoDB.
        /// Este valor se obtiene desde la sección de configuración, típicamente "MongoDbSettings:ConnectionString".
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Nombre de la base de datos MongoDB a la que se conectará la aplicación.
        /// Este valor se obtiene desde la sección de configuración, típicamente "MongoDbSettings:DatabaseName".
        /// </summary>
        public string DatabaseName { get; set; }
    }
}
