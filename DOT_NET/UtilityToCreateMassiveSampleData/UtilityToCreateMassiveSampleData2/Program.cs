using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoSampleData
{
    // 💡 Clase Owner: Responsable de representar al dueño de una propiedad.
    // Cada propiedad de esta clase tiene un único propósito, alineándose con el Principio de Responsabilidad Única (SRP) de SOLID.
    // Esto facilita la gestión de datos de propietarios sin mezclar funcionalidades adicionales que no correspondan a esta entidad.
    public class Owner
    {
        [BsonId]
        public ObjectId Id { get; set; } // 🧑‍💼 Identificador único para cada dueño. Es fundamental para evitar duplicados y asegurar que cada dueño tenga un único registro en la base de datos.
        public string Name { get; set; }  // 👤 Nombre del dueño. Es un dato básico pero esencial para personalizar la interacción con el sistema.
        public string Address { get; set; }  // 🏠 Dirección del dueño. Esta información puede ser útil para personalizar la experiencia y realizar validaciones si fuera necesario.
        public string Photo { get; set; }  // 📸 Foto del dueño. Aunque es un dato más visual, podría usarse para mostrar imágenes en la interfaz de usuario.
        public DateTime Birthday { get; set; }  // 🎂 Fecha de nacimiento del dueño. Nos ayuda a entender más sobre la edad del dueño, lo que podría ser útil para ciertos cálculos o categorizaciones.
    }

    // 💎 Clase Property: Representa las propiedades que los dueños poseen.
    // Aquí aplicamos el principio de SRP, ya que esta clase solo se encarga de gestionar los datos relacionados con las propiedades, como su precio, ubicación, etc.
    public class Property
    {
        [BsonId]
        public ObjectId Id { get; set; } // 🔑 Identificador único de la propiedad. Es un campo esencial para identificar de manera exclusiva cada propiedad.
        public string Name { get; set; } // 🏡 Nombre de la propiedad. Un atributo importante para diferenciar las propiedades entre sí.
        public string Address { get; set; } // 🌍 Dirección de la propiedad. Esta es la ubicación física y puede ser un dato crucial para determinar el valor o la cercanía a otros puntos de interés.
        public decimal Price { get; set; } // 💰 Precio de la propiedad. Este es probablemente el atributo más importante para la compra y venta de propiedades, así que su manejo adecuado es vital.
        public string CodeInternal { get; set; } // 🏷️ Código interno de la propiedad. Un identificador alfanumérico que podría utilizarse internamente para la gestión o categorización de propiedades.
        public int Year { get; set; } // 🗓️ Año de construcción de la propiedad. Esto puede ser relevante para determinar el valor de la propiedad o la antigüedad de la misma.
        public ObjectId OwnerId { get; set; } // 🧑‍🤝‍🧑 Relación con el dueño. Este es un campo de referencia que conecta la propiedad con su propietario correspondiente.
    }

    // 🖼️ Clase PropertyImage: Representa las imágenes asociadas con una propiedad.
    // De nuevo, SRP está presente, ya que esta clase se centra exclusivamente en los detalles de las imágenes relacionadas con una propiedad. 
    // Asegura que no se mezclen responsabilidades con la clase Property.
    public class PropertyImage
    {
        [BsonId]
        public ObjectId Id { get; set; } // 🖼️ Identificador único de la imagen. Es importante para manejar cada imagen de manera separada.
        public ObjectId PropertyId { get; set; } // 🔑 Relación con la propiedad a la que pertenece la imagen. Esto permite organizar las imágenes asociadas a cada propiedad.
        public string File { get; set; } // 🌐 URL de la imagen. Una cadena que contiene la dirección de la imagen, lo que facilita la visualización en la interfaz de usuario.
        public bool Enabled { get; set; } // ✔️ Indica si la imagen está habilitada o no. Esto es útil para activar/desactivar imágenes según las necesidades de visualización.
    }

    // 🏷️ Clase PropertyTrace: Representa el historial de ventas de una propiedad.
    // Esta clase se asegura de que la venta de la propiedad se gestione adecuadamente, siguiendo el principio de SRP para no mezclar responsabilidades.
    public class PropertyTrace
    {
        [BsonId]
        public ObjectId Id { get; set; } // 🔑 Identificador único para cada registro de venta. Esto permite un rastreo preciso del historial de cada propiedad.
        public ObjectId PropertyId { get; set; } // 🏡 Relación con la propiedad. Nos permite vincular cada venta con la propiedad correspondiente.
        public DateTime DateSale { get; set; } // 📅 Fecha de la venta. Es clave para poder gestionar y consultar el historial de ventas de las propiedades.
        public string Name { get; set; } // 🏷️ Nombre de la transacción o tipo de venta. Esto puede ser útil para distinguir entre ventas, alquileres u otros tipos de transacciones.
        public decimal Value { get; set; } // 💸 Valor de la venta. Es el monto en el que se realizó la transacción, un dato esencial en cualquier negocio relacionado con propiedades.
        public decimal Tax { get; set; } // 💵 Impuesto relacionado con la venta. Es importante tener este dato para cálculos fiscales o reportes financieros.
    }

    // 🚀 Clase principal que se encarga de interactuar con MongoDB.
    // Esta clase sigue buenas prácticas de manejo de excepciones y organiza la inserción de datos en lotes para evitar sobrecargar el sistema. Además, está diseñada de forma eficiente para manejar grandes volúmenes de datos.
    class Program
    {
        static async Task Main(string[] args)
        {
            var connectionString = "mongodb+srv://usuario:contraseña@cluster0.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0"; // 📡 Cadena de conexión a la base de datos. Recuerda mantenerla segura.
            int totalOwners = 1000; // 🧑‍💼 Total de dueños que se van a insertar.
            int batchSize = 100; // 📦 Tamaño del lote para insertar datos. Se recomienda no insertar todo de una vez para evitar sobrecargar la base de datos.

            try
            {
                Console.WriteLine("Conectando a MongoDB...");

                // Crear el cliente de MongoDB.
                var client = new MongoClient(connectionString);
                Console.WriteLine("Cliente MongoDB creado.");

                // Verificar la conexión al intentar listar las bases de datos.
                var databaseList = await client.ListDatabaseNamesAsync();
                await databaseList.ToListAsync(); // Lanza una excepción si la conexión falla.

                Console.WriteLine("Conexión exitosa a MongoDB.");

                // Obtener la base de datos y las colecciones.
                var database = client.GetDatabase("million_properties_db");

                var ownersCollection = database.GetCollection<Owner>("owners");
                var propertiesCollection = database.GetCollection<Property>("properties");
                var imagesCollection = database.GetCollection<PropertyImage>("propertyImages");
                var tracesCollection = database.GetCollection<PropertyTrace>("propertyTraces");

                // Borrar los datos existentes antes de insertar nuevos.
                Console.WriteLine("Borrando datos existentes...");
                await ownersCollection.DeleteManyAsync(FilterDefinition<Owner>.Empty);
                await propertiesCollection.DeleteManyAsync(FilterDefinition<Property>.Empty);
                await imagesCollection.DeleteManyAsync(FilterDefinition<PropertyImage>.Empty);
                await tracesCollection.DeleteManyAsync(FilterDefinition<PropertyTrace>.Empty);
                Console.WriteLine("Datos existentes borrados.");

                // Insertar datos de prueba.
                Console.WriteLine("Insertando datos...");

                var random = new Random();
                var ownerIds = new List<ObjectId>();

                // Insertar propietarios.
                for (int i = 0; i < totalOwners; i++)
                {
                    var owner = new Owner
                    {
                        Id = ObjectId.GenerateNewId(),
                        Name = $"Owner {i + 1}",
                        Address = $"Address {i + 1} - City",
                        Photo = $"https://example.com/photo{i + 1}.jpg",
                        Birthday = new DateTime(1990, 1, 1).AddYears(random.Next(0, 30))
                    };
                    ownerIds.Add(owner.Id);
                    await ownersCollection.InsertOneAsync(owner);

                    if ((i + 1) % batchSize == 0)
                    {
                        Console.WriteLine($"Insertados {i + 1} propietarios...");
                    }
                }

                // Insertar propiedades, imágenes e historial en paralelo.
                var insertTasks = new List<Task>();

                for (int i = 0; i < totalOwners; i++)
                {
                    var ownerId = ownerIds[i];
                    var propertyBatch = new List<Property>();
                    var imageBatch = new List<PropertyImage>();
                    var traceBatch = new List<PropertyTrace>();

                    // Crear propiedades, imágenes e historial de ventas.
                    for (int j = 0; j < 1; j++) // Se puede aumentar el número de propiedades por dueño.
                    {
                        var property = new Property
                        {
                            Id = ObjectId.GenerateNewId(),
                            Name = $"Luxury Villa {i + 1}",
                            Address = $"456 Ocean Drive {i + 1}",
                            Price = random.Next(50_000, 2_000_000),
                            CodeInternal = $"PROP{(i + 1):D5}",
                            Year = random.Next(1990, 2024),
                            OwnerId = ownerId
                        };
                        propertyBatch.Add(property);

                        var image = new PropertyImage
                        {
                            Id = ObjectId.GenerateNewId(),
                            PropertyId = property.Id,
                            File = $"https://example.com/image{i + 1}.jpg",
                            Enabled = true
                        };
                        imageBatch.Add(image);

                        var trace = new PropertyTrace
                        {
                            Id = ObjectId.GenerateNewId(),
                            PropertyId = property.Id,
                            DateSale = new DateTime(2022, 1, 1),
                            Name = "First Sale",
                            Value = random.Next(500_000, 1_500_000),
                            Tax = random.Next(1000, 10000)
                        };
                        traceBatch.Add(trace);
                    }

                    // Insertar en paralelo las propiedades, imágenes e historial.
                    insertTasks.Add(propertiesCollection.InsertManyAsync(propertyBatch));
                    insertTasks.Add(imagesCollection.InsertManyAsync(imageBatch));
                    insertTasks.Add(tracesCollection.InsertManyAsync(traceBatch));

                    if ((i + 1) % batchSize == 0)
                    {
                        Console.WriteLine($"Insertados {i + 1} propiedades, imágenes e historiales...");
                    }
                }

                // Esperar a que todas las inserciones finalicen.
                await Task.WhenAll(insertTasks);

                Console.WriteLine("✅ Datos insertados exitosamente.");
            }
            catch (Exception ex)
            {
                // Manejo de errores. Imprimir el mensaje si la conexión falla.
                Console.WriteLine("❌ Error al conectar con MongoDB Atlas: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);
            }
        }
    }
}
