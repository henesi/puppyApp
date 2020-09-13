using System;
using System.Collections.Generic;
using System.Text;

namespace Contract.Models.ComputerVision
{
    public class Statistics
    {
        public Guid StatisticId { get; set; }
        public int TypeOfMedia { get; set; }
        public string ElapsedTime { get; set; }
        public Guid AnimalId { get; set; }
        public string FileName { get; set; }

    }
}
