using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExamLibraryCheckout
{
    
    internal class LibraryItem
    {
        public int Id { get; set; } //ID of the item 
        public string Title { get; set; } //Title of the item
        public string Type { get; set; } //Type of the item (Book, DVD, Magazine)
        public double DailyLateFee { get; set; } //Daily late fee for the item
        public LibraryItem(int id, string title, string type, double dailyLateFee)
        {
            Id = id;
            Title = title;
            Type = type;
            DailyLateFee = dailyLateFee;
        }
        public void ShowInfo()
        {
            Console.WriteLine($"{Id}|{Title}|{Type}|{DailyLateFee}");
        }

    }       
 }      

