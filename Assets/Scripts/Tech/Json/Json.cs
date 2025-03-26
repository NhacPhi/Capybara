using System;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using Cysharp.Threading.Tasks;

namespace Tech.Json
{
    public static class Json 
    {
        private static readonly string _key = "gCjK+DZ/GCYbKIGiAt1qCA==";
        private static readonly string _iv = "47l5QsSe1POo31adQ/u7nQ==";

        private static JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented
        };

        //public static IEncryption Encryption = new AES(_key, _iv);
        public static IEncryption Encryption;
        
        public static void SaveJson<T>(this T data, string path)
        {
            try{
                string json = JsonConvert.SerializeObject(data, settings);
                WriteAllText(path, json);
    #if UNITY_EDITOR
                AssetDatabase.Refresh();
    #endif
            }
            catch
            {
                // ignored
            }
        }
    
        public static async void SaveJsonAsync<T>(this T data, string path, Action saveDone = null)
        {
            try{
                await UniTask.RunOnThreadPool(async () =>
                {
                    string json = JsonConvert.SerializeObject(data, settings);
                    await WriteAllTextAsync(path, Encryption.Encrypt(json));
                });

                saveDone?.Invoke();
    #if UNITY_EDITOR
                AssetDatabase.Refresh();
    #endif
            }
            catch
            {
                // ignored
            }
        }
        
        public static void LoadJson<T>(string path, out T value)
        {
            try
            {
                if (File.Exists(path))
                {
                    string json = ReadAllText(path);
                    T data = JsonConvert.DeserializeObject<T>(json, settings);
                    value = data;
                    return;
                }
            }
            catch
            {
                // ignored
            }


            value = default;
        }

        private static async UniTask WriteAllTextAsync(string path, string text)
        {
            if (Encryption != null)
            {
                await File.WriteAllTextAsync(path, JsonConvert.SerializeObject(Encryption.Encrypt(text), settings));
                return;
            }

            await File.WriteAllTextAsync(path, text);
        }

        private static void WriteAllText(string path, string text)
        {
            if (Encryption != null)
            {
                File.WriteAllText(path, JsonConvert.SerializeObject(Encryption.Encrypt(text), settings));
                return;
            }

            File.WriteAllText(path, text);
        }
        
        
        public static string ReadAllText(string path)
        {
            if (Encryption != null)
            {
                return Encryption.Decrypt(JsonConvert.DeserializeObject<string>(File.ReadAllText(path), settings));
            }

            return File.ReadAllText(path);
        }

        public static T DeserializeObject<T>(string json)
        {
            var newJson = json;
            
            if (Encryption != null)
            {
                newJson = Encryption.Decrypt(json);
            }
            
            return JsonConvert.DeserializeObject<T>(newJson, settings);
        }

        public static string SerializeObject<T>(T data)
        {
            var json = JsonConvert.SerializeObject(data, settings);
            
            if (Encryption != null)
            {
                json = Encryption.Encrypt(json);
            }

            return json;
        }
    }
}
