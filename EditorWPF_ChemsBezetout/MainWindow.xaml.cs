using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EditorWPF_ChemsBezetout
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        String filePath;
        String selectedPath;
        Microsoft.Win32.OpenFileDialog OpenFiles;
        Microsoft.Win32.SaveFileDialog saveFileDialog1;
        ColorDialog colorDialog1;
        FontDialog fontDialog1;

        public MainWindow()
        {
            InitializeComponent();
            richTextBox1.Document.Blocks.Clear();

        }


        /**
         * Muestra para seleccionar un fichero y lo añade a la lista
         */
        private void ShowFiles_Click(object sender, RoutedEventArgs e)
        {

            OpenFiles = new Microsoft.Win32.OpenFileDialog();

            //Preparo el openFile con las opciones necesarias
            OpenFiles.InitialDirectory = "c:\\"; //Primer directorio
            OpenFiles.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*|rtf files (*.rtf)|*.rtf"; //Filtra ficheros de texto y rtf
            OpenFiles.FilterIndex = 2;
            OpenFiles.RestoreDirectory = true;

            if (OpenFiles.ShowDialog() == true)
            {
   
                filePath = OpenFiles.FileName;
                rutas.Items.Add(filePath);

            }
        }


        /**
         * Permite abrir un nuevo texto
         */
        private void Open_Click(object sender, EventArgs e)
        {

            OpenFiles = new Microsoft.Win32.OpenFileDialog();

            //Preparo el openFile con las opciones necesarias
            OpenFiles.InitialDirectory = "c:\\"; //Primer directorio
            OpenFiles.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*|rtf files (*.rtf)|*.rtf"; //Filtra ficheros de texto y rtf
            OpenFiles.FilterIndex = 2;
            OpenFiles.RestoreDirectory = true;

            if (OpenFiles.ShowDialog() == true)
            {
                //Get the path of specified file
                filePath = OpenFiles.FileName;
                
                rutas.Items.Add(filePath);
                selectedPath = filePath.ToString();
                System.Windows.MessageBox.Show("Fichero cargado con exito");
                leeArchivo();

            }


        }

        /**
         * Introducie el contenido del fichero seleccionado en el editor
         */
        private void Rutas_DoubleClick(object sender, EventArgs e)
        {


            if (rutas.SelectedItem != null)
            {
                selectedPath = rutas.SelectedItem.ToString();
                leeArchivo();
            }

         

        }

        /**
         *Añade al editor el texto del fichero especificado
         */
        private void leeArchivo()
        {

            TextRange range;

            FileStream fStream;

            if (File.Exists(selectedPath))
            {

                range = new TextRange(richTextBox1.Document.ContentStart, richTextBox1.Document.ContentEnd);

                fStream = new FileStream(selectedPath, FileMode.OpenOrCreate);

                if (filePath.EndsWith(".txt"))
                {
                    range.Load(fStream, System.Windows.DataFormats.Text);

                }
                else if (filePath.EndsWith(".rtf"))
                {
                    range.Load(fStream, System.Windows.DataFormats.Rtf);
                }


                fStream.Close();

            }


        }

        /**
         * Comprueba si esta seleccionado el checkBox
         */
        private void ShowMenu_CheckedChanged(object sender, EventArgs e)
        {


            if (activarConfig.IsChecked==true)
            {
                Options.IsEnabled = true;
            }
            else
            {
                Options.IsEnabled = false;
            }

        }


        /**
         * Permite copiar un texto seleccionacdo
         */
        private void Copy_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        /**
         * Permite cortar un texto seleccionado
         */
        private void Cut_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        /**
         * Permite pegar un texto copiado o cortado previamente
         */
        private void Paste_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }



        /**
         * Permite cambiar la fuente de un texto seleccionado
         */
        private void FontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog1 = new FontDialog();
            if (fontDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Debug.WriteLine(fontDialog1.Font);

                richTextBox1.FontFamily = new FontFamily(fontDialog1.Font.Name);
                richTextBox1.FontSize = fontDialog1.Font.Size * 96.0 / 72.0;
                richTextBox1.FontWeight = fontDialog1.Font.Bold ? FontWeights.Bold : FontWeights.Regular;
                richTextBox1.FontStyle = fontDialog1.Font.Italic ? FontStyles.Italic : FontStyles.Normal;
            }
        }



        /**
        * Permite cambiar el color de un texto seleccionado
        */
        private void ColorToolStripMenuItem_Click(object sender, EventArgs e)
        {

            colorDialog1 = new ColorDialog();
            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                richTextBox1.Foreground = new SolidColorBrush(Color.FromArgb(colorDialog1.Color.A, colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B));
            }
        }



        /**
         * Cerrar aplicación
         */
        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        /**
         * En caso de haber contenido en el editor deja seleccionar al usuario donde guardarlo
         */
        private void nuevoFile(object sender, EventArgs e)
        {

            string richText = new TextRange(richTextBox1.Document.ContentStart, richTextBox1.Document.ContentEnd).Text;

            if (richText.Length != 0)
            {
                saveFileDialog1 = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog1.InitialDirectory = "c:\\"; //Primer directorio
                saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*|rtf files (*.rtf)|*.rtf";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == true)
                {
                    filePath = saveFileDialog1.FileName;
                    selectedPath = filePath;
                    File.WriteAllText(filePath, richText);
                    rutas.Items.Add(filePath);
                    System.Windows.MessageBox.Show("Archivo creado con exito, link anyadido en MOSTRAR");
                }
                else
                {
                  
                }

            }


        }

        /**
         * Una vez seleccionado el fichero y una vez abierto, en caso de ser editado guarda el conenido dentro de este
         */
        private void guardarTexto(object sender, EventArgs e)
        {
            string richText = new TextRange(richTextBox1.Document.ContentStart, richTextBox1.Document.ContentEnd).Text;

            if (richText.Length > 0 && selectedPath!= null)
            {
                File.WriteAllText(selectedPath, richText);
                System.Windows.MessageBox.Show("Archivo guardado con exito");
            }


        }


        /**
         * Cerrar aplicación Preguntando antes de salir
         */
        private void Form_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Estas seguro que quieres salir?", "Salir", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {

            }
            else if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }


        /**
         * Muestra informacion adicional sobre la aplicacion
         */
        private void info(object sender, EventArgs e)
        {
            Info info = new Info();
            info.ShowDialog();

        }


    }
}
