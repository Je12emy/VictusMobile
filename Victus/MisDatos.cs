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
    [Activity(Label = "MisDatos")]
    public class MisDatos : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.MisDatos);

            VictusWebService cliente = new VictusWebService();
            DataTable tabla = new DataTable();

            tabla = cliente.BuscarUltimoRegistro(Intent.GetStringExtra("correoUsuario"));
            if (!string.IsNullOrWhiteSpace(tabla.Rows[0][0].ToString()))
            {
                DateTime fecha = Convert.ToDateTime(tabla.Rows[0][0].ToString());
                tabla = cliente.BuscarCliente(Intent.GetStringExtra("correoUsuario"), fecha);
                if (tabla.Rows.Count > 0)
                {
                    TextView imc = FindViewById<TextView>(Resource.Id.userIMC);
                    TextView agua = FindViewById<TextView>(Resource.Id.userAgua);
                    imc.Text = tabla.Rows[0][5].ToString();
                    agua.Text = tabla.Rows[0][6].ToString();
                }
            }
            else
                Toast.MakeText(this, "Parece que no tienes datos registrados", ToastLength.Long).Show();
            Button btnDatosForm = FindViewById<Button>(Resource.Id.btnModificarDatos);
            btnDatosForm.Click += delegate {
                Intent misDatosForm = new Intent(this,typeof(MisDatosForm));
                misDatosForm.PutExtra("correoUsuario", Intent.GetStringExtra("correoUsuaio"));
                StartActivity(misDatosForm);
            };
        }
    }
}