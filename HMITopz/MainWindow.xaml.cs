using Microsoft.Win32;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace HMIDTopz
{
    public partial class MainWindow : Window
    {
        private string _xmlContent;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    _xmlContent = File.ReadAllText(openFileDialog.FileName);
                    ProcessXmlContent();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_xmlContent == null)
            {
                MessageBox.Show("No mimic content loaded.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Remove XML Declaration if it exists
            _xmlContent = RemoveXmlDeclaration(_xmlContent);

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, _xmlContent);
                    MessageBox.Show($"File saved to {saveFileDialog.FileName}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private string RemoveXmlDeclaration(string xmlContent)
        {
            // This regex removes the XML declaration if it is present
            return Regex.Replace(xmlContent, @"^\s*<\?xml.*?\?>\s*", string.Empty, RegexOptions.Multiline);
        }

        //private void SaveButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (_xmlContent == null)
        //    {
        //        MessageBox.Show("No mimic content loaded.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }

        //    SaveFileDialog saveFileDialog = new SaveFileDialog();

        //    if (saveFileDialog.ShowDialog() == true)
        //    {
        //        try
        //        {
        //            File.WriteAllText(saveFileDialog.FileName, _xmlContent);
        //            MessageBox.Show($"File saved to {saveFileDialog.FileName}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //    }
        //}

        private void ProcessXmlContent()
        {
            // Step 1: Linearize the XML
            _xmlContent = LinearizeXml(_xmlContent);

            // Step 2: Pretty print the XML
            _xmlContent = PrettyPrintXml(_xmlContent);

            // Step 3: Delete unnecessary property settings
            _xmlContent = DeleteUnnecessaryProperties(_xmlContent);

            // Step 4: Change av:DynamicResource to av:StaticResource
            _xmlContent = ChangeDynamicToStatic(_xmlContent);

            // Step 5: Remove default scale/rotate transforms
            _xmlContent = RemoveTransforms(_xmlContent);

            // Step 6: Round off decimal values
            _xmlContent = RoundOffValues(_xmlContent);
        }

        private string LinearizeXml(string xmlContent)
        {
            return Regex.Replace(xmlContent, @"\s+", " ");
        }

        private string PrettyPrintXml(string xmlContent)
        {
            try
            {
                var doc = new System.Xml.XmlDocument();
                doc.LoadXml(xmlContent);

                var stringBuilder = new System.Text.StringBuilder();
                var settings = new System.Xml.XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  ",
                    NewLineOnAttributes = false
                };

                using (var xmlWriter = System.Xml.XmlWriter.Create(stringBuilder, settings))
                {
                    doc.Save(xmlWriter);
                }

                return stringBuilder.ToString();
            }
            catch
            {
                return xmlContent; // Qualuqer coisa volta o conteudo original
            }
        }

        private string DeleteUnnecessaryProperties(string xmlContent)
        {
            string[] propertiesToRemove =
            {
                "Background=\"{av:DynamicResource Green}\"",
                "BorderBrush=\"{av:DynamicResource Red}\"",
                "BorderThickness=\"1,1,1,1\"",
                "Foreground=\"{av:DynamicResource Black}\"",
                "Foreground=\"{av:DynamicResource Gray}\"",
                "ContextMenu=\"{x:Null}\"",
                "av:KeyboardNavigation.IsTabStop=\"False\"",
                "IsTabStop=\"False\"",
                "ClipToBounds=\"False\"",
                "Branches=\"\"",
                "RenderTransformOrigin=\"0,0\"",
                "; Server = $Server$",
                "SymbolType=\"Master\"",
                "; Server =",
                "KeepDefaultSize=\"True\"",
                "; TrackCircuit = ; OutOfService =",
                "HorizontalContentAlignment=\"Left\""

            };

            foreach (var prop in propertiesToRemove)
            {
                xmlContent = xmlContent.Replace(prop, "");
            }

            return xmlContent;
        }

        private string ChangeDynamicToStatic(string xmlContent)
        {
            return xmlContent.Replace("av:DynamicResource", "av:StaticResource");
        }

        private string RemoveTransforms(string xmlContent)
        {
            string pattern = @"<AlsA_SymbolControl([^>]*?)>\s*<AlsA_SymbolControl.RenderTransform>.*?</AlsA_SymbolControl.RenderTransform>\s*</AlsA_SymbolControl>";
            string replacement = @"<AlsA_SymbolControl$1/>";
            xmlContent = Regex.Replace(xmlContent, pattern, replacement, RegexOptions.Singleline);

            return xmlContent;
        }

        private string RoundOffValues(string xmlContent)
        {
            string pattern = @"(av:Canvas\.(?:Left|Top|Height|Width)=""\d+)\.\d*""";
            return Regex.Replace(xmlContent, pattern, "$1\"");
        }
    }
}

