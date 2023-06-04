using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AplicacionTurismoGO_
{
    internal class ModuloHoteles
    {
        private ListBox menuListBox;

        private MenuVentas formulario;

        //Linea para que se puedan llamar los metodos de esta clase.

        public ModuloHoteles(MenuVentas form)
        {
            formulario = form;
        }
        public void ConsultarInventarioHoteleria()
        {
            formulario.LimpiarFormulario();

            formulario.AgregarEtiqueta("Destino:", 215, 5);
            TextBox destinoTextBox = formulario.AgregarTextBox(215, 30);
            formulario.AgregarEtiqueta("Fecha check-in:", 215, 55);
            DateTimePicker checkInPicker = formulario.AgregarDateTimePicker(215, 80);
            formulario.AgregarEtiqueta("Fecha check-out:", 215, 110);
            DateTimePicker checkOutPicker = formulario.AgregarDateTimePicker(215, 135);
            formulario.AgregarEtiqueta("Huéspedes:", 500, 100);
            NumericUpDown huespedesNumericUpDown = formulario.AgregarNumericUpDown(500, 130);

            Button buscarButton = formulario.AgregarBoton("Buscar", 230, 165);
            buscarButton.Click += (sender, e) =>
            {
                // Lógica de búsqueda de hoteles
                string destino = destinoTextBox.Text;
                DateTime checkIn = checkInPicker.Value;
                DateTime checkOut = checkOutPicker.Value;
                int cantidadHuespedes = (int)huespedesNumericUpDown.Value;

                // Implementar la lógica para buscar hoteles
                // y mostrar los resultados en la interfaz de usuario
            };

            Button volverButton = formulario.AgregarBoton("Volver al menú principal", 330, 165);
            volverButton.Click += (sender, e) =>
            {
                MenuVentas menuVentas = new MenuVentas();
                formulario.Hide(); // Ocultar el formulario actual (módulo de vuelos)
                menuVentas.ShowDialog(); // Mostrar el formulario del menú principal
            };
        }
    }
}
