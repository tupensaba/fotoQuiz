using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using MessageBox = System.Windows.Forms.MessageBox;

namespace FaceQuiz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool quizStart = true;

        private string name = "";
        private string surname = "";
        private int _count = 1;
        private List<string> _files;
        public List<string> _approvedImg = new List<string>() {""};
        public string file;

        public MainWindow()
        {
            InitializeComponent();
        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            start.Visibility = Visibility.Collapsed;


            quizStart = true;

            using (var fbd = new FolderBrowserDialog())
            {

                DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    var files = Directory.EnumerateFiles(fbd.SelectedPath, "*.*", SearchOption.AllDirectories)
                        .Where(s => s.EndsWith(".bnp") || s.EndsWith(".jpg") || s.EndsWith(".png")).ToList();

                    _files = files;
                    
                    uploadImg();
                }
                Name.Visibility = Visibility.Visible;

                Counters.Visibility = Visibility.Visible;

                Answer.Visibility = Visibility.Visible;

                menu.Visibility = Visibility.Visible;

                foto.Visibility = Visibility.Visible;

                imya.Visibility = Visibility.Visible;

                famlya.Visibility = Visibility.Visible;
            }
        }

        private void Answer_Click(object sender, RoutedEventArgs e)
        {

            if (Name.Text != "" && Surname.Text != "")
                if (Name.Text.ToLower() == name.ToLower() && Surname.Text.ToLower() == surname.ToLower())
                {
                    uploadImg(++_count);
                    Name.Text = "";
                    Surname.Text = "";
                }
                else
                    MessageBox.Show("Неправильный ответ");
            else if(Name.Text != "" && Surname.Text == "")
            {
                if (Name.Text.ToLower() == name.ToLower())
                {
                    if (Name.Text.ToLower() == name.ToLower())
                    {
                        uploadImg(++_count);
                        Name.Text = "";
                        Surname.Text = "";

                    }
                    else
                        MessageBox.Show("Неправильный ответ");
                }
                else if(surname != "")
                  MessageBox.Show("Введите фамилию");
            }
            else if(Name.Text == "")
            {
                MessageBox.Show("Введите имя");
            }
                    
        }

        public void uploadImg( int count = 1)
        {
          if (count > _files.Count())
            {
               var result = MessageBox.Show("Все отгадали! Вы молодец!:)\n Хотите ещё?","УРА!", MessageBoxButtons.YesNo);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    start.Visibility = Visibility.Visible;

                    Name.Visibility = Visibility.Collapsed;

                    Surname.Visibility = Visibility.Collapsed;

                    Counters.Visibility = Visibility.Collapsed;

                    Answer.Visibility = Visibility.Collapsed;

                    menu.Visibility = Visibility.Collapsed;
                    
                    foto.Visibility = Visibility.Collapsed;

                    imya.Visibility = Visibility.Collapsed;

                    famlya.Visibility = Visibility.Collapsed;

                    quizStart = false;

                    _files.Clear();
                    _approvedImg.Clear();

                    Counters.Content = "";
                    _count = 1;
                    return;
                }
                else if (result == System.Windows.Forms.DialogResult.No || result == System.Windows.Forms.DialogResult.None)
                {
                    Close();
                }
            }

            bool itNotWas = false;

            Random random = new Random();

            while (!itNotWas)
            {
               file = _files[random.Next(0,_files.Count())];
                if( _approvedImg != null)
                {
                    if(!_approvedImg.Contains(file))
                    {
                        _approvedImg.Add(file);
                        itNotWas = true;
                        
                    }
                    
                }
                else if (_approvedImg == null)
                {
                    _approvedImg.Append(file);
                    itNotWas = true;
                    
                }
            }
            

            foto.Source = new BitmapImage(new Uri(file));

            name = file.Substring(file.LastIndexOf("\\") + 1);
            if(name.Contains("-"))
            {
                surname = name.Substring(name.LastIndexOf("-") + 1);
                surname = surname.Remove(surname.LastIndexOf("."));
                Surname.Visibility = Visibility.Visible;
                name = name.Remove(name.LastIndexOf("-"));
            }
            else
            {
                name = name.Remove(name.LastIndexOf("."));
                surname = "";
                Surname.Visibility = Visibility.Hidden;
            }
            
            


            Counters.Content = $"{count}" + " из " + _files.Count().ToString();
        }



        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == Key.Enter && quizStart)
            {
                if (Name.Text != "" && Surname.Text != "")
                    if (Name.Text.ToLower() == name.ToLower() && Surname.Text.ToLower() == surname.ToLower())
                    {
                        uploadImg(++_count);
                        Name.Text = "";
                        Surname.Text ="";

                    }
                    else
                        MessageBox.Show("Неправильный ответ");
                else if (Name.Text != "" && Surname.Text == "")
                {
                    if ( Name.Text != "" && surname == "")
                    {
                        if (Name.Text.ToLower() == name.ToLower())
                        {
                            uploadImg(++_count);
                            Name.Text = "";
                            Surname.Text = "";
                        }
                        else
                            MessageBox.Show("Неправильный ответ");
                    }
                    else if (surname != "")
                        MessageBox.Show("Введите фамилию");
                }
                else if (Name.Text == "")
                {
                    MessageBox.Show("Введите имя");
                }

            }
        }
    }
}
