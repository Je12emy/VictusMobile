using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using Victus.VictusWS;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Victus
{
    [Activity(Label = "Dieta")]
    public class Dieta : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Dieta);
            VictusWebService cliente = new VictusWebService();
            DataTable tabla = new DataTable();

            // Obtener la dieta mas reciente
            string _correo = Intent.GetStringExtra("correoUsuario");
            Toast.MakeText(this, "Correo: " + _correo, ToastLength.Long).Show();
            tabla = cliente.ObtenerUltimaDieta(_correo);
            if (!string.IsNullOrWhiteSpace(tabla.Rows[0][0].ToString()))
            {
                DateTime fecha = Convert.ToDateTime(tabla.Rows[0][0]);
                string _fecha = tabla.Rows[0][0].ToString();
                tabla = cliente.ObtenerDieta(_correo, fecha.AddHours(-1));
                if (tabla.Rows.Count > 0)
                {
                    // Obtener el DataTable con la dieta completa
                    tabla = cliente.ObtenerDietaCompleta(_correo,Convert.ToInt32(tabla.Rows[0][0]));
                    if (tabla.Rows.Count>0)
                    {
                        // Crear una lista para almacenar todos los items
                        ListView listDieta = FindViewById<ListView>(Resource.Id.listDieta);
                        List<string> itemsNew = new List<string>();
                        // Llenar la lista
                        for (int i = 0; i < tabla.Rows.Count; i++)
                        {
                            itemsNew.Add(i+1+". " +tabla.Rows[i][0].ToString() + " - " + tabla.Rows[i][1].ToString() + " calorias.");
                        }
                        // Almacenar la lista en un adaptador
                        ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, itemsNew);
                        // Llenar el ListView
                        listDieta.Adapter = adapter;
                    }
             
                }
            }
            else
                Toast.MakeText(this, "Parece que no tienes datos registrados", ToastLength.Long).Show();


        }
    }
}