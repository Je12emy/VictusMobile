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
using Android.Content;

namespace Victus
{
    [Activity(Label = "Login")]
    public class Login : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Login);
            Button btnIngresar = FindViewById<Button>(Resource.Id.btnIngresarLogin);
            EditText correo = FindViewById<EditText>(Resource.Id.inputCorreoLogin);
            EditText clave = FindViewById<EditText>(Resource.Id.inputClaveLogin);

            btnIngresar.Click += delegate{
                Toast.MakeText(this, "Verificando", ToastLength.Long).Show();
                Intent dashboard;
                DataTable tabla;
                VictusWebService cliente = new VictusWebService();

                tabla = cliente.BuscarUsuario(correo.Text);
                if (tabla.Rows.Count>0)
                {
                    if (clave.Text == tabla.Rows[0][1].ToString())
                    {
                        Toast.MakeText(this, "Iniciando Sesion!", ToastLength.Long).Show();
                        dashboard = new Intent(this,typeof(MainActivity));
                        StartActivity(dashboard);
                    }else
                        Toast.MakeText(this, "Error, credenciales erroneas", ToastLength.Long).Show();

                }
                else
                    Toast.MakeText(this, "Error, puede que ese correo no este registrado", ToastLength.Long).Show();
            };
        }
    }
}