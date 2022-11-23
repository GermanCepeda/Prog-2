using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using EquipoQ22.Domino;

namespace EquipoQ22.Datos
{
    internal class HelperDAO
    {
        private static HelperDAO instancia;
        private SqlConnection conexion;
        private SqlCommand comando = new SqlCommand();


        private HelperDAO()
        {
            conexion = new SqlConnection(Properties.Resources.BDLocal); //SE CAMBIO BD
        }

        public static HelperDAO ObtenerInstancia()
        {
            if(instancia == null)
            {
                instancia = new HelperDAO();
            }
            return instancia;
        }

        private void conectar()
        {
            conexion.Open();
            comando.Connection = conexion;
            comando.CommandType = CommandType.StoredProcedure;
        }

        private void desconectar()
        {
            conexion.Close();
        }

        public DataTable consulta(string nombreSP)
        {
            DataTable tabla = new DataTable();
            conectar();

            comando.CommandText = nombreSP;
            tabla.Load(comando.ExecuteReader());

            desconectar();
            return tabla;
        }

        public bool ejecutar(string spMaestro, string spDetalle, Equipo equipo)
        {
            bool control = true;
            SqlTransaction t = null;

            try
            {
                conectar();
                t = conexion.BeginTransaction();
                comando.Transaction = t;

                // MAESTRO
                comando.Parameters.Clear();
                comando.CommandText = spMaestro;
                comando.Parameters.AddWithValue("@pais", equipo.pais);
                comando.Parameters.AddWithValue("@director_tecnico", equipo.directorTecnico);
                SqlParameter pOut = new SqlParameter("@id", SqlDbType.Int);
                pOut.Direction = ParameterDirection.Output;
                comando.Parameters.Add(pOut);

                comando.ExecuteNonQuery();
                int id = (int)pOut.Value;
                comando.Parameters.Clear();

                // DETALLE


                foreach(Jugador jugador in equipo.listaJugadores)
                {
                    comando.CommandText = spDetalle;

                    comando.Parameters.AddWithValue("@id_equipo",id);
                    comando.Parameters.AddWithValue("@id_persona", jugador.Persona.IdPersona);
                    comando.Parameters.AddWithValue("@camiseta", jugador.Camiseta);
                    comando.Parameters.AddWithValue("@posicion", jugador.Posicion);

                    comando.ExecuteNonQuery();
                    comando.Parameters.Clear();
                }
                t.Commit();

                control = true;
            }
            catch (Exception)
            {
                if(t != null)
                {
                    t.Rollback();
                }
                control = false;
            }
            finally
            {
                desconectar();
            }
            return control;
        }
    }
}
