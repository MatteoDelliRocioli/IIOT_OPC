namespace IIOT_OPC.Shared.Configuration
{
    using Newtonsoft.Json;
    using System.IO;

    /// <summary>
    /// Gestisce la configurazione dell'applicazione, appoggiandosi ad un file JSON
    /// rappresentato dalla classe T
    /// </summary>
    public class ConfigBuilder<T> where T : new()
    {
        private T _config;
        private string _filename;

        /// <summary>
        /// Carica il file indicato e lo mappa in Oggetto di configurazione di classe T
        /// </summary>
        /// <param name="filename">Nome del file da caricare</param>
        public ConfigBuilder(string filename)
        {
            try
            {
                Load(filename);
            }
            catch (FileNotFoundException e)
            {
                SaveTo(filename);
            }
        }

        public ConfigBuilder()
        {
        }

        /// <summary>
        /// Restituisce l'Oggetto di configurazione di classe T
        /// </summary>
        /// <returns>Oggetto di configurazione di classe T</returns>
        public T Config
        {
            get
            {
                if (_config == null)
                    _config = new T();
                return _config;
            }
        }

        /// <summary>
        /// Riassegna l'Oggetto di configurazione di classe T
        /// </summary>
        /// <param name="config">Oggetto di configurazione di classe T da caricare in memoria</param>
        /// <returns>Questo Builder</returns>
        public ConfigBuilder<T> SetConfig(T config)
        {
            _config = config;
            return this;
        }

        /// <summary>
        /// Carica l'Oggetto di configurazione di classe T da un file JSON
        /// </summary>
        /// <param name="filename">Nome del file da cui caricare la configurazione</param>
        /// <returns>Questo Builder</returns>
        public ConfigBuilder<T> Load(string filename)
        {
            _filename = filename;
            return Reload();
        }

        /// <summary>
        /// Ricarica l'Oggetto di configurazione di classe T
        /// </summary>
        /// <returns>Questo Builder</returns>
        public ConfigBuilder<T> Reload()
        {
            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(_filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                _config = (T)serializer.Deserialize(file, typeof(T));
            }
            return this;
        }

        /// <summary>
        /// Salva l'Oggetto di configurazione di classe T in un file JSON con nome 'nomeFile'
        /// </summary>
        /// <param name="filename">Nome del file in cui salvare l'Oggetto di configurazione di classe T</param>
        /// <returns>Questo Builder</returns>
        public ConfigBuilder<T> SaveTo(string filename)
        {
            _filename = filename;
            return Save();
        }

        /// <summary>
        ///  Salva l'Oggetto di configurazione di classe T in un file JSON con il nome già assegnato
        /// </summary>
        /// <returns></returns>
        public ConfigBuilder<T> Save()
        {
            // serialize JSON directly to a file
            using (StreamWriter file = File.CreateText(_filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, Config);
            }
            return this;
        }
    }
}