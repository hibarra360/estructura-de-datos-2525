
// 1. Clases que representan a las personas y las vacunas
// Definimos un 'Ciudadano' con un ID único y un nombre.
public class Ciudadano
{
    public int Id { get; set; }
    public string Nombre { get; set; }

    public Ciudadano(int id, string nombre)
    {
        Id = id;
        Nombre = nombre;
    }

    // Es VITAL para los conjuntos (HashSet) que definamos
    // cómo dos ciudadanos son 'iguales'. Aquí, dos ciudadanos
    // son iguales si tienen el mismo ID.
    public override bool Equals(object obj)
    {
        // Si el objeto no es un Ciudadano, no son iguales.
        if (obj is not Ciudadano otro) return false;
        // Comparamos los IDs para ver si son la misma persona.
        return Id == otro.Id;
    }

    // También debemos definir un 'código hash' basado en el ID,
    // que es un número único para cada objeto. Esto ayuda a los
    // conjuntos a encontrar objetos de forma rápida.
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}

// 2. Programa Principal: Simulación de Vacunación
public class ProgramaVacunacion
{
    public static void Main()
    {
        Console.WriteLine("--- SIMULACIÓN DE CAMPAÑA DE VACUNACIÓN ---");

        // Creamos una población de 500 ciudadanos.
        var poblacionTotal = new HashSet<Ciudadano>();
        for (int i = 1; i <= 500; i++)
        {
            poblacionTotal.Add(new Ciudadano(i, $"Ciudadano_{i}"));
        }
        Console.WriteLine($"\nTotal de la población: {poblacionTotal.Count} ciudadanos.");

        // Simulamos la vacunación con dos tipos de vacunas.
        // Usamos HashSet para manejar los grupos de forma eficiente.
        var random = new Random();
        
        // a) Grupo de ciudadanos vacunados con Pfizer (75 personas).
        var vacunadosPfizer = poblacionTotal.OrderBy(c => random.Next()).Take(75).ToHashSet();
        Console.WriteLine($"Vacunados con Pfizer: {vacunadosPfizer.Count}");
        
        // b) Grupo de ciudadanos vacunados con AstraZeneca (75 personas).
        // Queremos que 25 de ellos también estén en el grupo de Pfizer.
        var ciudadanosConAmbasDosis = vacunadosPfizer.OrderBy(c => random.Next()).Take(25);
        var ciudadanosSoloAstraZeneca = poblacionTotal.Except(vacunadosPfizer).OrderBy(c => random.Next()).Take(50);
        
        var vacunadosAstraZeneca = ciudadanosConAmbasDosis.Concat(ciudadanosSoloAstraZeneca).ToHashSet();
        Console.WriteLine($"Vacunados con AstraZeneca: {vacunadosAstraZeneca.Count}\n");

        Console.WriteLine("--- APLICANDO OPERACIONES DE TEORÍA DE CONJUNTOS ---");
        
        // Operación 1: Unión (juntar ambos grupos de vacunados)
        var todosLosVacunados = new HashSet<Ciudadano>(vacunadosPfizer);
        todosLosVacunados.UnionWith(vacunadosAstraZeneca);
        
        // Operación 2: Diferencia (quienes no están en el grupo de vacunados)
        var noVacunados = poblacionTotal.Except(todosLosVacunados).ToList();
        Console.WriteLine($"\nCiudadanos que NO se han vacunado: {noVacunados.Count}");
        
        // Operación 3: Intersección (quienes están en ambos grupos)
        var conAmbasDosis = vacunadosPfizer.Intersect(vacunadosAstraZeneca).ToList();
        Console.WriteLine($"Ciudadanos que recibieron AMBAS dosis: {conAmbasDosis.Count}");
        
        // Operación 4: Diferencia Simétrica (quienes solo están en un grupo)
        var soloPfizer = vacunadosPfizer.Except(vacunadosAstraZeneca).ToList();
        Console.WriteLine($"Ciudadanos que solo se vacunaron con PFIZER: {soloPfizer.Count}");
        
        var soloAstraZeneca = vacunadosAstraZeneca.Except(vacunadosPfizer).ToList();
        Console.WriteLine($"Ciudadanos que solo se vacunaron con ASTRAZENECA: {soloAstraZeneca.Count}");
    }
}