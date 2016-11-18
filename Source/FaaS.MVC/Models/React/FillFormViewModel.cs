using FaaS.DataTransferModels;
using System;
using System.Collections.Generic;

namespace FaaS.MVC.Models.React
{
    public class FillFormViewModel
    {
        public Form Form { get; set; }
        public Element[] Elements { get; set; }
        public string[] Values { get; set; }
    }
}
