using System;
using System.ComponentModel.DataAnnotations;

namespace FaaS.MVC.Models
{
    public enum InputType
    {
        CheckBox,
        Date,
        Radio,
        Range,
        Text,
        TextArea
    }

    public class ElementViewModel
    {
        [Display(Name = "ID")]
        public Guid Id { get; set; }

        public string Description { get; set; }

        public string Options { get; set; }

        public InputType Type { get; set; }

        public bool Required { get; set; }

        public Guid FormId { get; set; }
    }
}
