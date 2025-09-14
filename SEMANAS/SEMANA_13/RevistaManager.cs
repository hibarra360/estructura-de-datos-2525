// RevistaManager.cs

public class RevistaManager
{
    // Usamos una lista para almacenar los títulos de las revistas.
    private List<string> catalogo;

    public RevistaManager()
    {
        catalogo = new List<string>();
        InicializarCatalogo();
    }

    // Inicializa el catálogo con al menos 10 títulos.
    private void InicializarCatalogo()
    {
        catalogo.Add("National Geographic");
        catalogo.Add("Time Magazine");
        catalogo.Add("The Economist");
        catalogo.Add("Forbes");
        catalogo.Add("Vogue");
        catalogo.Add("Scientific American");
        catalogo.Add("PC Gamer");
        catalogo.Add("Wired");
        catalogo.Add("Architectural Digest");
        catalogo.Add("Car and Driver");
    }

    // Método para buscar un título en el catálogo de forma iterativa.
    public bool BuscarPorTitulo(string tituloBuscado)
    {
        string tituloNormalizado = tituloBuscado.ToLower();

        foreach (string titulo in catalogo)
        {
            if (titulo.ToLower() == tituloNormalizado)
            {
                return true;
            }
        }
        return false;
    }

    // Método para buscar una revista por su número de índice.
    public string BuscarPorIndice(int indice)
    {
        // El índice de la lista es base 0, por lo que el primer elemento es el 0.
        // Si el usuario ingresa 1, se busca en la posición 0.
        int indiceDeLista = indice - 1;

        if (indiceDeLista >= 0 && indiceDeLista < catalogo.Count)
        {
            return catalogo[indiceDeLista];
        }
        else
        {
            return string.Empty; // Retorna cadena vacía si el índice no es válido.
        }
    }

    // Método para mostrar el catálogo completo con sus números.
    public void MostrarCatalogo()
    {
        Console.WriteLine("\n--- Catálogo de Revistas ---");
        for (int i = 0; i < catalogo.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {catalogo[i]}");
        }
    }
}

// Program.cs
public class Program
{
    public static void Main(string[] args)
    {
        RevistaManager manager = new RevistaManager();
        string opcion;

        do
        {
            MostrarMenu();
            opcion = Console.ReadLine() ?? string.Empty;

            switch (opcion)
            {
                case "1":
                    manager.MostrarCatalogo();
                    break;
                case "2":
                    Console.WriteLine("\nIngrese el título de la revista o el número de su posición:");
                    string busqueda = Console.ReadLine() ?? string.Empty;
                    bool encontrado = false;
                    string resultado = "";

                    // Intentamos convertir la entrada a un número.
                    if (int.TryParse(busqueda, out int numero))
                    {
                        string tituloEncontrado = manager.BuscarPorIndice(numero);
                        if (tituloEncontrado != null)
                        {
                            encontrado = true;
                            resultado = tituloEncontrado;
                        }
                    }
                    else
                    {
                        if (manager.BuscarPorTitulo(busqueda))
                        {
                            encontrado = true;
                            resultado = busqueda;
                        }
                    }

                    if (encontrado)
                    {
                        Console.WriteLine($"\n¡Encontrado! La revista '{resultado}' se encuentra en el catálogo.");
                    }
                    else
                    {
                        Console.WriteLine($"\nNo encontrado. La entrada '{busqueda}' no coincide con ninguna revista en el catálogo.");
                    }
                    break;
                case "3":
                    Console.WriteLine("\nSaliendo del programa...");
                    break;
                default:
                    Console.WriteLine("\nOpción no válida. Por favor, intente de nuevo.");
                    break;
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
            Console.Clear();

        } while (opcion != "3");
    }

    // Muestra el menú de opciones al usuario.
    private static void MostrarMenu()
    {
        Console.WriteLine("--- Catálogo de Revistas ---");
        Console.WriteLine("1. Ver el catálogo completo");
        Console.WriteLine("2. Buscar un título o número de revista");
        Console.WriteLine("3. Salir");
        Console.Write("Seleccione una opción: ");
    }
}