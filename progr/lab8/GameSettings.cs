using System.IO;
using System.Runtime.Serialization.Json;
using System.Windows;

namespace PuzzleGame
{
    public static class GameSettings
    {
        private static string SettingsFile = "game_settings.json";
        private static GameSettingsData _settings = new GameSettingsData();

        static GameSettings()
        {
            LoadSettings();
        }

        public static string Level1Time
        {
            get => _settings.Level1Time;
            set
            {
                _settings.Level1Time = value;
                SaveSettings();
            }
        }

        public static string Level2Time
        {
            get => _settings.Level2Time;
            set
            {
                _settings.Level2Time = value;
                SaveSettings();
            }
        }

        public static string Level3Time
        {
            get => _settings.Level3Time;
            set
            {
                _settings.Level3Time = value;
                SaveSettings();
            }
        }

        public static string Level4Time
        {
            get => _settings.Level4Time;
            set
            {
                _settings.Level4Time = value;
                SaveSettings();
            }
        }

        private static void LoadSettings()
        {
            if (File.Exists(SettingsFile))
            {
                try
                {
                    using (FileStream fs = new FileStream(SettingsFile, FileMode.Open))
                    {
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(GameSettingsData));
                        _settings = (GameSettingsData)serializer.ReadObject(fs);
                    }
                }
                catch
                {
                    // Если произошла ошибка при чтении, используем настройки по умолчанию
                    _settings = new GameSettingsData();
                }
            }
        }

        private static void SaveSettings()
        {
            try
            {
                using (FileStream fs = new FileStream(SettingsFile, FileMode.Create))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(GameSettingsData));
                    serializer.WriteObject(fs, _settings);
                }
            }
            catch
            {
                MessageBox.Show("Не удалось сохранить настройки игры", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        [System.Runtime.Serialization.DataContract]
        private class GameSettingsData
        {
            [System.Runtime.Serialization.DataMember]
            public string Level1Time { get; set; } = "";
            [System.Runtime.Serialization.DataMember]
            public string Level2Time { get; set; } = "";
            [System.Runtime.Serialization.DataMember]
            public string Level3Time { get; set; } = "";
            [System.Runtime.Serialization.DataMember]
            public string Level4Time { get; set; } = "";
        }
    }
}