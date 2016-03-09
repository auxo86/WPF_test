using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Data.SQLite;
using System.Collections.ObjectModel;
using System.Data;

namespace WPF測試
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    /// 



    public partial class MainWindow : Window
    {
        //用於全域展示的ObservableCollection必須寫在這裡。
        public static List<Job> JobList = new List<Job>();
        public static List<List<Job>> DayList = new List<List<Job>>();
        public static List<string> JobNamelist = new List<string>();
        public static ObservableCollection<ObservableCollection<Job>> dataForShow = new ObservableCollection<ObservableCollection<Job>>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string strDayStart = "2016/04/01";
            string strDayEnd = "2016/04/29";
            DateTime dateStart = Convert.ToDateTime(strDayStart);
            DateTime dateEnd = Convert.ToDateTime(strDayEnd);

            SQLiteConnection sqlite_conn = new SQLiteConnection("data source=job_arrange.db");
            sqlite_conn.Open();
            SQLiteCommand sqlite_command = sqlite_conn.CreateCommand();
            sqlite_command.CommandText = "select * from JobHistory where date >= @dateStart and date <= @dateEnd;";
            sqlite_command.Parameters.Add(new SQLiteParameter("@dateStart") { Value = new DateTime(2016, 4, 1).ToString("yyyy-MM-dd") });
            sqlite_command.Parameters.Add(new SQLiteParameter("@dateEnd") { Value = new DateTime(2016, 4, 29).ToString("yyyy-MM-dd") });

            for (int i = 0; i < (dateEnd.Day - dateStart.Day + 1); i++)
            {
                sqlite_command.Parameters[0].Value = dateStart.AddDays(i).ToString("yyyy-MM-dd");
                sqlite_command.CommandText = "select * from JobHistory where date = @dateStart";
                SQLiteDataReader sqlite_datareader_JobHistory = sqlite_command.ExecuteReader();
                while (sqlite_datareader_JobHistory.Read())
                {
                    JobList.Add(new Job(sqlite_datareader_JobHistory[0].ToString(), Convert.ToDateTime(sqlite_datareader_JobHistory[1]).ToString("yyyy-MM-dd"), sqlite_datareader_JobHistory[2].ToString(), sqlite_datareader_JobHistory[3].ToString()));
                }

                DayList.Add(new List<Job>(JobList));
                JobList.Clear();
                sqlite_datareader_JobHistory.Close();
            }

            sqlite_command.CommandText = "select job_name from job where enable = 1 order by job_order;";
            SQLiteDataReader sqlite_datareader = sqlite_command.ExecuteReader();
            JobNamelist.Add("日期");
            while (sqlite_datareader.Read())
            {
                JobNamelist.Add(sqlite_datareader[0].ToString());
            }

            for (int i = 0; i < (dateEnd.Day - dateStart.Day + 1); i++)
            {
                //準備資料給dataForShow
                ObservableCollection<Job> DayJobsListForAdd = new ObservableCollection<Job>();
                DayJobsListForAdd.Add(new Job("", "", "", DayList[i][0].JobDate));//作弊一下把日期填在JobOwner屬性
                foreach (Job job in DayList[i])
                {
                    DayJobsListForAdd.Add(job);
                }
                dataForShow.Add(DayJobsListForAdd);
            }
            //DataCombine RowData = new DataCombine(JobNamelist, dataForShow);
            for (int MatchIndex = 0; MatchIndex < JobNamelist.Count; MatchIndex++)
            {
                //產生欄位資料
                DataGridTextColumn column = new DataGridTextColumn();
                //DataGridTemplateColumn column = new DataGridTemplateColumn();
                column.Header = JobNamelist[MatchIndex];
                column.Binding = new System.Windows.Data.Binding("[" + MatchIndex.ToString() + "].JobOwner");
                //column.CellTemplate = DataGridJobTable.FindResource("CCHJob").;
                DataGridJobTable.Columns.Add(column);
            }
            DataGridJobTable.ItemsSource = dataForShow;
            //MessageBox.Show("ok");
        }
    }
}
