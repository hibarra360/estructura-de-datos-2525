

namespace Traductor
{
    class Program
    {
        // Diccionario para traducciones de inglés a español.
        private static Dictionary<string, string> inglesAEspanol;
        // Diccionario para traducciones de español a inglés.
        private static Dictionary<string, string> espanolAIngles;

        // Se inicializan los diccionarios con palabras base.
        static Program()
        {
            inglesAEspanol = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "time", "tiempo" },
                { "year", "año" },
                { "people", "gente / personas" },
                { "way", "camino / forma" },
                { "day", "día" },
                { "thing", "cosa" },
                { "man", "hombre" },
                { "world", "mundo" },
                { "life", "vida" },
                { "hand", "mano" },
                { "part", "parte" },
                { "child", "niño/a" },
                { "eye", "ojo" },
                { "woman", "mujer" },
                { "place", "lugar" },
                { "work", "trabajo" },
                { "week", "semana" },
                { "case", "caso" },
                { "point", "punto / tema" },
                { "government", "gobierno" },
                { "company", "empresa / compañía" },
            };

            espanolAIngles = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "tiempo", "time" },
                { "año", "year" },
                { "gente / personas", "people" },
                { "camino / forma", "way" },
                { "día", "day" },
                { "cosa", "thing" },
                { "hombre", "man" },
                { "mundo", "world" },
                { "vida", "life" },
                { "mano", "hand" },
                { "parte", "part" },
                { "niño/a", "child" },
                { "ojo", "eye" },
                { "mujer", "woman" },
                { "lugar", "place" },
                { "trabajo", "work" },
                { "semana", "week" },
                { "caso", "case" },
                { "punto / tema", "point" },
                { "gobierno", "government" },
                { "empresa / compañía", "company" },
            };
        }

        static void Main(string[] args)
        {
            int opcion = -1;
            while (opcion != 0)
            {
                Console.WriteLine("\n==================== MENÚ ====================");
                Console.WriteLine("1. Traducir de Inglés a Español");
                Console.WriteLine("2. Traducir de Español a Inglés");
                Console.WriteLine("3. Agregar una nueva palabra");
                Console.WriteLine("0. Salir");
                Console.Write("\nSeleccione una opción: ");
                
                if (int.TryParse(Console.ReadLine(), out opcion))
                {
                    switch (opcion)
                    {
                        case 1:
                            Console.Write("Ingrese la frase en inglés: ");
                            string fraseIngles = Console.ReadLine() ?? string.Empty;
                            string traduccionEspanol = TraducirFrase(fraseIngles, inglesAEspanol);
                            Console.WriteLine($"\nTraducción: {traduccionEspanol}");
                            break;
                        case 2:
                            Console.Write("Ingrese la frase en español: ");
                            string fraseEspanol = Console.ReadLine() ?? string.Empty;
                            string traduccionIngles = TraducirFrase(fraseEspanol, espanolAIngles);
                            Console.WriteLine($"\nTraducción: {traduccionIngles}");
                            break;
                        case 3:
                            AgregarPalabra();
                            break;
                        case 0:
                            Console.WriteLine("Saliendo del programa...");
                            break;
                        default:
                            Console.WriteLine("Opción no válida. Por favor, intente de nuevo.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Entrada no válida. Por favor, ingrese un número.");
                }
            }
        }

        // Método genérico para traducir una frase usando un diccionario dado.
        private static string TraducirFrase(string frase, Dictionary<string, string> diccionario)
        {
            string[] palabras = frase.Split(new char[] { ' ', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> fraseTraducida = new List<string>();

            foreach (string palabra in palabras)
            {
                if (diccionario.ContainsKey(palabra))
                {
                    fraseTraducida.Add(diccionario[palabra]);
                }
                else
                {
                    fraseTraducida.Add(palabra);
                }
            }
            return string.Join(" ", fraseTraducida);
        }

        // Método para agregar una nueva palabra en ambos diccionarios.
        private static void AgregarPalabra()
        {
            Console.Write("Ingrese la palabra en inglés: ");
            string palabraIngles = Console.ReadLine() ?? string.Empty;
            Console.Write("Ingrese la traducción en español: ");
            string palabraEspanol = Console.ReadLine() ?? string.Empty;

            if (inglesAEspanol.ContainsKey(palabraIngles) || espanolAIngles.ContainsKey(palabraEspanol))
            {
                Console.WriteLine($"\nLa palabra '{palabraIngles}' o '{palabraEspanol}' ya existe en el diccionario.");
            }
            else
            {
                inglesAEspanol.Add(palabraIngles, palabraEspanol);
                espanolAIngles.Add(palabraEspanol, palabraIngles);
                Console.WriteLine($"\nLa palabra '{palabraIngles}' ha sido agregada con su traducción '{palabraEspanol}'.");
            }
        }
    }
}