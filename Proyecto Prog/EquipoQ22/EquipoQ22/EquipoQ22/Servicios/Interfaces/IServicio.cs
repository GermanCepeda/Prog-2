using EquipoQ22.Datos;
using EquipoQ22.Domino;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipoQ22.Servicios.Interfaces
{
    internal interface IServicio
    {
        DataTable ObtenerPersonas();
        bool CrearEquipo(Equipo equipo);
    }
}
