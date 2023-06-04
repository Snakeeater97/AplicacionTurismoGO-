using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using System.Globalization;
using static System.Windows.Forms.DataFormats;

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
            llamada.AgregarEtiqueta("Pasajeros:", 500, 100);
            NumericUpDown pasajerosNumericUpDown = llamada.AgregarNumericUpDown(500, 130);
            vuelosListBox = new ListBox();
            vuelosListBox.Location = new Point(215, 290);
            vuelosListBox.Size = new Size(400, 150);
            llamada.Controls.Add(vuelosListBox);

            Button buscarButton = llamada.AgregarBoton("Buscar", 215, 260);
            buscarButton.Click += (sender, e) =>
            {
                string origen = origenTextBox.Text;
                string destino = destinoTextBox.Text;
                DateTime fechaIda = fechaIdaPicker.Value;
                DateTime? fechaVuelta = fechaVueltaPicker.Checked ? fechaVueltaPicker.Value : null;
                int cantidadPasajeros = (int)pasajerosNumericUpDown.Value;

                // Cargar los vuelos desde el archivo JSON
                string rutaArchivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VuelosDisponibles.json");
                List<Dictionary<string, string>> vuelosDisponibles = new List<Dictionary<string, string>>();

                try
                {
                    string contenidoJson = File.ReadAllText(rutaArchivo);
                    vuelosDisponibles = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(contenidoJson);
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
                List<Dictionary<string, string>> vuelosFiltrados = vuelosDisponibles.Where(vuelo =>
                {
                    DateTime fechaIdaVuelo = DateTime.ParseExact(vuelo["Ida"], "d/M/yyyy", CultureInfo.InvariantCulture);
                    DateTime fechaVueltaVuelo = DateTime.ParseExact(vuelo["Vuelta"], "d/M/yyyy", CultureInfo.InvariantCulture);

                    return fechaIda <= fechaIdaVuelo &&
                        (!fechaVuelta.HasValue || fechaVuelta >= fechaVueltaVuelo) &&
                        vuelo["Origen"] == origen &&
                        vuelo["Destino"] == destino;
                }).ToList();


                // Mostrar los resultados en la interfaz de usuario
                vuelosListBox.Items.Clear();

                foreach (var vuelo in vuelosFiltrados)
                {
                    string origenVuelo = vuelo["Origen"];
                    string destinoVuelo = vuelo["Destino"];
                    string fechaIdaVuelo = vuelo["Ida"];
                    string fechaVueltaVuelo = vuelo["Vuelta"];

                    string vueloStr = $"{origenVuelo} - {destinoVuelo} | Ida: {fechaIdaVuelo} | Vuelta: {fechaVueltaVuelo}";
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



            Button agregarPresupuestoButton = llamada.AgregarBoton("Agregar al presupuesto", 630, 340);
            agregarPresupuestoButton.Click += (sender, e) =>
            {
                if (vuelosListBox.SelectedItem != null)
                {
                    string vueloSeleccionado = vuelosListBox.SelectedItem.ToString();
                    int cantidadPasajeros = (int)pasajerosNumericUpDown.Value;

                    llamada.moduloPresupuesto.AgregarVueloAlPresupuesto(vueloSeleccionado, cantidadPasajeros);

                    MessageBox.Show($"Vuelo '{vueloSeleccionado}' agregado al presupuesto");
                }
                else
                {
                    MessageBox.Show("Por favor, seleccione un vuelo de la lista.");
                }
            };



        }
    }
}
