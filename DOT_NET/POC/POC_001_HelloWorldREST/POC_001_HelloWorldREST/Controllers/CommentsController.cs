using Microsoft.AspNetCore.Mvc;                // Framework ASP.NET Core para construir servicios web RESTful
using MongoDB.Driver;                          // Biblioteca oficial de MongoDB para .NET
using POC_001_HelloWorldREST.Models;           // Espacio de nombres que contiene los modelos de datos (por ejemplo, Comment)
using POC_001_HelloWorldREST.Services;         // Espacio de nombres que contiene los servicios de acceso a datos
using System;                                   // Tipos básicos de .NET
using System.Collections.Generic;               // Tipos genéricos como List<T>
using System.Threading.Tasks;                   // Funcionalidad para operaciones asincrónicas

namespace POC_001_HelloWorldREST.Controllers
{
    // Define el controlador como parte de una API REST y especifica la ruta base "api/comments".
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        // Servicio de acceso a datos que interactúa con la base de datos MongoDB.
        private readonly MongoDbService _mongoDbService;

        /// <summary>
        /// Constructor del controlador que recibe una instancia del servicio MongoDbService.
        /// Implementa la inyección de dependencias, favoreciendo pruebas y mantenimiento.
        /// </summary>
        /// <param name="mongoDbService">Servicio de acceso a MongoDB para comentarios.</param>
        public CommentsController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        /// <summary>
        /// Método HTTP GET que retorna los primeros 20 comentarios almacenados en MongoDB.
        /// </summary>
        /// <returns>
        /// Un listado de objetos tipo Comment en una respuesta HTTP 200 OK,
        /// o un error HTTP 500 con información contextual si ocurre una excepción.
        /// </returns>
        [HttpGet]
        public async Task<ActionResult<List<Comment>>> GetFirst20Comments()
        {
            try
            {
                // Llama al servicio para obtener los primeros 20 documentos de la colección "comments".
                var comments = await _mongoDbService.GetFirst20CommentsAsync();

                // Devuelve los comentarios con estado HTTP 200 (OK).
                return Ok(comments);
            }
            catch (Exception ex)
            {
                // Manejo especializado para errores relacionados con MongoDB o demoras de conexión.
                if (ex is MongoException || ex is TimeoutException)
                {
                    // Retorna un error HTTP 500 con detalles del problema en MongoDB.
                    return StatusCode(500, new
                    {
                        message = "Ocurrió un error en la base de datos",
                        error = ex.Message
                    });
                }

                // En caso de error inesperado, también se responde con HTTP 500 pero con mensaje genérico.
                return StatusCode(500, new
                {
                    message = "Ocurrió un error inesperado",
                    error = ex.Message
                });
            }
        }
    }
}
