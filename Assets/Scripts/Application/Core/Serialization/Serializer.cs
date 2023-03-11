namespace Application.Core.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using ImGuiNET;
    using Newtonsoft.Json;
    using Surrogates;
    using UnityEngine;
    using Utility;
    using Debug = UnityEngine.Debug;

    /// <summary>
    /// Central API for saving and loading game state.
    /// </summary>
    public class Serializer
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented,
        };

        private Dictionary<string, object> _savedData;

        /// <summary>
        /// Initializes a new instance of the <see cref="Serializer"/> class.
        /// </summary>
        public Serializer()
        {
            SurrogateList = new Collection<SurrogateData>
            {
                new SurrogateData { Surrogate = new Vector3SerializationSurrogate(), Type = typeof(Vector3) },
                new SurrogateData { Surrogate = new QuaternionSerializationSurrogate(), Type = typeof(Quaternion) },
            };

            _savedData = new Dictionary<string, object>();

            ImGuiUtil.Register(DrawDebugUI);
        }

        /// <summary>
        /// Gets the list of surrogates used for serializing data.
        /// </summary>
        private Collection<SurrogateData> SurrogateList { get; }

        /// <summary>
        /// Returns the save path of a certain save file name.
        /// </summary>
        /// <param name="fileName">The name of the save file to use in the path.</param>
        /// <returns>The path to the save file named "fileName".</returns>
        public static string GetPath(string fileName)
        {
            string path = $"{Application.persistentDataPath}/Saves";
            Directory.CreateDirectory(path);
            return $"{path}/{fileName}";
        }

        /// <summary>
        /// Determine if a save file exists on the disk.
        /// </summary>
        /// <param name="fileName">The save file name to search for.</param>
        /// <returns>True if the save file exists, false if it does not.</returns>
        public static bool IsValid(string fileName)
        {
            string savePath = GetPath(fileName);
            return File.Exists(savePath);
        }

        /// <summary>
        /// Attempts to deserialize some data from a file.
        /// </summary>
        /// <param name="fileName">The name of the file to read from.</param>
        /// <param name="result">The result of the deserialization.</param>
        /// <typeparam name="T">The type of result we are expecting.</typeparam>
        /// <returns>True if we successfully loaded the data, false if something went wrong.</returns>
        public static bool TryLoad<T>(string fileName, out T result)
        {
            if (IsValid(fileName))
            {
                string path = GetPath(fileName);
                string data = File.ReadAllText(path);
                result = JsonConvert.DeserializeObject<T>(data, Settings);
                return true;
            }

            result = default;
            return false;
        }

        /// <summary>
        /// Serializes some object data to the disk.
        /// </summary>
        /// <param name="fileName">The name of the file to write the data to.</param>
        /// <param name="target">The object to serialize into the file.</param>
        /// <typeparam name="T">The type of data we are serializing.</typeparam>
        public static void Save<T>(string fileName, T target)
        {
            string path = GetPath(fileName);
            string data = JsonConvert.SerializeObject(target, Settings);
            File.WriteAllText(path, data);
        }

        /// <summary>
        /// Serializes all saved data into a save file.
        /// </summary>
        /// <param name="fileName">The name of the save file to write the information into.</param>
        public void WriteToDisk(string fileName)
        {
            // Creates a new file for the save.
            string savePath = GetPath(fileName);
            using FileStream saveFile = File.Create(savePath);

            // Writes our data to the file.
            var surrogateSelector = new SurrogateSelector();

            foreach (var surrogateData in SurrogateList)
            {
                surrogateSelector.AddSurrogate(
                    surrogateData.Type,
                    new StreamingContext(StreamingContextStates.All),
                    surrogateData.Surrogate);
            }

            var binaryFormatter = new BinaryFormatter { SurrogateSelector = surrogateSelector };
            binaryFormatter.Serialize(saveFile, _savedData);
        }

        /// <summary>
        /// Deserializes all saved data from a save file.
        /// </summary>
        /// <param name="fileName">The name of the save file to read the information from.</param>
        public void ReadFromDisk(string fileName)
        {
            string savePath = GetPath(fileName);
            FileStream fileStream;

            if (!IsValid(fileName))
            {
                Debug.LogWarning($"Tried to open the save \"{fileName}\" that doesn't exist! Creating a new file...");
                fileStream = File.Create(savePath);
            }
            else
            {
                fileStream = File.OpenRead(savePath);
            }

            Debug.Log($"Accessed the save \"{fileName}\".");

            // Opens the save file.
            if (fileStream.Length != 0)
            {
                // Parses the data from our file.
                var surrogateSelector = new SurrogateSelector();

                foreach (var surrogateData in SurrogateList)
                {
                    surrogateSelector.AddSurrogate(
                        surrogateData.Type,
                        new StreamingContext(StreamingContextStates.All),
                        surrogateData.Surrogate);
                }

                var binaryFormatter = new BinaryFormatter { SurrogateSelector = surrogateSelector };
                _savedData = (Dictionary<string, object>)binaryFormatter.Deserialize(fileStream);
            }

            fileStream.Dispose();
        }

        /// <summary>
        /// Writes information to serializable components attached to a GameObject.
        /// </summary>
        /// <param name="gameObject">The GameObject to search for serializable components.</param>
        public void ApplySavedDataTo(GameObject gameObject)
        {
            if (gameObject == null)
            {
                return;
            }

            foreach (var serializable in gameObject.GetComponentsInChildren<ISerializable>())
            {
                ApplySavedDataTo(serializable);
            }
        }

        /// <summary>
        /// Writes saved information into a serializable object.
        /// </summary>
        /// <param name="serializable">The object to write information into.</param>
        public void ApplySavedDataTo(ISerializable serializable)
        {
            if (serializable == null)
            {
                return;
            }

            string id = serializable.Id;

            if (_savedData.ContainsKey(id))
            {
                var data = _savedData[id];
                serializable.ReadData(data);
            }
        }

        /// <summary>
        /// Reads information from serializable components attached to a GameObject.
        /// </summary>
        /// <param name="gameObject">The GameObject to search for serialized components.</param>
        public void UpdateSavedDataFrom(GameObject gameObject)
        {
            if (gameObject == null)
            {
                return;
            }

            foreach (var serializable in gameObject.GetComponentsInChildren<ISerializable>())
            {
                UpdateSavedDataFrom(serializable);
            }
        }

        /// <summary>
        /// Reads information from a serializable object.
        /// </summary>
        /// <param name="serializable">The object to read information from.</param>
        public void UpdateSavedDataFrom(ISerializable serializable)
        {
            if (serializable == null)
            {
                return;
            }

            string id = serializable.Id;

            if (!_savedData.ContainsKey(id))
            {
                _savedData.Add(id, serializable.WriteData());
            }
            else
            {
                _savedData[id] = serializable.WriteData();
            }
        }

        private static void DrawDebugUI()
        {
            ImGui.Begin("Serialization");

            var path = $"{Application.persistentDataPath}/Saves";

            if (ImGui.Button("Open Save Location"))
            {
                Process.Start(path);
            }

            if (ImGui.Button("Clear Old Saves"))
            {
                Directory.Delete(path);
            }

            ImGui.End();
        }

        private struct SurrogateData
        {
            public ISerializationSurrogate Surrogate;
            public Type Type;
        }
    }
}
