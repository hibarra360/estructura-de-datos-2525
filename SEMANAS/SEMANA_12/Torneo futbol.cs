
// Clase para representar a un jugador
public class Jugador
{
    public string Nombre { get; set; }
    public int NumeroCamiseta { get; set; }

    public Jugador(string nombre, int numeroCamiseta)
    {
        Nombre = nombre;
        NumeroCamiseta = numeroCamiseta;
    }

    // Sobrescribir Equals y GetHashCode para que el HashSet funcione correctamente
    public override bool Equals(object? obj)
    {
        return obj is Jugador jugador &&
               Nombre == jugador.Nombre &&
               NumeroCamiseta == jugador.NumeroCamiseta;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Nombre, NumeroCamiseta);
    }
}

// Clase para representar a un equipo
public class Equipo
{
    public string Nombre { get; set; }
    public HashSet<Jugador> Jugadores { get; set; }

    public Equipo(string nombre)
    {
        Nombre = nombre;
        Jugadores = new HashSet<Jugador>();
    }
}

// Clase principal para la gestión del torneo
public class TorneoFutbol
{
    // Diccionario para almacenar los equipos del torneo
    private Dictionary<string, Equipo> equipos;

    public TorneoFutbol()
    {
        equipos = new Dictionary<string, Equipo>();
    }

    // Método para registrar un equipo
    public void RegistrarEquipo(string nombreEquipo)
    {
        if (!equipos.ContainsKey(nombreEquipo))
        {
            equipos.Add(nombreEquipo, new Equipo(nombreEquipo));
            Console.WriteLine($"Equipo '{nombreEquipo}' registrado con éxito.");
        }
        else
        {
            Console.WriteLine($"El equipo '{nombreEquipo}' ya existe.");
        }
    }

    // Método para agregar un jugador a un equipo
    public void AgregarJugadorAEquipo(string nombreEquipo, string nombreJugador, int numeroCamiseta)
    {
        if (equipos.TryGetValue(nombreEquipo, out var equipo))
        {
            var nuevoJugador = new Jugador(nombreJugador, numeroCamiseta);
            if (equipo.Jugadores.Add(nuevoJugador))
            {
                Console.WriteLine($"Jugador '{nombreJugador}' agregado a '{nombreEquipo}'.");
            }
            else
            {
                Console.WriteLine($"El jugador '{nombreJugador}' ya está en el equipo '{nombreEquipo}'.");
            }
        }
        else
        {
            Console.WriteLine($"El equipo '{nombreEquipo}' no se encontró.");
        }
    }

    // Reportería: Visualizar todos los equipos y sus jugadores
    public void VisualizarTorneo()
    {
        Console.WriteLine("\n--- Reporte del Torneo ---");
        if (equipos.Count == 0)
        {
            Console.WriteLine("No hay equipos registrados.");
            return;
        }

        foreach (var par in equipos)
        {
            var equipo = par.Value;
            Console.WriteLine($"\nEQUIPO: {equipo.Nombre}");
            if (equipo.Jugadores.Count == 0)
            {
                Console.WriteLine("  No tiene jugadores registrados.");
                continue;
            }
            Console.WriteLine("  JUGADORES:");
            foreach (var jugador in equipo.Jugadores)
            {
                Console.WriteLine($"    - Nombre: {jugador.Nombre}, Número: {jugador.NumeroCamiseta}");
            }
        }
        Console.WriteLine("\n--------------------------");
    }

    // Reportería: Consultar un equipo específico
    public void ConsultarEquipo(string nombreEquipo)
    {
        Console.WriteLine($"\n--- Consulta del Equipo '{nombreEquipo}' ---");
        if (equipos.TryGetValue(nombreEquipo, out var equipo))
        {
            Console.WriteLine($"EQUIPO: {equipo.Nombre}");
            Console.WriteLine($"Número de jugadores: {equipo.Jugadores.Count}");
            if (equipo.Jugadores.Count > 0)
            {
                Console.WriteLine("JUGADORES:");
                foreach (var jugador in equipo.Jugadores)
                {
                    Console.WriteLine($"  - {jugador.Nombre} ({jugador.NumeroCamiseta})");
                }
            }
        }
        else
        {
            Console.WriteLine($"El equipo '{nombreEquipo}' no se encontró.");
        }
    }

    // Ejemplo de uso
    public static void Main(string[] args)
    {
        TorneoFutbol miTorneo = new TorneoFutbol();

        // Registrar equipos
        miTorneo.RegistrarEquipo("Los Leones");
        miTorneo.RegistrarEquipo("Las Águilas");

        // Agregar jugadores
        miTorneo.AgregarJugadorAEquipo("Los Leones", "Carlos", 10);
        miTorneo.AgregarJugadorAEquipo("Los Leones", "Pedro", 7);
        miTorneo.AgregarJugadorAEquipo("Los Leones", "Carlos", 10); // Intento de duplicado
        miTorneo.AgregarJugadorAEquipo("Las Águilas", "Ana", 5);

        // Visualizar el estado del torneo
        miTorneo.VisualizarTorneo();

        // Consultar un equipo específico
        miTorneo.ConsultarEquipo("Los Leones");
        miTorneo.ConsultarEquipo("Los Halcones");
    }
}