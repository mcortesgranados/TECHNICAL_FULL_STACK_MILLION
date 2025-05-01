namespace POC_001_HelloWorldREST.Services
{
    using MongoDB.Driver;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using POC_001_HelloWorldREST.Models;

    /// <summary>
    /// Servicio encargado de interactuar con la base de datos MongoDB.
    /// Este servicio gestiona las operaciones de lectura de los comentarios almacenados en la colección "comments".
    /// 
    /// Principios SOLID aplicados:
    /// - **SRP (Principio de Responsabilidad Única):** El servicio tiene una responsabilidad única: interactuar con la base de datos para obtener comentarios.
    /// - **D (Principio de Inversión de Dependencias):** La dependencia de la configuración de MongoDB es inyectada a través de la interfaz `IOptions<MongoDbSettings>`, lo que permite desacoplar la configuración de la lógica del servicio.
    /// </summary>
    public class MongoDbService
    {
        // Colección de comentarios en MongoDB. Esta es la colección sobre la que operan las consultas.
        private readonly IMongoCollection<Comment> _commentsCollection;

        /// <summary>
        /// Constructor de la clase MongoDbService.
        /// Inicializa la conexión con la base de datos y asigna la colección de comentarios.
        /// 
        /// Utiliza la inyección de dependencias para obtener la configuración de MongoDB desde el archivo de configuración.
        /// </summary>
        /// <param name="mongoDbSettings">Configuración de MongoDB (cadena de conexión y nombre de la base de datos).</param>
        public MongoDbService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            // Creación del cliente MongoDB usando la cadena de conexión proporcionada
            var client = new MongoClient(mongoDbSettings.Value.ConnectionString);

            // Obtención de la base de datos usando el nombre proporcionado
            var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);

            // Asignación de la colección 'comments' a la variable _commentsCollection
            _commentsCollection = database.GetCollection<Comment>("comments");
        }

        /// <summary>
        /// Método asíncrono que obtiene los primeros 20 comentarios de la colección "comments".
        /// Este método utiliza el límite `.Limit(20)` para restringir los resultados a los primeros 20 registros.
        /// 
        /// Principios SOLID aplicados:
        /// - **SRP (Principio de Responsabilidad Única):** Este método se encarga exclusivamente de recuperar los primeros 20 comentarios, sin mezclar responsabilidades de manipulación de datos o lógica de negocio.
        /// - **L (Principio de Sustitución de Liskov):** Si se decide extender este servicio, el método puede ser sobrescrito o extendido sin afectar el comportamiento actual.
        /// </summary>
        /// <returns>Una lista de objetos `Comment` representando los primeros 20 comentarios en la base de datos.</returns>
        public async Task<List<Comment>> GetFirst20CommentsAsync()
        {
            try
            {
                // Realiza la consulta en la colección "comments" y limita los resultados a 20
                return await _commentsCollection.Find(comment => true)  // Encuentra todos los comentarios
                                                .Limit(20)           // Limita la consulta a los primeros 20 comentarios
                                                .ToListAsync();      // Convierte el resultado a una lista de manera asíncrona
            }
            catch (MongoException ex)
            {
                // Si ocurre un error relacionado con MongoDB, lanza una excepción con un mensaje detallado.
                throw new Exception("Ocurrió un error en la base de datos al acceder a MongoDB: " + ex.Message, ex);
            }
            catch (TimeoutException ex)
            {
                // Si la solicitud a MongoDB excede el tiempo de espera, lanza una excepción de tiempo de espera.
                throw new Exception("La solicitud a MongoDB superó el tiempo de espera: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                // Si ocurre cualquier otro tipo de error inesperado, lanza una excepción genérica.
                throw new Exception("Ocurrió un error inesperado: " + ex.Message, ex);
            }
        }
    }
}
