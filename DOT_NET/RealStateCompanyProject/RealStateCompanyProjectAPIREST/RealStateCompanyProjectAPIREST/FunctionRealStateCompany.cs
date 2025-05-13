// Autor: Manuela Cortés Granados

// Importando las bibliotecas necesarias para manejar solicitudes HTTP, funciones de Azure y registros.
using Microsoft.AspNetCore.Http;  // Proporciona tipos para manejar solicitudes y respuestas HTTP.
using Microsoft.AspNetCore.Mvc;  // Contiene clases relacionadas con MVC (Model-View-Controller), especialmente para crear respuestas basadas en IActionResult.
using Microsoft.Azure.Functions.Worker;  // Parte del SDK de Azure Functions para crear funciones que se ejecutan en la plataforma Azure Functions.
using Microsoft.Extensions.Logging;  // Proporciona funcionalidad de registro para las funciones de Azure.

namespace RealStateCompanyProjectAPIREST
{
    // Clase principal que representa una función de Azure diseñada para procesar solicitudes HTTP.
    // Aquí es donde definimos nuestra función y manejamos las solicitudes activadas por HTTP.
    public class FunctionRealStateCompany
    {
        // Declaración de un campo privado de tipo logger para registrar información sobre la ejecución de la función.
        // El logger ayuda a depurar, realizar un seguimiento y proporcionar información sobre la ejecución de la función.
        private readonly ILogger<FunctionRealStateCompany> _logger;

        // Constructor de Inyección de Dependencias (DI) para ILogger<FunctionRealStateCompany>.
        // Esto asegura que nuestra función obtenga automáticamente una instancia de logger cuando se cree mediante Azure.
        // Este es un principio clave de SOLID, específicamente el principio de **Inyección de Dependencias** (D de SOLID),
        // que asegura un bajo acoplamiento al proporcionar las dependencias externamente.
        public FunctionRealStateCompany(ILogger<FunctionRealStateCompany> logger)
        {
            // Asignación del logger inyectado al campo privado.
            // La Inyección de Dependencias asegura que el logger pueda ser simulado o reemplazado en entornos de prueba,
            // manteniendo el código de la función independiente de implementaciones específicas.
            _logger = logger;
        }

        // Este es el punto de entrada para la función de Azure. Se activa cuando se recibe una solicitud HTTP.
        // El atributo "Function" marca este método como una función que se activará mediante solicitudes HTTP.
        // Soportamos tanto los métodos GET como POST mediante el atributo [HttpTrigger].
        // AuthorizationLevel.Function asegura que la función solo sea accesible mediante una clave de nivel función, garantizando seguridad.
        [Function("FunctionRealStateCompany")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            // Registrando un mensaje informativo cuando se activa la función.
            // Este mensaje aparecerá en los registros de Azure, proporcionando información sobre cuándo se ejecuta la función.
            _logger.LogInformation("La función C# HTTP trigger procesó una solicitud.");

            // Devolviendo una respuesta 200 OK con un mensaje de texto simple.
            // OkObjectResult es un tipo de resultado estándar en ASP.NET Core MVC para devolver un estado HTTP 200 con un cuerpo de respuesta.
            // Este es un ejemplo simple, pero en un escenario real, esto podría devolver datos dinámicos relacionados con la empresa inmobiliaria.
            return new OkObjectResult("¡Bienvenido a Azure Functions!"); // Utilizando el tipo de resultado MVC para un mejor manejo de API RESTful. 🌍
        }
    }
}
