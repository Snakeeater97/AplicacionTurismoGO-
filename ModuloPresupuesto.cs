using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicacionTurismoGO_
{
    public class ModuloPresupuesto
    {
        private ListBox presupuestoListBox;

        private MenuVentas formulario;

        private List<Producto> presupuesto;

        //Linea para que se puedan llamar los metodos de esta clase.

        public ModuloPresupuesto(MenuVentas form)
        {
            formulario = form;
            presupuesto = new List<Producto>();
            presupuestoListBox = new ListBox();
        }
        public List<Producto> ObtenerPresupuesto()
        {
            return presupuesto;
        }

        public void VerPresupuestoActual()
        {
            //formulario.LimpiarFormulario();

            

            formulario.AgregarEtiqueta("Presupuesto actual:", 215, 10);

            presupuestoListBox.Location = new Point(215, 50);
            presupuestoListBox.Size = new Size(215, 150);
            presupuestoListBox.DisplayMember = "Nombre";
            presupuestoListBox.DataSource = presupuesto;
            formulario.Controls.Add(presupuestoListBox);

            Button eliminarButton = formulario.AgregarBoton("Eliminar producto", 215, 190);
            eliminarButton.Click += (sender, e) =>
            {
                if (presupuestoListBox.SelectedIndex >= 0)
                {
                    presupuesto.RemoveAt(presupuestoListBox.SelectedIndex);
                    presupuestoListBox.DataSource = null;
                    presupuestoListBox.DataSource = presupuesto;
                }
            };

            Button confirmarButton = formulario.AgregarBoton("Confirmar pre-reserva", 215, 190);
            confirmarButton.Click += (sender, e) =>
            {
                // Lógica para confirmar pre-reserva
                MessageBox.Show("Pre-reserva confirmada");
            };

            Button volverButton = formulario.AgregarBoton("Volver al menú principal", 500, 260);
            volverButton.Click += (sender, e) =>
            {
                formulario.Hide(); // Ocultar el formulario actual (módulo de vuelos)
                formulario.Owner.Show(); // Mostrar el formulario del menú principal
            };

        }
        public void AgregarVueloAlPresupuesto(string vuelo, int cantidadPasajeros)
        {
            // Lógica para agregar el vuelo al presupuesto
            decimal precioPorPasajero = 100.0m; // Precio ficticio por pasajero (puedes ajustarlo según tus necesidades)
            decimal precioTotal = cantidadPasajeros * precioPorPasajero;

            Producto producto = new Producto
            {
                Nombre = vuelo,
                Precio = precioTotal,
            };

            presupuesto.Add(producto);
        }
        
    }
    public class Producto
    {
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
    }
}
