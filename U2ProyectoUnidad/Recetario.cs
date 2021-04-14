using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace U2ProyectoUnidad
{
    public enum Vistas { Lista, Agregar, Editar, Eliminar }
    public class Recetario : INotifyPropertyChanged
    {
        public ICommand AgregarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand GuardarCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand CambiarVistaCommand { get; set; }
        public ObservableCollection<Receta> Recetas { get; set; }

        public Recetario()
        {
            Cargar();
            CambiarVistaCommand = new RelayCommand<Vistas>(CambiarVista);
            AgregarCommand = new RelayCommand(Agregar);
            GuardarCommand = new RelayCommand(Guardar);
            EliminarCommand = new RelayCommand(Eliminar);
            EditarCommand = new RelayCommand(Editar);
            CancelarCommand = new RelayCommand(Cancelar);
        }
        private void Cancelar()
        {
            CambiarVista(Vistas.Lista);

        }

        private bool ver;
        public bool MostrarUserControl
        {
            get { return ver; }
            set 
            {
                ver = value;
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs("MostrarUserControl"));
            }
        }

        private void CambiarVista(Vistas obj)
        {
            Vista = obj;
            if (obj== Vistas.Agregar)
            {
                Receta = new Receta();
            }
            if (obj==Vistas.Editar)
            {
                indiceRecetaOriginal = Recetas.IndexOf(Receta);

                var clon = new Receta
                {
                    Nombre = Receta.Nombre,
                    Ingredientes = Receta.Ingredientes,
                    Procedimiento = Receta.Procedimiento,
                    Imagen = Receta.Imagen
                };
                Receta = clon;
            }

        }

        private Vistas vistas;
        public Vistas Vista
        {
            get { return vistas; }
            set
            {
                vistas = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Vista)));
            }
        }

        private Receta receta;
        public Receta Receta
        {
            get { return receta; }
            set
            {
                receta = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Receta"));
            }
        }

        private string error;
        public string Error 
        {
            get { return error; }
            set
            {
                error = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Error)));
            }
        }

        int indiceRecetaOriginal;

       

        public void Agregar()
        {
            Error = "";
            if (string.IsNullOrWhiteSpace(Receta.Nombre))
            {
                Error = "Escribe el nombre de la receta.";
                return;
            }
            if (string.IsNullOrWhiteSpace(Receta.Ingredientes))
            {
                Error = "Escribe los ingredientes de la receta.";
                return;
            }
            if (string.IsNullOrWhiteSpace(Receta.Procedimiento))
            {
                Error = "Escribe el procedimiento de la receta.";
                return;
            }
            if (string.IsNullOrWhiteSpace(Receta.Imagen))
            {
                Error = "Escribe el URL de la imagen del platillo.";
                return;
            }
          
                if (Recetas.Any(x => x.Nombre.ToUpper() == Receta.Nombre.ToUpper()))
                {
                    Error = "Ya existe una receta con el mismo nombre.";
                    return;
                }
            
            Recetas.Add(Receta);
            Guardar();
            CambiarVista(Vistas.Lista);
        }

        public void Editar()
        {
            Cargar();
            Error="";
            if (string.IsNullOrWhiteSpace(Receta.Nombre))
            {
                Error = "Escribe el nombre de la receta.";
                return;
            }
            if (string.IsNullOrWhiteSpace(Receta.Ingredientes))
            {
                Error = "Escribe los ingredientes de la receta.";
                return;
            }
            if (string.IsNullOrWhiteSpace(Receta.Procedimiento))
            {
                Error = "Escribe el procedimiento de la receta.";
                return;
            }
            if (string.IsNullOrWhiteSpace(Receta.Imagen))
            {
                Error = "Escribe el URL de la imagen del platillo.";
                return;
            }
            Receta Original = Recetas[indiceRecetaOriginal];
            if (Original.Nombre!=receta.Nombre)
            {
                if (Recetas.Any(x=>x.Nombre.ToUpper()==Receta.Nombre.ToUpper()))
                {
                    Error = "Ya existe una receta con el mismo nombre.";
                    return;
                }
            }
            Recetas[indiceRecetaOriginal] = Receta;
            Guardar();
            CambiarVista(Vistas.Lista);
        }

        public void Eliminar()
        {
            if (Recetas.Remove(Receta))
            {
                Guardar();
            }
            CambiarVista(Vistas.Lista);
        }

        private void Guardar()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Create("recetas.data");
            bf.Serialize(fs, Recetas);
            fs.Close();
        }

        private void Cargar()
        {
            try
            {
                FileStream fs = File.OpenRead("recetas.data");
                BinaryFormatter bf = new BinaryFormatter();
                Recetas = (ObservableCollection<Receta>)bf.Deserialize(fs);
                fs.Close();
            }
            catch
            {
                Recetas = new ObservableCollection<Receta>();
            }
        }
      

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
