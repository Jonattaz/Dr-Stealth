using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

#if UNITY_WEBGL
// Needed for WebGL
using System.Runtime.InteropServices;
#endif

namespace Mz.Serializers {
    public enum DataFormat {
        Binary,
        Json,
        Xml
    }

    public static class Data {
#if UNITY_WEBGL
        [DllImport("__Internal")] 
        private static extern void SyncFiles();
#endif
        
        public static string ApplicationDirectory {
            get {
                /*
                 * Environment.CurrentDirectory is not recommended
                 * AppDomain.CurrentDomain.BaseDirectory returns unexpected results during testing
                 */
                var appDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                appDirectory = appDirectory.Replace("file:", "");
                return appDirectory;
            }
        }
        private static string _baseDirectory ;
        public static string BaseDirectory {
            get => _baseDirectory ?? ApplicationDirectory;
            set => _baseDirectory = value;
        }
        
        private static string _dataSubDirectory ;
        public static string DataSubDirectory {
            get => _dataSubDirectory ?? "";
            set => _dataSubDirectory = value;
        }
        
        private static string _fileExensionDefault;
        public static string FileExtension {
            get => _fileExensionDefault ?? "dat";
            set => _fileExensionDefault = value;
        }
        
        public static bool SerializeToStream(
            object instance, 
            Stream stream, 
            DataFormat format = DataFormat.Json, 
            bool isPrettyPrint = false,
            bool isThrowExceptions = false
        ) {
            var isSuccess = true;

            // No need to Close() or Dispose() the stream writers.
            // The reason this class is disposable is not because it holds unmanaged resources,
            // but to allow the disposal of the stream which itself could hold unmanaged resources.
            // If the life of the underlying stream is handled elsewhere, no need to dispose the writer.

            try {
                switch (format) {
                    case DataFormat.Binary:
                        var serializerBinary = new BinaryFormatter();
                        serializerBinary.Serialize(stream, instance);
                        break;
                    case DataFormat.Json:
                        var json = SerializerJson.Serialize(instance, true, isPrettyPrint, false);
                        var streamWriterJson = new StreamWriter(stream);
                        streamWriterJson.Write(json);
                        streamWriterJson.Flush();
                        break;
                    case DataFormat.Xml:
                        XmlTextWriter writerXml;
                        var serializerXml = new XmlSerializer(instance.GetType());
                        writerXml = new XmlTextWriter(stream, new UTF8Encoding());
                        writerXml.Formatting = Formatting.Indented;
                        writerXml.IndentChar = ' ';
                        writerXml.Indentation = 4;
                        serializerXml.Serialize(writerXml, instance);
                        break;
                }
            } catch (Exception ex) {
                isSuccess = false;
                Console.Write("SerializeObject failed with : " + ex.GetBaseException().Message + "\r\n" + (ex.InnerException != null ? ex.InnerException.Message : ""), "Mz.Serializer");
                if (isThrowExceptions) throw;
            }

#if UNITY_WEBGL
            if (Application.platform == RuntimePlatform.WebGLPlayer && isSuccess)
            {
                SyncFiles();
            }
#endif

            return isSuccess;
        }
        
        /// <summary>
        /// Serialize the specified <paramref name="instance"/> to <paramref name="directoryPath"/> with the given <paramref name="fileName"/>.
        /// The user must ensure that the directory already exists, or an error will be thrown.
        /// </summary>
        public static SerializeResult Serialize(
            object instance,
            string fileName,
            string fileExtension = null,
            string directoryPath = null,
            bool isRelativePath = true,
            DataFormat format = DataFormat.Json,
            bool isPrettyPrint = false,
            bool isThrowExceptions = false
        ) {
            bool isSuccess;
            var serializeResult = new SerializeResult();
            string filePath;

            try {
                if (string.IsNullOrEmpty(fileName)) {
                    if (isThrowExceptions) throw new ArgumentException("Missing file name.");
                    return serializeResult;
                }
                
                if (!string.IsNullOrEmpty(directoryPath) && isRelativePath) directoryPath = Path.Combine(BaseDirectory, directoryPath);

                var fileNameSplit = fileName.Split();
                if (fileNameSplit.Length > 1) {
                    fileName = fileNameSplit[0];
                    fileExtension = fileNameSplit[1];
                }
                
                // We can't call Directory.CreateDirectory() here without running into permission issues,
                // so we'll have to defer to the user of the method to ensure that the directory has been created.
                filePath = Path.Combine(directoryPath ?? Path.Combine(BaseDirectory, DataSubDirectory), $"{fileName}.{fileExtension ?? FileExtension}");
                
                Stream stream;
                if (File.Exists(filePath))
                {
                    File.WriteAllText(filePath, string.Empty);
                    stream = File.Open(filePath, FileMode.Open);
                }
                else
                {
                    // stream = File.Create(filePath);
                    stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Write);
                }
              
                using (stream) {
                    isSuccess = SerializeToStream(instance, stream, format, isPrettyPrint, isThrowExceptions);
                }
            } catch (Exception ex) {
                Console.Write("Serialize failed with : " + ex.GetBaseException().Message + "\r\n" + (ex.InnerException != null ? ex.InnerException.Message : ""), "Mz.Serializer");
                if (isThrowExceptions) throw;
                return serializeResult;
            }
            
            serializeResult.IsSuccess = isSuccess;
            serializeResult.FilePath = filePath;
            return serializeResult;
        }

        public static bool SerializeToString(
            object instance, 
            out string resultString, 
            DataFormat format,
            bool isPrettyPrint = false,
            bool isThrowExceptions = false
        ) {
            bool isSuccess;
            resultString = string.Empty;

            using (var stream = new MemoryStream()) {
                isSuccess = SerializeToStream(instance, stream, format, isPrettyPrint, isThrowExceptions);
                if (isSuccess) resultString = Encoding.UTF8.GetString(stream.ToArray(), 0, (int)stream.Length);
            }
           
            return isSuccess;
        }

        public static bool SerializeToByteArray(object instance, out byte[] resultBuffer, bool isThrowExceptions = false) {
            resultBuffer = Array.Empty<byte>();
            var stream = new MemoryStream();
            var isSuccess = SerializeToStream(instance, stream, DataFormat.Binary, false, isThrowExceptions);
            if (isSuccess) resultBuffer = stream.ToArray();
            return isSuccess;
        }

        public static bool DeserializeFromStream<TObject>(
            out TObject instance,
            Stream stream, 
            DataFormat format = DataFormat.Json,
            bool isThrowExceptions = false
        ) {
            var isSuccess = true;
            instance = default;

            try {
                switch (format) {
                    case DataFormat.Binary:
                        var serializerBinary = new BinaryFormatter();
                        instance = (TObject)serializerBinary.Deserialize(stream);
                        break;
                    case DataFormat.Json:
                        var streamReaderJson = new StreamReader(stream);
                        instance = DeserializerJson.ToObject<TObject>(streamReaderJson);
                        streamReaderJson.Close();
                        break;
                    case DataFormat.Xml:
                        var serializerXml = new XmlSerializer(typeof(TObject));
                        instance = (TObject)serializerXml.Deserialize(stream);
                        break;
                }
            } catch (Exception ex) {
                isSuccess = false;
                Console.Write("SerializeObject failed with : " + ex.GetBaseException().Message + "\r\n" + (ex.InnerException != null ? ex.InnerException.Message : ""), "Mz.Serializer");
                if (isThrowExceptions) throw;
            }

            return isSuccess;
        }

        public static bool FileToString(
            out string text,
            string fileName,
            string fileExtension = null,
            string directoryPath = null,
            bool isRelativePath = true,
            bool isThrowExceptions = false
        )
        {
            text = "";
            var textFromStream = "";

            Action<Stream> action = stream =>
            {
                var streamReaderText = new StreamReader(stream);
                textFromStream = streamReaderText.ReadToEnd();
                streamReaderText.Close();
            };

            var isSuccess = ProcessStream(action, fileName, fileExtension, directoryPath, isRelativePath, isThrowExceptions);

            text = textFromStream;
            return isSuccess;
        }
        
        public static bool ProcessStream(
            Action<Stream> action,
            string fileName,
            string fileExtension = null,
            string directoryPath = null,
            bool isRelativePath = true,
            bool isThrowExceptions = false
        )
        {
            try {
                if (string.IsNullOrEmpty(fileName)) {
                    if (isThrowExceptions) throw new ArgumentException("Missing file name.");
                    return false;
                }

                if (!string.IsNullOrEmpty(directoryPath) && isRelativePath) directoryPath = Path.Combine(BaseDirectory, directoryPath);
                
                var fileNameSplit = fileName.Split();
                if (fileNameSplit.Length > 1) {
                    fileName = fileNameSplit[0];
                    fileExtension = fileNameSplit[1];
                }

                var filePath = Path.Combine(directoryPath ?? Path.Combine(BaseDirectory, DataSubDirectory), $"{fileName}.{fileExtension ?? FileExtension}");

                // It's important to use File.OpenRead(filePath) here, rather than FileStream(filePath, FileMode.Open),
                // to avoid write access errors on some platforms, like iOS.
                using (Stream stream = File.OpenRead(filePath)) {
                    action.Invoke(stream);
                }
                
                return true;
            } catch (Exception ex) {
                Console.Write("Failed to read file : " + ex.GetBaseException().Message + "\r\n" + (ex.InnerException != null ? ex.InnerException.Message : ""), "Mz.Serializers.Data");
                if (isThrowExceptions) throw;
                return false;
            }
        }
        
        public static bool Deserialize<TObject>(
            out TObject instance,
            string fileName,
            string fileExtension = null,
            string directoryPath = null,
            bool isRelativePath = true,
            DataFormat format = DataFormat.Json,
            bool isThrowExceptions = false
        )
        {
            instance = default;

            var isSuccessDeserialize = false;
            var instanceDeserialize = default(TObject);
            Action<Stream> action = stream =>
            {
                isSuccessDeserialize = DeserializeFromStream(out instanceDeserialize, stream, format, isThrowExceptions);
            };

            var isSuccess = ProcessStream(action, fileName, fileExtension, directoryPath, isRelativePath, isThrowExceptions);
            instance = instanceDeserialize;
            
            return isSuccessDeserialize && isSuccess;
        }

        public static bool DeserializeFromString<TObject>(out TObject instance, string inputString, DataFormat format, bool isThrowExceptions = false) {
            bool isSuccess;

            using (var stream = new MemoryStream()) {
                var writer = new StreamWriter(stream);
                writer.Write(inputString);
                writer.Flush();
                stream.Position = 0;
                isSuccess = DeserializeFromStream(out instance, stream, format, isThrowExceptions);
            }

            return isSuccess;
        }

        public static bool DeserializeFromByteArray<TObject>(out TObject instance, byte[] buffer, bool isThrowExceptions = false) {
            bool isSuccess;

            using (var stream = new MemoryStream(buffer)) {
                isSuccess = DeserializeFromStream(out instance, stream, DataFormat.Binary, isThrowExceptions);
            }

            return isSuccess;
        }
    } 
}
