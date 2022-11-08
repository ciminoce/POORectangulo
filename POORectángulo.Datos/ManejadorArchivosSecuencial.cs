using POORectángulo.Entidades;
using System.Collections.Generic;
using System.IO;

namespace POORectángulo.Datos
{
    public static class ManejadorArchivosSecuencial
    {
        private static string archivo = "Rectangulos.txt";
        public static void GuardarEnArchivosSecuencial(List<Rectangulo> lista)
        {
            using (var escritor=new StreamWriter(archivo))
            {
                foreach (var rectangulo in lista)
                {
                    string linea = ConstruirLinea(rectangulo);
                    escritor.WriteLine(linea);
                }
            }
        }

        private static string ConstruirLinea(Rectangulo rectangulo)
        {
            return $"{rectangulo.Alto}|{rectangulo.Largo}";
        }
        public static List<Rectangulo> LeerArchivoSecuencial()
        {
            List<Rectangulo> lista = new List<Rectangulo>();
            if (File.Exists(archivo))
            {
                using (var lector = new StreamReader(archivo))
                {
                    while (!lector.EndOfStream)
                    {
                        string linea = lector.ReadLine();
                        Rectangulo rectangulo = ConstuirRectangulo(linea);
                        lista.Add(rectangulo);
                    }
                }

            }
            return lista;
        }

        private static Rectangulo ConstuirRectangulo(string linea)
        {
            var campos = linea.Split('|');
            Rectangulo rectangulo = new Rectangulo()
            {
                Alto = int.Parse(campos[0]),
                Largo = int.Parse(campos[1])
            };
            return rectangulo;

        }
    }
}
