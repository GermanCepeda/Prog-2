using EquipoQ22.Domino;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipoQ22.Datos.Implementaciones
{
    internal class EquipoDAO : IEquipoDao
    {
        public bool CrearEquipo(Equipo equipo)
        {
            return HelperDAO.ObtenerInstancia().ejecutar("SP_INSERTAR_EQUIPO", "SP_INSERTAR_DETALLES_EQUIPO", equipo);
        }

        public DataTable ObtenerPersonas()
        {
            return HelperDAO.ObtenerInstancia().consulta("SP_CONSULTAR_PERSONAS");
        }
    }
}
