using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WPF測試
{
    public class Job
    {
        public string JobGuid { get; set; }
        public string JobDate { get; set; }
        public string JobName { get; set; }
        public string JobOwner { get; set; }

        public Job(string jobguid, string jobdate, string jobname, string jobowner)
        {
            JobGuid = jobguid;
            JobDate = jobdate;
            JobName = jobname;
            JobOwner = jobowner;
        }
    }

    public class ColumnMatchData
    {
        //public List<string> JobNameList { get; set; }
        //public ObservableCollection<List<string>> DataForShow { get; set; }
        public Dictionary<string, string> row { get; set; }
        public ColumnMatchData(List<string> jobnamelist, ObservableCollection<string> OneDayJobData)
        {
            row = new Dictionary<string, string>();
            for (int iJob = 0; iJob < jobnamelist.Count; iJob++)
            {
                row.Add(jobnamelist[iJob], OneDayJobData[iJob]);
            }
        }

    }

    public class DataCombine
    {

        public ObservableCollection<ColumnMatchData> Rows { get; set; }
        public List<string> JobNameList { get; set; }
        public ObservableCollection<ObservableCollection<string>> DataForShow { get; set; }

        public DataCombine(List<string> jobnamelist, ObservableCollection<ObservableCollection<string>> dataforshow)
        {
            JobNameList = jobnamelist;
            DataForShow = dataforshow;
            Rows = new ObservableCollection<ColumnMatchData>();
            foreach (ObservableCollection<string> oneDayJobData in DataForShow)
            {
                Rows.Add(new ColumnMatchData(JobNameList,oneDayJobData));
            }
        }
    }
}
//JobInOneDay.Add(date);
//foreach( Job job in JobListInOneDay )
//{
//    JobInOneDay.Add(job.JobOwner);
//}
//    Date = date;
//    foreach (Job job in JobListInOneDay)
//    {
//        listName.Add(job.JobOwner);
//    }
