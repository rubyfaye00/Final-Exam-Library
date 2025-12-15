using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExamLibraryCheckout
{
    internal class CheckoutItem
    {
        public LibraryItem LibraryItem { get; set; }

        public string BorrowerName { get; set; }

        public DateTime CheckoutDate { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public CheckoutItem(LibraryItem libraryItem, string borrowerName, int loanDays)
        {
            LibraryItem = libraryItem;
            BorrowerName = borrowerName;
            CheckoutDate = DateTime.Now;
            DueDate = CheckoutDate.AddDays(loanDays); 
        }

        // Calculate late fees
        public double CalculateLateFee()
        {
            // Not returned yet, no fee
            if (ReturnDate == null)
                return 0;

            // Returned on or before due date, no fee
            if (ReturnDate <= DueDate)
                return 0;

            // Calculate days late
            int daysLate = (ReturnDate.Value - DueDate).Days;

            // Multiply by the item daily late fee
            return daysLate * LibraryItem.DailyLateFee;

        }

        // Line for receipt 
        public string GetReceiptLine()
        {
            double lateFee = CalculateLateFee();
            return $"{LibraryItem.Title} (Borrower: {BorrowerName}) - Due: {DueDate.ToShortDateString()} - Late Fee: ${CalculateLateFee():F2}";
        }


    }
}
