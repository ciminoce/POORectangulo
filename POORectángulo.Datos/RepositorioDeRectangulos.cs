using POORectángulo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POORectángulo.Datos
{
    public class RepositorioDeRectangulos
    {
        private List<Rectangulo> listaRectangulo;
        private bool hayCambios = false;
        public RepositorioDeRectangulos()
        {
            listaRectangulo = new List<Rectangulo>();
            listaRectangulo = ManejadorArchivosSecuencial.LeerArchivoSecuencial();
        }
        public void Agregar(Rectangulo rectangulo)
        {
            hayCambios = true;
            listaRectangulo.Add(rectangulo);
        }
        public List<Rectangulo> GetLista()
        {
            return listaRectangulo;
        }
        public int GatCantidad()
        {
            return listaRectangulo.Count();
        }

        public bool Borrar(Rectangulo rect)
        {
            if (listaRectangulo.Contains(rect))
            {
                hayCambios = true;
                listaRectangulo.Remove(rect);
                return true;
            }
            return false;
        }

        public void Editar(Rectangulo rectangulo)
        {
            hayCambios = true;
        }
        public List<Rectangulo> FiltrarDatos(int areaFiltro)
        {
            return listaRectangulo.Where(c => c.GetArea() > areaFiltro).ToList();
                        
        }

        public int GatCantidadFiltrada(int areaFiltro)
        {
            return listaRectangulo.Count(c => c.GetArea() > areaFiltro);
        }

        public void Guardar()
        {
            if (hayCambios)
            {
                ManejadorArchivosSecuencial.GuardarEnArchivosSecuencial(listaRectangulo);
            }
        }

        public List<Rectangulo> Ordenar(Orden orden)
        {
            switch (orden)
            {
                case Orden.Ascendente:
                    return listaRectangulo.OrderBy(r => r.GetArea()).ToList();
                    break;
                case Orden.Descendente:
                    return listaRectangulo.OrderByDescending(r => r.GetArea()).ToList();

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(orden), orden, null);
            }
        }
    }
}
