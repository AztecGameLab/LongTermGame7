using ImGuiNET;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Rendering;
using Utility;
using Debug = UnityEngine.Debug;

namespace Application.Core
{
    public class Serializer
    {
        public List<SurrogateData> SurrogateList { get; }
        private Dictionary<string, object> _savedData;

        public Serializer()
        {
            SurrogateList = new List<SurrogateData>
            {
                new SurrogateData {Surrogate = new Vector3SerializationSurrogate(), Type = typeof(Vector3)},
                new SurrogateData {Surrogate = new QuaternionSerializationSurrogate(), Type = typeof(Quaternion)},
            };

            _savedData = new Dictionary<string, object>();

            ImGuiUtil.Register(DrawDebugUI);
        }

        private void DrawDebugUI()
        {
            ImGui.Begin("Serialization");

            var path = $"{UnityEngine.Application.persistentDataPath}/Saves";
            
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

        public static string GetPath(string fileName)
        {
            string path = $"{UnityEngine.Application.persistentDataPath}/Saves";
            Directory.CreateDirectory(path);
            return $"{path}/{fileName}.dat";
        }
        
        public static bool IsValid(string fileName)
        {
            string savePath = GetPath(fileName);
            return File.Exists(savePath);
        }
        
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
                    surrogateData.Surrogate
                );
            }
            
            var binaryFormatter = new BinaryFormatter { SurrogateSelector = surrogateSelector };
            binaryFormatter.Serialize(saveFile, _savedData);
        }

        public void ReadFromDisk(string fileName)
        {
            string savePath = GetPath(fileName);
            FileStream fileStream;

            if (IsValid(fileName) == false)
            {
                Debug.LogWarning($"Tried to open the save \"{fileName}\" that doesn't exist! Creating a new file...");
                fileStream = File.Create(savePath);
            }
            else fileStream = File.OpenRead(savePath);
        
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
                        surrogateData.Surrogate
                    );
                }
            
                var binaryFormatter = new BinaryFormatter { SurrogateSelector = surrogateSelector };
                _savedData = (Dictionary<string, object>) binaryFormatter.Deserialize(fileStream);    
            }
            
            fileStream.Dispose();
        }

        public void ApplySavedDataTo(GameObject gameObject)
        {
            foreach (var serializable in gameObject.GetComponentsInChildren<ISerializable>())
            {
                ApplySavedDataTo(serializable);
            }
        }

        public void UpdateSavedDataFrom(GameObject gameObject)
        {
            foreach (var serializable in gameObject.GetComponentsInChildren<ISerializable>())
            {
                UpdateSavedDataFrom(serializable);
            }
        }

        public void ApplySavedDataTo(ISerializable serializable)
        {
            string id = serializable.GetID();
            
            if (_savedData.ContainsKey(id))
            {
                var data = _savedData[id];
                serializable.ReadData(data);
            }
        }

        public void UpdateSavedDataFrom(ISerializable serializable)
        {
            string id = serializable.GetID();

            if (!_savedData.ContainsKey(id))
            {
                _savedData.Add(id, serializable.WriteData());
            }
            else
            {
                _savedData[id] = serializable.WriteData();
            }
        }
    }
}