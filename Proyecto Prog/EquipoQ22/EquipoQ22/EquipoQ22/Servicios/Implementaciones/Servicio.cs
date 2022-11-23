using EquipoQ22.Datos;
using EquipoQ22.Datos.Implementaciones;
using EquipoQ22.Domino;
using EquipoQ22.Servicios.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipoQ22.Servicios.Implementaciones
{
    internal class Servicio : IServicio
    {

        private IEquipoDao oDAO;

        public Servicio()
        {
            oDAO = new EquipoDAO();
        }
        public bool CrearEquipo(Equipo equipo)
        {
            return oDAO.CrearEquipo(equipo);
        }

        public DataTable ObtenerPersonas()
        {
            return oDAO.ObtenerPersonas();
        }
    }
}
