



// Título Principal
/*
 * *****************************************
 * UNIVERSIDAD ESTATAL AMAZÓNICA
 * GESTIÓN DE VUELOS
 * *****************************************
 */

public class Flight
{
    public string From { get; set; }
    public string To { get; set; }
    public decimal Cost { get; set; }
    public string DepartureTime { get; set; } // Nuevo campo para la hora de salida

    public Flight(string from, string to, decimal cost, string departureTime)
    {
        From = from;
        To = to;
        Cost = cost;
        DepartureTime = departureTime;
    }
}

public class Graph
{
    // Usa 'string' para el aeropuerto de origen y una lista de 'Flight' para sus salidas
    public Dictionary<string, List<Flight>> adjacencyList = new Dictionary<string, List<Flight>>();

    public void AddFlight(Flight flight)
    {
        if (!adjacencyList.ContainsKey(flight.From))
        {
            adjacencyList[flight.From] = new List<Flight>();
        }
        adjacencyList[flight.From].Add(flight);
    }

    public void AddFlightFromConsole()
    {
        Console.WriteLine("\n--- Agregar Nuevo Vuelo ---");
        Console.Write("Ingrese el aeropuerto de origen (e.g., GYE): ");
        string? fromInput = Console.ReadLine();
        string from = fromInput?.ToUpperInvariant() ?? string.Empty;

        Console.Write("Ingrese el aeropuerto de destino (e.g., UIO): ");
        string? toInput = Console.ReadLine();
        string to = toInput?.ToUpperInvariant() ?? string.Empty;

        Console.Write("Ingrese el costo del vuelo: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal cost))
        {
            Console.WriteLine(" Costo inválido. Vuelo no agregado.");
            return;
        }

        Console.Write("Ingrese la hora de salida (e.g., 08:30): ");
        string? timeInput = Console.ReadLine();
        string time = timeInput ?? string.Empty;

        Flight newFlight = new Flight(from, to, cost, time);
        AddFlight(newFlight);
        Console.WriteLine($"\n Vuelo de {from} a {to} agregado exitosamente.");
    }

    public List<Flight> GetFlightsFrom(string airport)
    {
        // Se asegura de que el aeropuerto esté en mayúsculas para la búsqueda consistente
        return adjacencyList.ContainsKey(airport.ToUpperInvariant()) 
            ? adjacencyList[airport.ToUpperInvariant()] 
            : new List<Flight>();
    }

    public IEnumerable<string> GetAllAirports()
    {
        // Obtiene una lista de todos los aeropuertos (origen y destino) para el algoritmo de Dijkstra
        var origins = adjacencyList.Keys;
        var destinations = adjacencyList.Values.SelectMany(flights => flights.Select(f => f.To));
        return origins.Concat(destinations).Distinct().ToList();
    }
}

public static class Database
{
    // Creación de la base de datos de vuelos con rutas en Ecuador
    public static Graph CreateSampleDatabase()
    {
        Graph graph = new Graph();

        // Rutas dentro de Ecuador (ejemplos)
        // GYE: Guayaquil, UIO: Quito, CUE: Cuenca, MDE: Medellín (Colombia)
        graph.AddFlight(new Flight("GYE", "UIO", 80m, "07:00"));
        graph.AddFlight(new Flight("UIO", "GYE", 75m, "08:30"));
        graph.AddFlight(new Flight("GYE", "CUE", 120m, "10:15"));
        graph.AddFlight(new Flight("CUE", "UIO", 90m, "12:00"));
        graph.AddFlight(new Flight("UIO", "MDE", 350m, "14:45")); // Ruta internacional
        graph.AddFlight(new Flight("CUE", "GYE", 110m, "16:30"));
        graph.AddFlight(new Flight("GYE", "MDE", 300m, "18:00"));

        return graph;
    }
}

public static class DijkstraAlgorithm
{
    // Algoritmo de Dijkstra para encontrar la ruta más barata
    public static (Dictionary<string, decimal>, Dictionary<string, string>) FindCheapestRoute(Graph graph, string start, string end)
    {
        // Convierte a mayúsculas para consistencia
        start = start.ToUpperInvariant();
        end = end.ToUpperInvariant();
        
        Dictionary<string, decimal> distances = new Dictionary<string, decimal>();
        Dictionary<string, string> previous = new Dictionary<string, string>();

        // Inicialización de distancias
        foreach (var airport in graph.GetAllAirports())
        {
            distances[airport] = decimal.MaxValue;
        }
        
        // El aeropuerto de inicio se inicializa a 0 solo si es un aeropuerto válido
        if (distances.ContainsKey(start))
        {
            distances[start] = 0;
        }
        else if(graph.GetAllAirports().Any()){
            // Si el aeropuerto de inicio no estaba en la lista, lo agrega con distancia 0
            distances[start] = 0;
        }
        else
        {
            // Caso especial: Grafo vacío
            return (distances, previous);
        }

        // PriorityQueue simulada con SortedSet para obtener el elemento de menor costo rápidamente
        // En C#, SortedSet funciona como un min-heap para este propósito.
        var priorityQueue = new SortedSet<(decimal, string)>();
        priorityQueue.Add((0, start));

        while (priorityQueue.Count > 0)
        {
            // Obtener el aeropuerto con la menor distancia actual (el primer elemento del SortedSet)
            var (currentDistance, currentAirport) = priorityQueue.Min;
            priorityQueue.Remove(priorityQueue.Min); // Remover el nodo que estamos visitando

            // Si ya encontramos el destino o si la distancia actual es infinita, se detiene.
            if (currentAirport == end || currentDistance == decimal.MaxValue) break;
            
            // Si el costo actual registrado en el SortedSet es mayor que el registrado en distances (lo cual puede pasar si el nodo se actualizó y se re-añadió a la cola), se ignora esta iteración.
            if(currentDistance > distances.GetValueOrDefault(currentAirport, decimal.MaxValue)) continue;


            foreach (var flight in graph.GetFlightsFrom(currentAirport))
            {
                // Calcula el nuevo costo total
                var newDistance = currentDistance + flight.Cost;
                
                // Si encontramos un camino más barato
                if (newDistance < distances.GetValueOrDefault(flight.To, decimal.MaxValue))
                {
                    // Actualiza la distancia
                    distances[flight.To] = newDistance;
                    // Registra el predecesor
                    previous[flight.To] = currentAirport;
                    
                    // Añade el elemento a la cola. Si ya existía, el SortedSet lo inserta correctamente.
                    priorityQueue.Add((newDistance, flight.To));
                }
            }
        }

        return (distances, previous);
    }

    // Reconstrucción de la ruta a partir de los predecesores
    public static List<string>? GetRoute(Dictionary<string, string> previous, string start, string end)
    {
        start = start.ToUpperInvariant();
        end = end.ToUpperInvariant();

        List<string> route = new List<string>();
        string current = end;

        // Si el destino no tiene un predecesor, es inalcanzable.
        if (!previous.ContainsKey(end) && start != end)
        {
            return null;
        }
        
        // Manejar caso de inicio = fin
        if (start == end)
        {
            // Solo se llega si ya se tiene el aeropuerto en el grafo o se agregó en Dijkstra.
            if (previous.ContainsKey(end) || previous.Count == 0 && current == start)
            {
                return new List<string> { start };
            }
        }

        // Recorre hacia atrás desde el final hasta el inicio
        while (current != start)
        {
            route.Add(current);
            if (!previous.ContainsKey(current))
            {
                return null; // No hay ruta
            }
            current = previous[current];
        }

        route.Add(start);
        route.Reverse(); // Invertir para tener el orden correcto (inicio -> fin)
        return route;
    }
}

public static class Reporter
{
    public static void ShowDatabase(Graph graph)
    {
        Console.WriteLine("\n--- Base de Datos de Vuelos ---");
        // Asegura que todos los aeropuertos, incluso los que solo son destino, aparezcan.
        var allAirports = graph.GetAllAirports().OrderBy(a => a);

        foreach (var airport in allAirports.Distinct())
        {
            var flights = graph.GetFlightsFrom(airport);
            if (flights.Any())
            {
                Console.WriteLine($"\n Aeropuerto de Origen: {airport}");
                foreach (var flight in flights.OrderBy(f => f.To))
                {
                    Console.WriteLine($"  -> Destino: {flight.To} | Costo: ${flight.Cost} | Salida: {flight.DepartureTime}");
                }
            }
        }
        if (!allAirports.Any())
        {
            Console.WriteLine("La base de datos de vuelos está vacía.");
        }
        Console.WriteLine("------------------------------");
    }

    public static void ShowCheapestRoute(Graph graph, string start, string end)
    {
        start = start.ToUpperInvariant();
        end = end.ToUpperInvariant();
        
        (Dictionary<string, decimal> distancesResult, Dictionary<string, string> previous) = DijkstraAlgorithm.FindCheapestRoute(graph, start, end);
        List<string>? route = DijkstraAlgorithm.GetRoute(previous, start, end);

        Console.WriteLine($"\n--- Búsqueda de Ruta más Barata: {start} -> {end} ---");

        if (route != null && distancesResult.ContainsKey(end) && distancesResult[end] != decimal.MaxValue)
        {
            Console.WriteLine($" ¡Ruta Encontrada! Ruta más barata de {start} a {end}:");
            Console.WriteLine(string.Join(" -> ", route));
            Console.WriteLine($" Costo total: ${distancesResult[end]}");
            
            // Opcional: Mostrar los detalles de los vuelos
            Console.WriteLine("Detalle de los Vuelos:");
            for (int i = 0; i < route.Count - 1; i++)
            {
                string currentAirport = route[i];
                string nextAirport = route[i+1];
                var flight = graph.GetFlightsFrom(currentAirport)
                                  .FirstOrDefault(f => f.To == nextAirport);
                
                if (flight != null)
                {
                    Console.WriteLine($"  - Vuelo de {currentAirport} a {nextAirport} | Costo: ${flight.Cost} | Salida: {flight.DepartureTime}");
                }
            }
        }
        else
        {
            Console.WriteLine($" No se encontró una ruta de {start} a {end}.");
        }
        Console.WriteLine("-------------------------------------------------");
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Título Principal
        Console.WriteLine("\n*****************************************");
        Console.WriteLine("UNIVERSIDAD ESTATAL AMAZÓNICA");
        Console.WriteLine("APLICACIÓN DE GESTIÓN DE VUELOS");
        Console.WriteLine("*****************************************");

        Console.WriteLine("\n Iniciando aplicación de gestión de vuelos...");
        
        // Crear base de datos
        Graph graph = Database.CreateSampleDatabase();
        Console.WriteLine(" Creando base de datos de vuelos de ejemplo (Ecuador)...");

        // Mostrar base de datos inicial
        Reporter.ShowDatabase(graph);
        
        // --- Interacción con el Usuario para Agregar Vuelo ---
        Console.Write("¿Desea agregar un vuelo manualmente? (s/n): ");
        string? respuesta = Console.ReadLine();
        if (respuesta != null && respuesta.ToLower() == "s")
        {
            graph.AddFlightFromConsole();
            Reporter.ShowDatabase(graph); // Muestra la base de datos actualizada
        }

        // --- Búsqueda de Ruta ---
        Console.WriteLine("\n--- Consulta de Ruta ---");
        Console.Write("Ingrese el aeropuerto de ORIGEN para la búsqueda (e.g., GYE): ");
        string? startAirport = Console.ReadLine();
        startAirport = startAirport?.ToUpperInvariant() ?? "GYE";

        Console.Write("Ingrese el aeropuerto de DESTINO para la búsqueda (e.g., MDE): ");
        string? endAirport = Console.ReadLine();
        endAirport = endAirport?.ToUpperInvariant() ?? "MDE";

        // Buscar y mostrar la ruta más barata
        Reporter.ShowCheapestRoute(graph, startAirport, endAirport);
        
        Console.WriteLine("\nPrograma finalizado. ¡Gracias por usar el gestor de vuelos!");
    }
}