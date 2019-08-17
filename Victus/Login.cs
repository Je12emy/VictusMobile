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
            // Declarar Variables
            Button btnIngresar = FindViewById<Button>(Resource.Id.btnIngresarLogin);
            EditText correo = FindViewById<EditText>(Resource.Id.inputCorreoLogin);
            EditText clave = FindViewById<EditText>(Resource.Id.inputClaveLogin);

            btnIngresar.Click += delegate{
                // Mensaje de feedback para el usuario
                Toast.MakeText(this, "Verificando", ToastLength.Long).Show();
                if (!string.IsNullOrEmpty(correo.Text) && !string.IsNullOrEmpty(clave.Text))
                {
                    // Declarar variables
                    Intent dashboard;
                    DataTable tabla;
                    VictusWebService cliente = new VictusWebService();

                    // Buscar las credenciales
                    tabla = cliente.BuscarUsuario(correo.Text);
                    // Verificacion del DataTable
                    if (tabla.Rows.Count > 0)
                    {
                        // Verificacion de la clave
                        if (clave.Text == tabla.Rows[0][1].ToString())
                        {
                            // Se avisa que se esta en proceso de iniciar sesion
                            Toast.MakeText(this, "Iniciando Sesion!", ToastLength.Long).Show();

                            dashboard = new Intent(this, typeof(Dashboard));                         
                            // Insertar un string para pasarlo atravez del modelo.
                            dashboard.PutExtra("correoUsuario", correo.Text);
                            // Iniciar Actividad
                            StartActivity(dashboard);
                        }
                        else
                            Toast.MakeText(this, "Error, credenciales erroneas", ToastLength.Long).Show();
                        // Retorna un error, pues la contraseña es incorrecta
                    }
                    else
                        Toast.MakeText(this, "Error, puede que ese correo no este registrado", ToastLength.Long).Show();
                    // Retorna un error, pues no se encontro ese correo en la BD
                }else
                    Toast.MakeText(this, "Por favor ingrese su correo y contraseña", ToastLength.Long).Show();
                    // Mostrar error
            };
        }
    }
}