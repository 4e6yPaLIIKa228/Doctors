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
            TexBoxPolic.MaxLength = 16;
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

                if (String.IsNullOrEmpty(TexBxLog.Text) || String.IsNullOrEmpty(PassBx.Password) || String.IsNullOrEmpty(TexBxOtchestv.Text) || String.IsNullOrEmpty(BtFam.Text) || String.IsNullOrEmpty(TexBoxMail.Text) || String.IsNullOrEmpty(TexBoxPolic.Text))
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
                else if (TexBoxPolic.Text.Length != 16)
                {
                    MessageBox.Show("Полис должен быть равен 16 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                else
                {
                    connection.Open();
                    string query = $@"SELECT  COUNT(1) FROM  USERS WHERE Login=@Login";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Login", TexBxLog.Text.ToLower());
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    int pr = 0;
                    if (count == 1)
                    {
                        MessageBox.Show("Login занят", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        pr = 1;
                    }
                    string query2 = $@"SELECT  COUNT(1) FROM USERS WHERE Phone=@Phone";
                    SQLiteCommand cmd2 = new SQLiteCommand(query2, connection);
                    cmd2.Parameters.AddWithValue("@Phone", TexBxOtchestv.Text.ToLower());
                    int count2 = Convert.ToInt32(cmd2.ExecuteScalar());
                    int pr2 = 0;
                    if (count2 == 1)
                    {
                        MessageBox.Show("Телефон занят", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        pr2 = 1;
                    }
                    string query4 = $@"SELECT  COUNT(1) FROM USERS WHERE Mail=@Mail";
                    SQLiteCommand cmd4 = new SQLiteCommand(query4, connection);
                    cmd2.Parameters.AddWithValue("@Mail", TexBoxMail.Text.ToLower());
                    int count4 = Convert.ToInt32(cmd2.ExecuteScalar());
                    int pr3 = 0;
                    if (count2 == 1)
                    {
                        MessageBox.Show("Mail занят", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        pr2 = 1;
                    }
                    if (pr == 0 && pr2 == 0 && pr3 == 0)
                    {
                        try
                        {
                            SmtpClient Smtp = new SmtpClient("smtp.mail.ru", 25);
                            Smtp.UseDefaultCredentials = true;
                            Smtp.EnableSsl = true;
                            Smtp.Credentials = new NetworkCredential("fujiwara_1990@mail.ru", "5cFZ8MB4w731QH9zNAVW");
                            MailMessage Message = new MailMessage();
                            Message.From = new MailAddress("fujiwara_1990@mail.ru");
                            Message.To.Add(new MailAddress(TexBoxMail.Text));
                            Message.To.Add(new MailAddress(TexBoxMail.Text));
                            Message.Subject = "Учёт записей к врачу.";
                            Message.Body = "Успешная регистрация. На это сообще не нужно отвечать.";
                            Smtp.Send(Message);
                            //MessageBox.Show("На вашу почту выслан код для проверки, введите его, чтобы продолжить", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            Saver.Proverka = 0;

                        }
                        catch (FormatException)
                        {
                            MessageBox.Show("Введенная почта некорректна.");
                            Saver.Proverka = 1;
                        }
                        catch (SmtpFailedRecipientsException)
                        {
                            MessageBox.Show("Введенная почта некорректна.");
                            Saver.Proverka = 1;
                        }
                        if (Saver.Proverka == 0)
                        {
                            string query3 = $@"INSERT INTO USERS ('Famil','Phone','Policy','Login','Pass','Mail') VALUES (@Famil,@Phone,@Policy,@Login,@Pass,@Mail)";
                            SQLiteCommand cmd3 = new SQLiteCommand(query3, connection);
                            try
                            {
                                byte[] Pass;
                                Сipher f = new Сipher();
                                Pass = f.GetHashPassword(PassBx.Password);
                                cmd3.Parameters.AddWithValue("@Famil", BtFam.Text.ToLower());
                                cmd3.Parameters.AddWithValue("@Phone", TexBxOtchestv.Text.ToLower());
                                cmd3.Parameters.AddWithValue("@Policy", TexBoxPolic.Text.ToLower());
                                cmd3.Parameters.AddWithValue("@Login", TexBxLog.Text.ToLower());
                                cmd3.Parameters.AddWithValue("@Pass", Pass);
                                cmd3.Parameters.AddWithValue("@Mail", TexBoxMail.Text.ToLower());
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

        public int i = 0;
        private void BtFam_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = (Char.IsDigit(e.Text, 0)); //Только буквы         
        }
        public int a = 0;
        private void BtFam_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            a++;
            if (a == 1)
            {
                BtFam.Text = BtFam.Text[0].ToString().ToUpper();
            }
            else
            {
                BtFam.SelectionStart = BtFam.Text.Length;
            }
            if (BtFam.Text.Length == 0)
            {
                a = 0;
            }
        } //Первая буква большая

        private void TexBoxPolic_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0)); //Тольок цифры
        }

        private void TexBxOtchestv_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0)); //Тольок цифры
        }
    }
}

