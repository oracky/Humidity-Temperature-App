using HumidityAndTempApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace HumidityAndTempApp.Controllers

{
    public class ConditionsController : Controller
    {
        // GET: Conditions
        public ActionResult TemperatureCelsius()
        {
            Conditions tempC = new Conditions("C2:C", "M3", "P3", "J3");

            tempC.valueList = Data.GetColumnValues(tempC.valueRange);
            tempC.lastValue = Data.GetLastFromColumn(tempC.valueList);
            tempC.lastTime = Data.GetLastFromColumn(Data.GetColumnValues(tempC.timeRange,true));
            tempC.maxV = Data.GetOneValue(tempC.maxCell);
            tempC.minV = Data.GetOneValue(tempC.minCell);
            tempC.avgV = Data.GetOneValue(tempC.avgCell);
            

            return View(tempC);
        }

        public ActionResult TemperatureFahrenheit()
        {
            Conditions tempF = new Conditions("D2:D", "M4", "P4", "J4");

            tempF.valueList = Data.GetColumnValues(tempF.valueRange);
            tempF.lastValue = Data.GetLastFromColumn(tempF.valueList);
            tempF.lastTime = Data.GetLastFromColumn(Data.GetColumnValues(tempF.timeRange, true));
            tempF.maxV = Data.GetOneValue(tempF.maxCell);
            tempF.minV = Data.GetOneValue(tempF.minCell);
            tempF.avgV = Data.GetOneValue(tempF.avgCell);
            


            return View(tempF);
        }

        public ActionResult Humidity()
        {
            Conditions humidity = new Conditions("B2:D", "M2", "P2", "J2");

            humidity.valueList = Data.GetColumnValues(humidity.valueRange);
            humidity.lastValue = Data.GetLastFromColumn(humidity.valueList);
            humidity.lastTime = Data.GetLastFromColumn(Data.GetColumnValues(humidity.timeRange, true));
            humidity.maxV = Data.GetOneValue(humidity.maxCell);
            humidity.minV = Data.GetOneValue(humidity.minCell);
            humidity.avgV = Data.GetOneValue(humidity.avgCell);

            return View(humidity);
        }
    }
}