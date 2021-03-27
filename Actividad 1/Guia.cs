using GalaSoft.MvvmLight.Command;
using Microsoft.TeamFoundation.Server;
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

namespace Actividad_1
{
    public class Guia : INotifyPropertyChanged
    {
        public ICommand MostrarAgregarCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        public ICommand AgregarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand EditarCommand { get; set; }

        public Guia()
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
                Episodios = new Episodios();
            }
            else
            {
                Episodios copia = new Episodios()
                {
                    TituloEspañol = Episodios.TituloEspañol,
                    TituloOriginal = Episodios.TituloOriginal,
                    NumeroEpisodio = Episodios.NumeroEpisodio,
                    NumeroTemporada= Episodios.NumeroTemporada,
                    Descripcion=Episodios.Descripcion
                };

                posicionOriginal = GuiaEpisodios.IndexOf(Episodios);
                

                Episodios = copia;

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

     
       public ObservableCollection<Episodios> GuiaEpisodios { get; set; }
       

        private Episodios episodios;

        public Episodios Episodios
        {
            get { return episodios; }
            set
            {
                episodios = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Episodios"));
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

            if (string.IsNullOrWhiteSpace(Episodios.TituloEspañol))
            {
                Error = "El episodio debe tener un nombre en Español.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Episodios.TituloOriginal))
            {
                Error = "El episodio debe tener un nombre en su idioma original.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Episodios.NumeroEpisodio))
            {
                Error = "El episodio debe tener un numero de episodio.";
                return;
            }
            if (string.IsNullOrWhiteSpace(Episodios.NumeroTemporada))
            {
                Error = "El episodio debe corresponder a una temporada de la serie.";
                return;
            }
            if (string.IsNullOrWhiteSpace(Episodios.Descripcion))
            {
                Error = "El episodio debe contar con una descricpcion del episodio.";
                return;
            }

            if (GuiaEpisodios.Any(x => x.TituloEspañol == Episodios.TituloEspañol))
            {
                Error = "Ya existe un episodio con el mismo nombre.";
                return;
            }


            GuiaEpisodios.Add(Episodios);

            
            Save();

            MostrarUserControl = false;
        }

        public void Editar()
        {
            Error = "";
            if (string.IsNullOrWhiteSpace(Episodios.TituloEspañol))
            {
                Error = "El episodio debe tener un nombre en Español.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Episodios.TituloOriginal))
            {
                Error = "El episodio debe tener un nombre en su idioma original.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Episodios.NumeroEpisodio))
            {
                Error = "El episodio debe tener un numero de episodio.";
                return;
            }
            if (string.IsNullOrWhiteSpace(Episodios.NumeroTemporada))
            {
                Error = "El episodio debe corresponder a una temporada de la serie.";
                return;
            }
            if (string.IsNullOrWhiteSpace(Episodios.Descripcion))
            {
                Error = "El episodio debe contar con una descricpcion del episodio.";
                return;
            }


            GuiaEpisodios[posicionOriginal] = Episodios;
            Save();
            MostrarUserControl = false;
        }

        public void Eliminar()
        {
            if (GuiaEpisodios.Remove(Episodios))
            {
                Save();
            }
        }

        private void Save()
        {
            FileStream fs = File.Create("episodios.dat");
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, GuiaEpisodios);
            fs.Close();
        }
        private void Load()
        {
            try
            {
                FileStream fs = File.OpenRead("episodios.dat");
                BinaryFormatter bf = new BinaryFormatter();
                GuiaEpisodios = (ObservableCollection<Episodios>)bf.Deserialize(fs);
                fs.Close();
            }
            catch (Exception)
            {
                GuiaEpisodios = new ObservableCollection<Episodios>();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
    }
