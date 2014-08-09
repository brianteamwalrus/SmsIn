using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmsIn.Models
{
    public class SmsGrid
    {
        public List<SmsClass> SmsList { get; set; }
        public int CurrentPage;
        public int PageSize;
        public double TotalPages;
        public int SortBy;
        public bool IsAsc;
        public string Search;
        public int IsLastRecord;
        public int Count;

        public SmsGrid()
        {
            SmsList = new List<SmsClass>();
        }

    }
}