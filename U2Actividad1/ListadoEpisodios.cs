using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace U2Actividad1
{
    public class ListadoEpisodios : INotifyPropertyChanged
    {
        public ICommand MostrarAgregarCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        public ICommand AgregarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand EditarCommand { get; set; }

        public ListadoEpisodios()
        {
            Load();
            MostrarAgregarCommand = new RelayCommand<string>(VerAgregar);
            CancelarCommand = new RelayCommand(Cancelar);
            AgregarCommand = new RelayCommand(Agregar);
            EliminarCommand = new RelayCommand(Eliminar);
            EditarCommand = new RelayCommand(Editar);
        }
        private void Cancelar()
        {
            MostrarUserControl = false;
        }

        private bool ver;

        public bool MostrarUserControl
        {
            get { return ver; }
            set
            {
                ver = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MostrarUserControl"));
            }
        }

        int posicionOriginal;
        void VerAgregar(string modo)
        {
            Modo = modo;
            if (modo == "Agregar")
            {
                Episodio = new Episodio();
            }
            else
            {
                Episodio copia = new Episodio()
                {
                    TituloEspañol = Episodio.TituloEspañol,
                    TituloOriginal = Episodio.TituloOriginal,
                    NumeroEpisodio = Episodio.NumeroEpisodio,
                    NumeroTemporada = Episodio.NumeroTemporada,
                    Descripcion = Episodio.Descripcion
                };

                posicionOriginal = ListaEpisodios.IndexOf(Episodio);


                Episodio = copia;

            }
            MostrarUserControl = true;
        }

        private string modo;

        public string Modo
        {
            get { return modo; }
            set
            {
                modo = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Modo"));
            }
        }


        public ObservableCollection<Episodio> ListaEpisodios { get; set; }


        private Episodio episodio;

        public Episodio Episodio
        {
            get { return episodio; }
            set
            {
                episodio = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Episodio"));
            }
        }

        private string error;



        public string Error
        {
            get { return error; }
            set
            {
                error = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Error"));
            }
        }



        public void Agregar()
        {

            Error = "";

            if (string.IsNullOrWhiteSpace(Episodio.TituloEspañol))
            {
                Error = "El episodio debe tener un nombre en Español.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Episodio.TituloOriginal))
            {
                Error = "El episodio debe tener un nombre en su idioma original.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Episodio.NumeroEpisodio))
            {
                Error = "El episodio debe tener un numero de episodio.";
                return;
            }
            if (string.IsNullOrWhiteSpace(Episodio.NumeroTemporada))
            {
                Error = "El episodio debe corresponder a una temporada de la serie.";
                return;
            }
            if (string.IsNullOrWhiteSpace(Episodio.Descripcion))
            {
                Error = "El episodio debe contar con una descricpcion del episodio.";
                return;
            }

            if (ListaEpisodios.Any(x => x.TituloEspañol == Episodio.TituloEspañol))
            {
                Error = "Ya existe un episodio con el mismo nombre.";
                return;
            }


            ListaEpisodios.Add(Episodio);


            Save();

            MostrarUserControl = false;
        }

        public void Editar()
        {
            Error = "";
            if (string.IsNullOrWhiteSpace(Episodio.TituloEspañol))
            {
                Error = "El episodio debe tener un nombre en Español.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Episodio.TituloOriginal))
            {
                Error = "El episodio debe tener un nombre en su idioma original.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Episodio.NumeroEpisodio))
            {
                Error = "El episodio debe tener un numero de episodio.";
                return;
            }
            if (string.IsNullOrWhiteSpace(Episodio.NumeroTemporada))
            {
                Error = "El episodio debe corresponder a una temporada de la serie.";
                return;
            }
            if (string.IsNullOrWhiteSpace(Episodio.Descripcion))
            {
                Error = "El episodio debe contar con una descricpcion del episodio.";
                return;
            }


            ListaEpisodios[posicionOriginal] = Episodio;
            Save();
            MostrarUserControl = false;
        }

        public void Eliminar()
        {
            if (ListaEpisodios.Remove(Episodio))
            {
                Save();
            }
        }

        private void Save()
        {
            FileStream fs = File.Create("episodio.dat");
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, ListaEpisodios);
            fs.Close();
        }
        private void Load()
        {
            try
            {
                FileStream fs = File.OpenRead("episodio.dat");
                BinaryFormatter bf = new BinaryFormatter();
                ListaEpisodios = (ObservableCollection<Episodio>)bf.Deserialize(fs);
                fs.Close();
            }
            catch (Exception)
            {
                ListaEpisodios = new ObservableCollection<Episodio>();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
