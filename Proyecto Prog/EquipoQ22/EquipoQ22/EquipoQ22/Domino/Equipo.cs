using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipoQ22.Domino
{
    public class Equipo
    {
        public string pais { get; set; }
        public string directorTecnico { get; set; }

        public List<Jugador> listaJugadores { get; set; }

        public Equipo()
        {
            listaJugadores = new List<Jugador>();
        }

        public void AgregarJugador(Jugador jugador)
        {
            listaJugadores.Add(jugador);
        }

        public void QuitarJugador(int indice)
        {
            listaJugadores.RemoveAt(indice);
        }
    }
}
