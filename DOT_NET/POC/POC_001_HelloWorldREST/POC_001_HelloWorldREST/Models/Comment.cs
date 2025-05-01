namespace POC_001_HelloWorldREST.Models
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System;

    /// <summary>
    /// Representa un comentario realizado por un usuario asociado a una película.
    /// Cumple con el Principio de Responsabilidad Única (SRP) de SOLID,
    /// ya que su única responsabilidad es modelar los datos del comentario.
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Obtiene o establece el identificador único del comentario.
        /// Este campo actúa como clave primaria en MongoDB.
        /// </summary>
        /// <remarks>
        /// SOLID - SRP: Esta propiedad solo representa el ID del comentario.
        /// </remarks>
        [BsonId]
        public ObjectId Id { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del usuario que hizo el comentario.
        /// </summary>
        /// <remarks>
        /// SOLID - SRP: Campo específico para almacenar datos del usuario.
        /// </remarks>
        [BsonElement("name")]
        public string Name { get; set; }

        /// <summary>
        /// Obtiene o establece el correo electrónico del usuario que hizo el comentario.
        /// </summary>
        /// <remarks>
        /// SOLID - SRP: Dato relacionado exclusivamente con la identidad del usuario.
        /// </remarks>
        [BsonElement("email")]
        public string Email { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador de la película a la que pertenece el comentario.
        /// Este campo actúa como una referencia externa (clave foránea).
        /// </summary>
        /// <remarks>
        /// SOLID - SRP: Relaciona esta instancia con una entidad de película específica.
        /// </remarks>
        [BsonElement("movie_id")]
        public ObjectId MovieId { get; set; }

        /// <summary>
        /// Obtiene o establece el contenido del comentario realizado por el usuario.
        /// </summary>
        /// <remarks>
        /// SOLID - SRP: Encapsula el mensaje principal del comentario.
        /// </remarks>
        [BsonElement("text")]
        public string Text { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha y hora en la que se creó el comentario.
        /// </summary>
        /// <remarks>
        /// SOLID - SRP: Captura metadatos temporales del comentario.
        /// </remarks>
        [BsonElement("date")]
        public DateTime Date { get; set; }
    }
}
