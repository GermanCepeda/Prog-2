using EquipoQ22.Domino;
using EquipoQ22.Servicios.Implementaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//COMPLETAR --> Curso:1W4      Legajo: 113829        Apellido y Nombre: Freccia, Genaro Farid

//CadenaDeConexion: "Data Source=sqlgabineteinformatico.frc.utn.edu.ar;Initial Catalog=Qatar2022;User ID=alumnoprog22;Password=SQL+Alu22"

namespace EquipoQ22
{
    public partial class FrmAlta : Form
    {
        private Servicio servicio;
        private Equipo equipo;
        public FrmAlta()
        {
            InitializeComponent();
            servicio = new Servicio();
            equipo = new Equipo();
        }
        private void FrmAlta_Load(object sender, EventArgs e)
        {
            cargarPersonas();
            cantidadJugadores();
        }

        private void cargarPersonas()
        {
            cboPersona.DataSource = servicio.ObtenerPersonas();
            cboPersona.DisplayMember = "nombre_completo";
            cboPersona.ValueMember = "id_persona";
            cboPersona.SelectedIndex = -1;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if(cboPersona.SelectedIndex != -1 && cboPosicion.SelectedIndex != -1)
            {
                if (!existe())
                {
                    Jugador jugador = new Jugador();
                    jugador.Persona = new Persona((int)cboPersona.SelectedValue, cboPersona.Text);
                    jugador.Posicion = cboPosicion.Text;
                    jugador.Camiseta = (int)nudCamiseta.Value;
                    equipo.AgregarJugador(jugador);

                    dgvDetalles.Rows.Add(new object[] { jugador.Persona.IdPersona, jugador.Persona.NombreCompleto, jugador.Camiseta, jugador.Posicion });
                    cantidadJugadores();
                    nudCamiseta.Value = 1;
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar posicion y persona.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private bool existe()
        {
            bool existe = false;

            foreach(DataGridViewRow j in dgvDetalles.Rows)
            {
                if (j.Cells["jugador"].Value.ToString() == cboPersona.Text)
                {
                    MessageBox.Show("El jugador no puede ocupar 2 posiciones", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    existe = true;
                    return existe;
                }
                if (j.Cells["camiseta"].Value.ToString() == nudCamiseta.Value.ToString())
                {
                    MessageBox.Show("La camiseta ya esta en uso.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    existe = true;
                    return existe;
                }
            }
            return existe;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (validar())
            {
                equipo.pais = txtPais.Text;
                equipo.directorTecnico = txtDT.Text;

                if (servicio.CrearEquipo(equipo))
                {
                    MessageBox.Show("El equipo se guardo con exito", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    LimpiarCampos();
                    return;
                }
                else
                {
                    MessageBox.Show("El equipo NO se pudo guardar.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }
            }
        }

        private void LimpiarCampos()
        {
            txtPais.Text = "";
            txtDT.Text = "";
            cboPersona.SelectedIndex = -1;
            cboPosicion.SelectedIndex = -1;
            nudCamiseta.Value = 1;
            dgvDetalles.Rows.Clear();
            cantidadJugadores();
            equipo.listaJugadores.Clear();
        }

        private void cantidadJugadores()
        {
            lblTotal.Text = "Total de Jugadores: " + dgvDetalles.Rows.Count.ToString();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Está seguro que desea cancelar?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Dispose();
            }
        }

        private bool validar()
        {
            if(String.IsNullOrEmpty(txtPais.Text) || String.IsNullOrEmpty(txtDT.Text) || cboPosicion.SelectedIndex == -1)
            {
                MessageBox.Show("Algun campo se encuentra vacio","ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error,MessageBoxDefaultButton.Button1);
                return false;
            }
            if (nudCamiseta.Value < 1 || nudCamiseta.Value > 23)
            {
                MessageBox.Show("La camiseta puede ser del 1 al 23.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return false;
            }
            if(dgvDetalles.Rows.Count < 1)
            {
                MessageBox.Show("Se debe ingresar al menos 1 jugador.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return false;
            }
            return true;
        }

        private void dgvDetalles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 4)
            {
                dgvDetalles.Rows.RemoveAt(e.RowIndex);
                equipo.QuitarJugador(e.RowIndex);
                cantidadJugadores();
            }
        }
    }
}
