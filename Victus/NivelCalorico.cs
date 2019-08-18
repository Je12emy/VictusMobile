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
    [Activity(Label = "NivelCalorico")]
    public class NivelCalorico : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.NivelCalorico);
            VictusWebService cliente = new VictusWebService();
            DataTable tabla = new DataTable();

            string _correo = Intent.GetStringExtra("correoUsuario");
            Toast.MakeText(this, "Correo: " + _correo, ToastLength.Long).Show();
            tabla = cliente.BuscarUltimoRegistroHarris(Intent.GetStringExtra("correoUsuario"));
            if (!string.IsNullOrWhiteSpace(tabla.Rows[0][0].ToString()))
            {
                DateTime fecha = Convert.ToDateTime(tabla.Rows[0][0]);
                string _fecha = tabla.Rows[0][0].ToString();
                tabla = cliente.BuscarRegistroHarris(_correo, fecha.AddHours(-1));
                if (tabla.Rows.Count > 0)
                {
                    TextView nivelCalorico = FindViewById<TextView>(Resource.Id.userCalorias);
                    nivelCalorico.Text = tabla.Rows[0][3].ToString();
                }
            }
            else
                Toast.MakeText(this, "Parece que no tienes datos registrados", ToastLength.Long).Show();

            Button btnNivelCalorico = FindViewById<Button>(Resource.Id.btnModificarNivelCalorico);
            btnNivelCalorico.Click += delegate
            {
                Intent miNivelActividad = new Intent(this, typeof(NivelCaloricoForm));
                miNivelActividad.PutExtra("correoUsuario", _correo);
                StartActivity(miNivelActividad);
            };
        }
    }
}