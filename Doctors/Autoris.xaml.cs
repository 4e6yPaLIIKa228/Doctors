using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using Doctors.Connection;
using System.Data;
using System.IO;
using System.Drawing;
using System.Net.Mail;
using System.Net;

namespace Doctors
{
    /// <summary>
    /// Логика взаимодействия для Autoris.xaml
    /// </summary>
    public partial class Autoris : Window
    {
        public Autoris()
        {
            InitializeComponent();
        }

        private void BtBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow a = new MainWindow();
            a.Show();
            this.Close();
        }

        private void BtExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtEnter_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(TexBxLog.Text) || String.IsNullOrEmpty(PassBx.Password))
            {
                MessageBox.Show("Заполните поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
                    try
                    {
                        connection.Open();
                        string query = $@"SELECT  COUNT(1) FROM Registrs WHERE Login=@Login AND Pass=@Pass";
                        SQLiteCommand cmd = new SQLiteCommand(query, connection);
                        string LoginLower = TexBxLog.Text.ToLower();
                        cmd.Parameters.AddWithValue("@Login", TexBxLog.Text.ToLower());
                        cmd.Parameters.AddWithValue("@Pass", PassBx.Password);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        if (count == 1)
                        {
                            string query2 = $@"SELECT ID FROM Registrs WHERE Login=@Login";
                            SQLiteCommand cmd2 = new SQLiteCommand(query2, connection);
                            cmd2.Parameters.AddWithValue("@Login", TexBxLog.Text.ToLower());
                            int countID = Convert.ToInt32(cmd2.ExecuteScalar());
                            //Saver.Login = TbLogin.Text.ToLower();
                            //Saver.ID = countID;
                            connection.Close();
                            MessageBox.Show("Добро пожаловать!");
                            Menu menu = new Menu();
                            menu.Show();
                            this.Close();

                        }
                        else
                        {
                            MessageBox.Show("Неверное имя пользователя или пароль");
                        }
                    }
                    catch (SQLiteException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
            }
        }
    }
    
}
