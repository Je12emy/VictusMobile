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
    [Activity(Label = "NivelCaloricoForm")]
    public class NivelCaloricoForm : Activity
    {
        string opcion;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.CaloriasForm);

            RadioGroup radioNivelActividad = FindViewById<RadioGroup>(Resource.Id.radioNivelActividad);
            RadioButton muyBajo = FindViewById<RadioButton>(Resource.Id.actividadMuyBaja);
            RadioButton bajo = FindViewById<RadioButton>(Resource.Id.actividadBaja);
            RadioButton moderado = FindViewById<RadioButton>(Resource.Id.actividadMedia);
            RadioButton alta = FindViewById<RadioButton>(Resource.Id.actividadAlta);
            RadioButton muyAlta = FindViewById<RadioButton>(Resource.Id.actividadMuyAlta);

            opcion = "sedentario";

            muyBajo.Click += RadioButtonClick;
            bajo.Click += RadioButtonClick;
            moderado.Click += RadioButtonClick;
            alta.Click += RadioButtonClick;
            muyAlta.Click += RadioButtonClick;

            Button btnModificar = FindViewById<Button>(Resource.Id.btnModificarNivelActividadForm);
            btnModificar.Click += delegate
            {
                Toast.MakeText(this, "Agregando tus datos", ToastLength.Short).Show();
                // Calculos
                // Obtener los datos personales del usuario
                VictusWebService cliente = new VictusWebService();
                DataTable tabla = new DataTable();

                string _correo = Intent.GetStringExtra("correoUsuario");
                //Toast.MakeText(this, "Correo: " + _correo, ToastLength.Long).Show();
                tabla = cliente.BuscarUltimoRegistro(Intent.GetStringExtra("correoUsuario"));
                if (!string.IsNullOrWhiteSpace(tabla.Rows[0][0].ToString()))
                {
                    DateTime fecha = Convert.ToDateTime(tabla.Rows[0][0]);
                    string _fecha = tabla.Rows[0][0].ToString();
                    tabla = cliente.BuscarCliente(_correo, fecha.AddHours(-1));
                    if (tabla.Rows.Count > 0)
                    {
                        int edad;
                        double peso;
                        int altura;
                        // Capturar las variables
                        peso = Convert.ToDouble(tabla.Rows[0][2]);
                        altura = Convert.ToInt32(tabla.Rows[0][3]);
                        edad = Convert.ToInt32(tabla.Rows[0][4]);
                        // Obtener el genero
                        tabla = cliente.BuscarUsuarioTodo(_correo);
                        if (tabla.Rows.Count > 0)
                        {
                            bool genero = Convert.ToBoolean(tabla.Rows[0][5]);

                            double factorActividad = 1;
                            // Seleccionar un factor de actividad
                            if (opcion.Equals("sedentario"))
                            {
                                factorActividad = 1.2;
                            }
                            else if (opcion.Equals("ligero"))
                            {
                                factorActividad = 1.375;
                            }
                            else if (opcion.Equals("moderado"))
                            {
                                factorActividad = 1.55;
                            }
                            else if (opcion.Equals("intenso"))
                            {
                                factorActividad = 1.725;
                            }
                            else if (opcion.Equals("muy_intenso"))
                            {
                                factorActividad = 1.9;
                            }
                            // Calcular la tasa de metabolismo basal
                            double tmb;

                            if (genero)
                            {
                                tmb = 66 + (13.7 * peso) + (5 * altura) - (6.8 * edad);
                            }
                            else
                                tmb = 655 + (9.6 * peso) + (1.8 * altura) - (4.7 * edad);

                            double totalCalorias;
                            totalCalorias = tmb * factorActividad;

                            // Insertar todos los datos
                            int i;
                            i = cliente.AgregarRegistroHarris(factorActividad, tmb, totalCalorias, fecha, _correo);
                            if (i > 0)
                            {
                                Toast.MakeText(this, "Se han registrado tus datos", ToastLength.Long).Show();

                                Intent dashboard = new Intent(this, typeof(Dashboard));
                                dashboard.PutExtra("correoUsuario", Intent.GetStringExtra("correoUsuario").ToString());
                                StartActivity(dashboard);
                            }
                            else
                                Toast.MakeText(this, "Se produjo un error la registrar tus datos", ToastLength.Long).Show();

                        }
                    }
                    else
                        Toast.MakeText(this, "Parece que no tienes tus Datos Personales registrados", ToastLength.Long).Show();

                }
                else
                    Toast.MakeText(this, "Parece que no tienes datos registrados", ToastLength.Long).Show();
            };
        }


        private void RadioButtonClick(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Text == "Trabajo de escritorio – sin ejercicio")
            {
                opcion = "sedentario";
            }
            else if (rb.Text == "Ejercicio 1-3 días por semana")
            {
                opcion = "ligero";
            }
            else if (rb.Text == "Ejercicio 3-5 días por semana")
            {
                opcion = "moderado";
            }
            else if (rb.Text == "Ejercicio 6-7 días por semana")
            {
                opcion = "intenso";
            }
            else if (rb.Text == "Ejercicio 2 veces al día")
            {
                opcion = "muy_intenso";
            }
        }
    }
}
