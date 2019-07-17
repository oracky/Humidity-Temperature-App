using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumidityAndTempApp.Models
{
    public class Conditions : Data
    {

        public Conditions(string r, string mxC, string mnC, string agC)
        {
            valueRange = r;
            maxCell = mxC;
            minCell = mnC;
            avgCell = agC;
        }

        public List<int> valueList { get; set; }
        public string maxCell { get; set; }
        public string minCell { get; set; }
        public string avgCell { get; set; }
        

        public double maxV { get; set; }
        public double minV { get; set; }
        public double avgV { get; set; }
        public DateTime lastTime { get; set; }

    }
}