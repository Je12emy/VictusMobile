using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Victus.VictusWS;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Victus
{
    [Activity(Label = "Medidas")]
    public class Medidas : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Medidas);
            VictusWebService cliente = new VictusWebService();
            DataTable tabla = new DataTable();

            // Obtener la dieta mas reciente
            string _correo = Intent.GetStringExtra("correoUsuario");
            //Toast.MakeText(this, "Correo: " + _correo, ToastLength.Long).Show();
            tabla = cliente.ObtenerUltimaMedida(_correo);
            if (!string.IsNullOrWhiteSpace(tabla.Rows[0][0].ToString()))
            {
                DateTime fecha = Convert.ToDateTime(tabla.Rows[0][0]);
                string _fecha = tabla.Rows[0][0].ToString();
                tabla = cliente.ObtenerMedida(_correo, fecha.AddHours(-1));
                if (tabla.Rows.Count > 0)
                {

                    // Crear una lista para almacenar todos los items
                    ListView listMedida = FindViewById<ListView>(Resource.Id.listMedida);
                    List<string> itemsNew = new List<string>();
                    //List<string> itemsParte = new List<string>();
                    string[] partes = { "","","","Bicep Izquierdo","Bicep Derecho","Abdomen", "Cuadricep Izquierdo", "Cuadricep Derecho", "Pantorrilla Izquierda", "Pantorrila Derecha"};
                    // Llenar la lista
                    for (int i = 3; i < tabla.Columns.Count; i++)
                    {
                        itemsNew.Add(i - 2 + ". " + partes[i] + " - " + tabla.Rows[0][i].ToString() + " cm.");
                    }
                    // Almacenar la lista en un adaptador
                    ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, itemsNew);
                    // Llenar el ListView
                    listMedida.Adapter = adapter;


                }
            }
            else
                Toast.MakeText(this, "Parece que no tienes datos registrados", ToastLength.Long).Show();

            Button btnMedidasForm = FindViewById<Button>(Resource.Id.btnModificarMedidas);
            btnMedidasForm.Click += delegate {
                Intent medidas = new Intent(this,typeof(MedidasForm));
                medidas.PutExtra("correoUsuario", _correo);
                StartActivity(medidas);
            };

        }
    }
}