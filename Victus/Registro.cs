using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victus.VictusWS;
using System.Data;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content;

namespace Victus
{
    [Activity(Label = "Registro")]
    public class Registro : Activity
    {
        bool genero;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Registro);


            // Rellenar el spinet con valores
            // Fuente: https://docs.microsoft.com/es-es/xamarin/android/user-interface/controls/spinner
            Spinner spinner = FindViewById<Spinner>(Resource.Id.spinner);

            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(
                    this, Resource.Array.planets_array, Android.Resource.Layout.SimpleSpinnerItem);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
            // Boton
            Button btnRegistrarme = FindViewById<Button>(Resource.Id.btnRegistrarme);
            btnRegistrarme.Click += delegate
            {
                
                Toast.MakeText(this, "Registrando", ToastLength.Long).Show();
                EditText correo = FindViewById<EditText>(Resource.Id.inputCorreo);
                EditText cedula = FindViewById<EditText>(Resource.Id.inputCedula);
                EditText clave = FindViewById<EditText>(Resource.Id.inputClave);
                // Separar el nombre completo
                EditText nombre = FindViewById<EditText>(Resource.Id.inputNombre);

                // Si todos los campos estan llenos
                if (!string.IsNullOrEmpty(correo.Text) && !string.IsNullOrEmpty(cedula.Text) && !string.IsNullOrEmpty(clave.Text) && !string.IsNullOrEmpty(nombre.Text))
                {
                    // Hacer split en los espacios
                    var nombreCompleto = nombre.Text.Split(' ');
                    if (nombreCompleto.Length < 2)
                    {
                        Toast.MakeText(this, "Por favor ingrese su Nombres y Apellidos!", ToastLength.Long).Show();
                        // Mostrar error          
                    }
                    else {
                        // Capturarlo en una variable
                        string _nombre = nombreCompleto[0];
                        string primerApellido = nombreCompleto[1];
                        string segundoApellido = nombreCompleto[2];

                        // Registrar usuario en el sistema
                        VictusWebService cliente = new VictusWebService();
                        int i;
                        i = cliente.CrearUsuario(correo.Text, Convert.ToInt32(cedula.Text), _nombre, primerApellido, segundoApellido, genero, clave.Text);
                        if (i > 0)
                        {
                            Toast.MakeText(this, "Ha sido registrado!", ToastLength.Long).Show();
                        }
                        else
                            Toast.MakeText(this, "Se produjo un error al registrarlo!", ToastLength.Long).Show();

                        // Volver al inicio de la aplicacion.
                        Intent inicio = new Intent(this, typeof(MainActivity));
                        StartActivity(inicio);
                    }
                        
                    
                }else
                    Toast.MakeText(this, "Faltan datos de seleccionar!", ToastLength.Long).Show();
                    // Mostrar error
            };

        }
        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            if (spinner.GetItemAtPosition(e.Position).ToString() == "Masculino")
            {
                genero = true;
            }
            else if (spinner.GetItemAtPosition(e.Position).ToString() == "Femenino")
            {
                genero = false;
            }
        }
    }
}