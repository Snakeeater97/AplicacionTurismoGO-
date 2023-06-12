using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using System.Globalization;
using static System.Windows.Forms.DataFormats;
using static AplicacionTurismoGO_.Vuelos;

namespace AplicacionTurismoGO_
{
    internal class ModuloVuelos

    {
        //Linea para que se puedan llamar los metodos de esta clase.
        private ModuloPresupuesto moduloPresupuesto; // Agregar una instancia de ModuloPresupuesto
        public ModuloVuelos(MenuVentas form)
        {
            llamada = form;
            moduloPresupuesto = form.moduloPresupuesto; // Asignar el valor de moduloPresupuesto desde el parámetro del constructor
        }

        private ListBox vuelosListBox;

        private MenuVentas formulario;

        public MenuVentas llamada { get; set; }

        private ListBox presupuestoListBox;
        private List<string> presupuestoVuelos = new List<string>();

        public void ConsultarDisponibilidadVuelos()
        {
            llamada.LimpiarFormulario();

            llamada.AgregarEtiqueta("Origen:", 215, 5);
            TextBox origenTextBox = llamada.AgregarTextBox(210, 30);
            llamada.AgregarEtiqueta("Destino:", 215, 55);
            TextBox destinoTextBox = llamada.AgregarTextBox(210, 80);
            llamada.AgregarEtiqueta("Fecha ida:", 215, 110);
            DateTimePicker fechaIdaPicker = llamada.AgregarDateTimePicker(215, 140);
            llamada.AgregarEtiqueta("Fecha vuelta (opcional):", 215, 200);
            DateTimePicker fechaVueltaPicker = llamada.AgregarDateTimePicker(215, 230);
            llamada.AgregarEtiqueta("Adultos:", 500, 30);
            NumericUpDown adultosNumericUpDown = llamada.AgregarNumericUpDown(500, 55);
            llamada.AgregarEtiqueta("Menores:", 500, 85);
            NumericUpDown menoresNumericUpDown = llamada.AgregarNumericUpDown(500, 110);
            llamada.AgregarEtiqueta("Infantes:", 500, 140);
            NumericUpDown infantesNumericUpDown = llamada.AgregarNumericUpDown(500, 165);
            vuelosListBox = new ListBox();
            vuelosListBox.Location = new Point(215, 290);
            vuelosListBox.Size = new Size(1000, 150);
            llamada.Controls.Add(vuelosListBox);

            Button buscarButton = llamada.AgregarBoton("Buscar", 215, 260);
            buscarButton.Click += (sender, e) =>
            {
                string origen = origenTextBox.Text;
                string destino = destinoTextBox.Text;
                DateTime fechaIda = fechaIdaPicker.Value;
                DateTime? fechaVuelta = fechaVueltaPicker.Checked ? fechaVueltaPicker.Value : null;
                int cantidadAdultos = (int)adultosNumericUpDown.Value;
                int cantidadMenores = (int)menoresNumericUpDown.Value;
                int cantidadInfantes = (int)infantesNumericUpDown.Value;

                // Cargar los vuelos desde el archivo JSON
                string rutaArchivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VuelosDisponibles.json");
                List<Vuelo> vuelosDisponibles = new List<Vuelo>();

                try
                {
                    string contenidoJson = File.ReadAllText(rutaArchivo);
                    vuelosDisponibles = JsonConvert.DeserializeObject<List<Vuelo>>(contenidoJson);
                }
                catch (FileNotFoundException ex)
                {
                    MessageBox.Show("No se encontró el archivo 'VuelosDisponibles.json'. Verifica la ubicación y el nombre del archivo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al leer el archivo 'VuelosDisponibles.json'. Detalles del error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Filtrar los vuelos según las fechas de ida y vuelta, origen y destino
                List<Vuelo> vuelosFiltrados = vuelosDisponibles.Where(vuelo =>
                {
                    return (vuelo.FechaHoraSalida.Date == null || vuelo.FechaHoraArribo.Date <= fechaVuelta.Value.Date) &&
                    vuelo.Origen == origen &&
                       vuelo.Destino == destino;
                }).ToList();

                // Mostrar los resultados en la interfaz de usuario
                vuelosListBox.Items.Clear();

                foreach (var vuelo in vuelosFiltrados)
                {
                    string origenVuelo = vuelo.NombreOrigen;
                    string destinoVuelo = vuelo.NombreDestino;
                    string fechaIdaVuelo = vuelo.FechaHoraSalida.ToString("yyyy-MM-dd HH:mm");
                    string fechaVueltaVuelo = vuelo.FechaHoraArribo.ToString("yyyy-MM-dd HH:mm");
                    string aerolinea = vuelo.Aerolinea;
                    string tiempoDeVuelo = vuelo.TiempoVuelo;

                    string vueloStr = $"{origenVuelo} - {destinoVuelo} | Ida: {fechaIdaVuelo} | Vuelta: {fechaVueltaVuelo} | Tiempo de vuelo: {tiempoDeVuelo} | Aerolinea: {aerolinea}";
                    vuelosListBox.Items.Add(vueloStr);
                }
            };

            Button volverButton = llamada.AgregarBoton("Volver al menú principal", 330, 260);
            volverButton.Click += (sender, e) =>
            {
                MenuVentas menuVentas = new MenuVentas();
                llamada.Hide(); // Ocultar el formulario actual (módulo de vuelos)
                menuVentas.ShowDialog(); // Mostrar el formulario del menú principal
            };

            Button agregarPresupuestoButton = llamada.AgregarBoton("Agregar al presupuesto", 630, 260);
            agregarPresupuestoButton.Click += (sender, e) =>
            {
                if (vuelosListBox.SelectedItem != null)
                {
                    string vueloSeleccionado = vuelosListBox.SelectedItem.ToString();
                    int cantidadAdultos = (int)adultosNumericUpDown.Value;
                    int cantidadMenores = (int)menoresNumericUpDown.Value;
                    int cantidadInfantes = (int)infantesNumericUpDown.Value;

                    if (Validaciones.ValidarCantidadAdultos(cantidadAdultos))
                    {
                        llamada.moduloPresupuesto.AgregarVueloAlPresupuesto(vueloSeleccionado, cantidadAdultos, cantidadMenores, cantidadInfantes);
                        MessageBox.Show($"Vuelo '{vueloSeleccionado}' agregado al presupuesto");
                    }
                    else
                    {
                        MessageBox.Show("Debe haber al menos 1 pasajero adulto.");
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, seleccione un vuelo de la lista.");
                }
            };
        }

    }
}
