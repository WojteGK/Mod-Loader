using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MC_mods_installer
{
    public class Resources
    {
        private List<Resource> mods = new List<Resource>();
        private List<Resource> shaders = new List<Resource>();
        private List<Resource> textures = new List<Resource>();

        public List<Resource> Mods { get => mods; set => mods = value; }
        public List<Resource> Shaders { get => shaders; set => shaders = value; }
        public List<Resource> Textures { get => textures; set => textures = value; }
        public void LoadResources(string exePath)
        {            
            string resourcesJsonFilePath = Path.Combine(exePath, "resources.json");            
            try
            {
                if (!File.Exists(resourcesJsonFilePath))
                {
                    CreateDefaultResourcesJson(exePath);
                    Console.WriteLine($"File not found: {resourcesJsonFilePath}. Created default json instead: {resourcesJsonFilePath}.");  
                }
                string json = File.ReadAllText(resourcesJsonFilePath);
                var deserializedResources = JsonConvert.DeserializeObject<Resources>(json);
                if (deserializedResources != null){
                Mods = deserializedResources.Mods;
                Shaders = deserializedResources.Shaders;
                Textures = deserializedResources.Textures;
                }
                else{
                    throw new Exception("deserializedResources is null");
                }           
            }
            catch (NullReferenceException){
                Console.WriteLine($"Error: {resourcesJsonFilePath} is empty.");
            }
            catch (JsonReaderException){
                Console.WriteLine($"Error: {resourcesJsonFilePath} is empty or corrupted. Please, delete it and restart the program.");
            }
            catch (FileNotFoundException){
                Console.WriteLine($"Error: {resourcesJsonFilePath} is empty or corrupted. Please, delete it and restart the program.");
            }
            catch (DirectoryNotFoundException){
                Console.WriteLine($"Error: {resourcesJsonFilePath} is empty or corrupted. Please, delete it and restart the program.");
            }
            catch (UnauthorizedAccessException){
                Console.WriteLine($"Error: {resourcesJsonFilePath} is empty or corrupted. Please, delete it and restart the program.");
            }
            catch (PathTooLongException){
                Console.WriteLine($"Error: {resourcesJsonFilePath} is empty or corrupted. Please, delete it and restart the program.");
            }
            catch (NotSupportedException){
                Console.WriteLine($"Error: {resourcesJsonFilePath} is empty or corrupted. Please, delete it and restart the program.");
            }
            catch (ArgumentException){
                Console.WriteLine($"Error: {resourcesJsonFilePath} is empty or corrupted. Please, delete it and restart the program.");
            }
            catch (IOException){
                Console.WriteLine($"Error: {resourcesJsonFilePath} is empty or corrupted. Please, delete it and restart the program.");
            }
            catch (Exception ex){
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }                    
        }
        protected static void CreateDefaultResourcesJson(string exePath){
            var resources = new
            {
                mods = DefaultResources.Mods,
                shaders = DefaultResources.Shaders,
                textures = DefaultResources.Textures
            };
            string json = JsonConvert.SerializeObject(resources, Formatting.Indented);
            File.WriteAllText(Path.Combine(exePath, "resources.json"), json);
        }
    }
} 