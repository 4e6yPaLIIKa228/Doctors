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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TexBxLog.MaxLength = 30;
            TexBxOtchestv.MaxLength = 11;
        }

        private void BtBack_Click(object sender, RoutedEventArgs e)
        {
            Autoris a = new Autoris();
            a.Show();
            this.Close();
        }

        private void BtClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtReg_Click(object sender, RoutedEventArgs e)
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {

                if (String.IsNullOrEmpty(TexBxLog.Text) || String.IsNullOrEmpty(PassBx.Password) || String.IsNullOrEmpty(TexBxOtchestv.Text) || String.IsNullOrEmpty(BtFam.Text) || String.IsNullOrEmpty(BtFirst.Text) || String.IsNullOrEmpty(BtName.Text) || String.IsNullOrEmpty(TexBoxPolic.Text))
                {
                    MessageBox.Show("Заполните поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (TexBxLog.Text.Length <= 4)
                {
                    MessageBox.Show("Логин должен быть больше 4", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (PassBx.Password.Length <= 4)
                {
                    MessageBox.Show("Пароль должен быть больше 4", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (TexBxOtchestv.Text.Length != 11)
                {
                    MessageBox.Show("Телефон должен быть 11 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    connection.Open();
                    string query = $@"SELECT  COUNT(1) FROM  REGISTRS WHERE Login=@Login";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Login", TexBxLog.Text.ToLower());
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    int pr = 0;
                    if (count == 1)
                    {
                        MessageBox.Show("Login занят", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        pr = 1;
                    }
                    string query2 = $@"SELECT  COUNT(1) FROM Registrs WHERE Phone=@Phone";
                    SQLiteCommand cmd2 = new SQLiteCommand(query2, connection);
                    cmd2.Parameters.AddWithValue("@Phone", TexBxOtchestv.Text.ToLower());
                    int count2 = Convert.ToInt32(cmd2.ExecuteScalar());
                    int pr2 = 0;
                    if (count2 == 1)
                    {
                        MessageBox.Show("Телефон занят", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        pr2 = 1;
                    }
                    if (pr == 0 && pr2 == 0)
                    {
                        
                        {
                            string query3 = $@"INSERT INTO Registrs ('Login','Phone','Pass','Surname','Name','MiddleName','Policy') VALUES (@Login,@Phone,@Pass,@Surname,@Name,@MiddleName,@Policy)";
                            SQLiteCommand cmd3 = new SQLiteCommand(query3, connection);
                            try
                            {
                                cmd3.Parameters.AddWithValue("@Login", TexBxLog.Text.ToLower());
                                cmd3.Parameters.AddWithValue("@Pass", PassBx.Password);
                                cmd3.Parameters.AddWithValue("@Phone", TexBxOtchestv.Text.ToLower());
                                cmd3.Parameters.AddWithValue("@Surname", BtFam.Text.ToLower());
                                cmd3.Parameters.AddWithValue("@Name", BtName.Text.ToLower());
                                cmd3.Parameters.AddWithValue("@MiddleName", BtFirst.Text.ToLower());
                                cmd3.Parameters.AddWithValue("@Policy", TexBoxPolic.Text.ToLower());
                                cmd3.ExecuteNonQuery();
                                MessageBox.Show("Проверка пройдена. Аккаунт зарегистрирован.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                                Autoris Aftoriz = new Autoris();
                                this.Close();
                                Aftoriz.ShowDialog();
                            }

                            catch (SQLiteException ex)
                            {
                                MessageBox.Show("Ошибка" + ex);
                            }
                        }
                    }
                }
            }
        }
    }
}
