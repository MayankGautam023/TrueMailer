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
using EmailValidation;
using MySql.Data.MySqlClient;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Validity(object sender, RoutedEventArgs e)
        {

            string email = Email.Text;
            Comment1.Foreground = Brushes.DarkGray;
            if (email != "example: example@gmail.com")
            {
                Comment1.Text = "Comments";
                Validation val = new Validation();
                string result = string.Empty;
                bool ret = val.Validate(email, ref result);
                Result.Content = result;
                if (result == "Valid Email")
                    Result.Foreground = Brushes.Green;
                DatabseConn dbcon = new DatabseConn();

                string result2 = string.Empty;
                string dat = string.Empty;

                string comm = dbcon.Connection(email, ref result2, ref dat);

                if (!string.IsNullOrEmpty(result2))
                {
                    Result.Foreground = Brushes.Red;
                    Result.Content = result2;
                }
                string newResult = string.Empty;

                if (!string.IsNullOrEmpty(comm))
                {
                    string[] com = comm.Split('^');

                    string[] dt = dat.Split('^');

                    for (int i=0;i<com.Length; i++)
                    {
                        if(com[i] != "")
                            newResult = dt[i]+ ": " + com[i] + "\n" + newResult;
                    }

                    string size = (com.Length - 1).ToString();
                    if (int.Parse(size) > 0)
                        CommentNo.Content = "Total Comments: " + size;
                    Comment1.Foreground = Brushes.Black;
                    Comment1.Text = newResult;
                }
                else if(string.IsNullOrEmpty(comm))
                    CommentNo.Content = "No Comments Available";
            }
            
        }

        private void btn_Report(object sender, RoutedEventArgs e)
        {
            string email = Email.Text;
            string comment = Comment.Text;
            if (comment == "Post Comment")
                comment = "";
            DatabseConn dbcon = new DatabseConn();
            dbcon.Report(email, comment);

        }

        private void RemoveText(object sender, RoutedEventArgs e)
        {
            if (Email.Text == "example: example@gmail.com")
            {
                Email.Text = "";
                Email.Foreground = Brushes.Black;
            }
        }

        private void AddText(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Email.Text))
            {
                Email.Text = "example: example@gmail.com";
                Email.Foreground = Brushes.DarkGray;
            }
        }

        private void AddPlace(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Comment.Text))
            {
                Comment.Text = "Post Comment";
                Comment.Foreground = Brushes.DarkGray;
            }
        }

        private void RemovePlace(object sender, RoutedEventArgs e)
        {
            if (Comment.Text == "Post Comment")
            {
                Comment.Text = "";
                Comment.Foreground = Brushes.Black;
            }
        }
    }
}
