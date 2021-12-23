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
    /// Логика взаимодействия для Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
            CbFill();
            //InfoForComBoMedic();
            //InfoForComBoTime();
        }

        private void BtInfo_Click(object sender, RoutedEventArgs e)
        {
            Autoris a = new Autoris();
            a.Show();
            this.Close();
        }
        public void CbFill()  //Данные для комбобоксов 
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {
                try
                {
                    connection.Open();
                    string query1 = $@"SELECT * FROM Specialists"; // Типы
                    //string query2 = $@"SELECT * FROM Medics"; // Состояние
                    //string query3 = $@"SELECT * FROM Times"; // Кабинеты
                  
                    //----------------------------------------------
                    SQLiteCommand cmd1 = new SQLiteCommand(query1, connection);
                    //SQLiteCommand cmd2 = new SQLiteCommand(query2, connection);
                    //SQLiteCommand cmd3 = new SQLiteCommand(query3, connection);
                    
                    //----------------------------------------------
                    SQLiteDataAdapter SDA1 = new SQLiteDataAdapter(cmd1);
                    //SQLiteDataAdapter SDA2 = new SQLiteDataAdapter(cmd2);
                    //SQLiteDataAdapter SDA3 = new SQLiteDataAdapter(cmd3);
                    
                    //----------------------------------------------
                    DataTable dt1 = new DataTable("Specialists");
                    //DataTable dt2 = new DataTable("Medics");
                    //DataTable dt3 = new DataTable("Times");
                   
                    //----------------------------------------------
                    SDA1.Fill(dt1);
                    //SDA2.Fill(dt2);
                    //SDA3.Fill(dt3);
                   
                    //----------------------------------------------
                    CbSpec.ItemsSource = dt1.DefaultView;
                    CbSpec.DisplayMemberPath = "Special";
                    CbSpec.SelectedValuePath = "ID";
                    //----------------------------------------------
                    //CbDoc.ItemsSource = dt2.DefaultView;
                    //CbDoc.DisplayMemberPath = "Medic";
                    //CbDoc.SelectedValuePath = "ID";
                    ////----------------------------------------------
                    //CbTime.ItemsSource = dt3.DefaultView;
                    //CbTime.DisplayMemberPath = "Watch";
                    //CbTime.SelectedValuePath = "ID";
                    //----------------------------------------------
                    //CbBrand.ItemsSource = dt4.DefaultView;
                    //CbBrand.DisplayMemberPath = "Brand";
                    //CbBrand.SelectedValuePath = "ID";
                    ////----------------------------------------------
                    //CbTitle.ItemsSource = dt5.DefaultView;
                    //CbTitle.DisplayMemberPath = "Title";
                    //CbTitle.SelectedValuePath = "ID";
                    ////----------------------------------------------
                    //CbModel.ItemsSource = dt6.DefaultView;
                    //CbModel.DisplayMemberPath = "Model";
                    //CbModel.SelectedValuePath = "ID";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public void InfoForComBoMedic()
        {
        if (CbSpec.SelectedIndex != -1)
            {
                using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
                {
                    connection.Open();
                    int id;
                    bool resultClass = int.TryParse(CbSpec.SelectedValue.ToString(), out id);
                    string query = $@"SELECT * FROM Medics WHERE Medics.ID =  '{id}';"; // Типы
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable("Specialists");
                    SDA.Fill(dt);
                    CbDoc.ItemsSource = dt.DefaultView;
                    CbDoc.DisplayMemberPath = "Medic";
                    CbDoc.SelectedValuePath = "ID";
                }
            }
   
    }

        public void InfoForComBoTime()
        {
            if (DpDate.Text != "" && CbDoc.SelectedIndex != -1)
            {
               
                using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
                {
                    connection.Open();
                    int id, id2;
                    bool resultClass = int.TryParse(CbSpec.SelectedValue.ToString(), out id);
                    bool resultClass2 = int.TryParse(CbDoc.SelectedValue.ToString(), out id2);
                    string query = $@"SELECT Times.Watch,Times.ID FROM Vrachis 
                                      JOIN Times On Vrachis.IDTime = Times.ID
                                      WHERE Vrachis.IDSpecial = ('{id}') AND Vrachis.IDNameDoc = ('{id2}');"; 
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    SQLiteDataAdapter SDA = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable("Times");
                    SDA.Fill(dt);
                    CbWatch.ItemsSource = dt.DefaultView;
                    CbWatch.DisplayMemberPath = "Watch";
                    CbWatch.SelectedValuePath = "ID";
                   
                }
            }
        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            using (SQLiteConnection connection = new SQLiteConnection(DBConnection.myConn))
            {
                connection.Open();
                if (CbSpec.SelectedIndex == -1 || CbDoc.SelectedIndex == -1 || CbWatch.SelectedIndex == -1 || String.IsNullOrEmpty(DpDate.Text))
                {
                    MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    int id, id2, id3/*, id4, id5, id6*/;
                    bool resultClass = int.TryParse(CbSpec.SelectedValue.ToString(), out id);
                    bool resultKab = int.TryParse(CbDoc.SelectedValue.ToString(), out id2);
                    bool resultCon = int.TryParse(CbWatch.SelectedValue.ToString(), out id3);
                    //bool resultTitl = int.TryParse(CbTitle.SelectedValue.ToString(), out id4);
                    //bool resultBrand = int.TryParse(CbBrand.SelectedValue.ToString(), out id5);
                    //bool resultModel = int.TryParse(CbModel.SelectedValue.ToString(), out id6);
                    var UserAdd = Saver.Login;
                    //var numkab = TbNumber.Text;
                    //var number = TbNumber.Text;
                    //var idtype = CbClass.Text;
                    //var idcon = CbCondition.Text;
                    var startWork = DpDate.Text; //SELECT  COUNT(1) FROM Doctors WHERE IDSpecialist=('1') AND IDMedic=('1') AND IDTime=('20.12.2021') AND IDDay=('1')
                    
                    string query = $@"SELECT  COUNT(1) FROM Records WHERE IDSpecialist=@IDSpecialist AND IDMedic=@IDMedic AND IDDay=@IDDay AND IDTime=@IDTime";
                    SQLiteCommand cmd = new SQLiteCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IDSpecialist", id);
                    cmd.Parameters.AddWithValue("@IDMedic", id2);
                    cmd.Parameters.AddWithValue("@IDDay", DpDate.Text);
                    cmd.Parameters.AddWithValue("@IDTime", id3);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.ExecuteNonQuery();
                    int pr = 0;
                    if (count == 1)
                    {
                        MessageBox.Show("Данное время  в этот день занято", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        pr = 1;
                    }
                    if (pr == 0)
                    {
                        string query2 = $@"INSERT INTO Doctors(IDSpecialist,IDMedic,IDTime,IDDay,Login) values ('{id}','{id2}','{id3}','{startWork}','{UserAdd}');";
                        SQLiteCommand cmd2 = new SQLiteCommand(query2, connection);
                        try
                        {
                            MessageBox.Show("Вы записались к врачу");
                            cmd2.ExecuteNonQuery();

                        }

                        catch (SQLiteException)
                        {
                            //MessageBox.Show("Такой номер занят!");
                            //TbNumber.Clear();
                        }
                    }
                }
            }
        }

        private void CbSpec_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InfoForComBoMedic();
        }
        private void DpDate_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //InfoForComBoTime();
        }

        private void CbDoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //InfoForComBoTime();
        }

        private void DpDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            InfoForComBoTime();
        }
    }
    
}
