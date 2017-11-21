using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tasks.Models
{
    public class ProductViewModel
    {
        public int ID { get; set; }
        [Required]
        public int Code { get; set; }
        [Required]
        public string Name{ get; set; }
        [Required]
        public double Price { get; set; }
        public string Barcode { get; set; }
    }
    public class CustomMessage
    {
        public CustomMessage(string text,string color)
        {
            Text = text;
            Color = color;
        }
        public string Text { get; set; }
        public string Color { get; set; }
    }
    //...
    public enum Color
    {
        Red
    }
}