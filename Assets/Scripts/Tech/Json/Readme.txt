Read And Write Json File Has AES Encryption With Async And Not Async
JSON File Handling with AES Encryption
This system provides functionality for reading and writing JSON files with AES encryption, supporting both synchronous and asynchronous operations.

Features:
Supports primitive types (int, float, string, etc.) and complex data structures (List, Array, Dictionary, etc.).
Uses AES encryption for secure data storage.
[JsonIgnore] attribute allows excluding specific variables from serialization.
Supports custom serialization by inheriting from JsonConverter<T> and applying [JsonConverter(typeof(YourCustomData))] to a class.
DataService reads JSON files and caches data for optimized access.

Auto-detects save path depending on the environment (Editor vs. Build)

Example:
    #if UNITY_EDITOR
    public const string SavePath = "Assets/.../FileName.json";
    #else
    public const string SavePath = Application.persistentDataPath + "/.../FileName.json";
    #endif
