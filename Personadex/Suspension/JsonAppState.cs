using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Personadex.Suspension
{
    internal sealed class JsonAppState : IAppState
    {
        private const string FileName      = "PersonadexState.txt";
        private const string WriteTimeName = "WriteTime";

        private readonly Dictionary<string, JToken> _jsonTokens = new Dictionary<string, JToken>();

        public DateTime? LastUtcPersistTime
        {
            get
            {
                return ApplicationData.Current.LocalSettings.Values.ContainsKey(WriteTimeName)
                    ? DateTime.Parse((string)ApplicationData.Current.LocalSettings.Values[WriteTimeName])
                    : (DateTime?)null;
            }
        }

        public void WriteOutFrom<T>(string key, T value)
        {
            var jsonSerializable = value as IJsonSerializable;
            if (jsonSerializable != null)
            {
                _jsonTokens[key] = jsonSerializable.Write();
                return;
            }

            _jsonTokens[key] = new JValue(value);
        }

        public bool ReadInto<T>(string key, ref T value)
        {
            if (!_jsonTokens.ContainsKey(key))
            {
                return false;
            }

            var jsonSerializable = value as IJsonSerializable;
            if (jsonSerializable != null)
            {
                jsonSerializable.Read(_jsonTokens[key]);

                return true;
            }

            value = _jsonTokens[key].ToObject<T>();

            return true;
        }

        public async Task PersistStateAsync()
        {
            ApplicationData.Current.LocalSettings.Values[WriteTimeName] = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);

            await SaveJsonToDisk(CreateJObect(), FileName);
        }

        public async Task<bool> RestoreStateAsync()
        {
            var jobject = await LoadJsonFromDisk(FileName);
            if (jobject == null)
            {
                return false;
            }

            foreach (var jproperty in jobject.Properties())
            {
                _jsonTokens[jproperty.Name] = jproperty.Value;
            }

            return true;
        }

        private JObject CreateJObect()
        {
            var jobject = new JObject();
            foreach (var keyValue in _jsonTokens.Where(keyValue => keyValue.Value != null).ToList())
            {
                jobject.Add(keyValue.Key, keyValue.Value);
            }

            return jobject;
        }

        private static async Task SaveJsonToDisk(JToken jobject, string fileName)
        {
            var storageFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            using (var fileStream = await storageFile.OpenStreamForWriteAsync())
            using (var streamWriter = new StreamWriter(fileStream))
            {
                await streamWriter.WriteAsync(jobject.ToString(Formatting.Indented));
            }
        }

        private static async Task<JObject> LoadJsonFromDisk(string fileName)
        {
            StorageFile storageFile;
            try
            {
                storageFile = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
            }
            catch (FileNotFoundException)
            {
                return null;
            }

            string jsonString;

            using (var fileStream = await storageFile.OpenStreamForReadAsync())
            using (var streamReader = new StreamReader(fileStream))
            {
                jsonString = await streamReader.ReadToEndAsync();
            }

            return JObject.Parse(jsonString);
        }
    }
}
