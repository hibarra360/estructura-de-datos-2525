using System;
using System.Collections.Generic;
using System.Diagnostics;

// Clase que representa una persona
public class Persona
{
    public string Nombre { get; set; }
    public int Id { get; set; }

    public Persona(string nombre, int id)
    {
        Nombre = nombre;
        Id = id;
    }
}

// Clase que gestiona la atracción y la cola
public class Atraccion
{
    private Queue<Persona> colaEspera;
    private List<Persona> asientosOcupados;
    private int capacidadMaxima;
    private int asientosDisponibles;

    public Atraccion(int capacidad)
    {
        colaEspera = new Queue<Persona>();
        asientosOcupados = new List<Persona>();
        capacidadMaxima = capacidad;
        asientosDisponibles = capacidad;
    }

    // Agrega una persona a la cola
    public void Encolar(Persona persona)
    {
        if (asientosDisponibles > 0 || colaEspera.Count < capacidadMaxima)
        {
            colaEspera.Enqueue(persona);
            Console.WriteLine($"{persona.Nombre} se ha unido a la cola.");
        }
        else
        {
            Console.WriteLine("¡Todos los asientos están ocupados y la cola está llena! No se puede agregar más personas.");
        }
    }

    // Asigna un asiento a la siguiente persona en la cola
    public void AsignarAsiento()
    {
        if (colaEspera.Count > 0 && asientosDisponibles > 0)
        {
            Persona persona = colaEspera.Dequeue();
            asientosOcupados.Add(persona);
            asientosDisponibles--;
            Console.WriteLine($"{persona.Nombre} ha tomado el asiento {capacidadMaxima - asientosDisponibles}.");
        }
        else if (asientosDisponibles == 0)
        {
            Console.WriteLine("No hay asientos disponibles.");
        }
        else
        {
            Console.WriteLine("No hay personas en la cola.");
        }
    }

    // Muestra las personas en espera
    public void MostrarCola()
    {
        Console.WriteLine("\n--- Personas en cola ---");
        if (colaEspera.Count == 0)
        {
            Console.WriteLine("La cola está vacía.");
            return;
            
        }
        
        foreach (var persona in colaEspera)
        {
            Console.WriteLine($"ID: {persona.Id}, Nombre: {persona.Nombre}");
        }
    }

    // Muestra los asientos ocupados y devuelve la cantidad
    public int MostrarAsientos()
    {
        Console.WriteLine("\n--- Asientos ocupados ---");
        if (asientosOcupados.Count == 0)
        {
            Console.WriteLine("No hay asientos ocupados.");
            return 0;
        }
        
        for (int i = 0; i < asientosOcupados.Count; i++)
        {
            Console.WriteLine($"Asiento {i + 1}: {asientosOcupados[i].Nombre}");
        }
        return asientosOcupados.Count;
    }

    public int AsientosOcupadosCount => asientosOcupados.Count;
}

class Program
{
    static void Main(string[] args)
    {
        Atraccion atraccion = new Atraccion(30); // Capacidad de 30 asientos

        // Simulación: Agregar 35 personas (5 se quedarán sin asiento)
        for (int i = 1; i <= 35; i++)
        {
            atraccion.Encolar(new Persona($"Persona {i}", i));
        }

        // Asignar asientos hasta agotarlos
        while (true)
        {
            Console.Clear(); // Limpiar pantalla para mejor visualización
            
            atraccion.AsignarAsiento();
            atraccion.MostrarCola();
            int asientosOcupados = atraccion.MostrarAsientos();

            if (asientosOcupados == 30)
            {
                Console.WriteLine("\n¡Todos los asientos están ocupados!");
                break;
            }

            Console.WriteLine("\nPresione Enter para continuar...");
            Console.ReadLine();
        }

        // Análisis de tiempo de ejecución
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        // Operación de encolar y asignar asientos (ejemplo adicional)
        atraccion.Encolar(new Persona("Persona Extra", 36)); // No se agregará
        atraccion.AsignarAsiento(); // No hay asientos

        stopwatch.Stop();
        Console.WriteLine($"\nTiempo de ejecución de operaciones: {stopwatch.ElapsedMilliseconds} ms");
    }
}