using System;
using System.Collections.Generic;


namespace FinalExamLibraryCheckout
{
    internal class Program
    {
        // List
        static List<LibraryItem> catalog = FileManager.LoadCatalog("catalog.txt"); //holds all library items loads from catalog file
        static List<CheckoutItem> checkoutList = new List<CheckoutItem>();//holds all items checked out

        static void Main(string[] args)//entry point 
        {
            if (catalog == null || catalog.Count == 0)
            {
                Console.WriteLine("Catalog file not found or empty.");
            }

            // Run menu loop
            MainMenu();
        }

        static void ShowBanner()
        {
            Console.WriteLine("===== Library Checkout =====");
        }

        static void MainMenu()
        {
            ShowBanner();
            while (true)
            {
                Console.WriteLine("-----------------------------------------------");
                Console.WriteLine("1. Add Library Item");
                Console.WriteLine("2. View Available Items");
                Console.WriteLine("3. Checkout Item");
                Console.WriteLine("4. Return Item");
                Console.WriteLine("5. View Checkout Receipt");
                Console.WriteLine("6. Save checkout list to file");
                Console.WriteLine("7. Load previous checkout list from file");
                Console.WriteLine("8. Exit");
                Console.WriteLine("-----------------------------------------------");

                Console.Write("Select an option: ");
                string choice = Console.ReadLine();

                if (choice == "1") 
                    AddLibraryItem();
                else if (choice == "2") 
                    ViewAvailableItems();
                else if (choice == "3") 
                    CheckoutLibraryItem();
                else if (choice == "4") 
                    ReturnItem();
                else if (choice == "5") 
                    ViewCheckoutReceipt();
                else if (choice == "6") 
                    SaveCheckOutListToFile();
                else if (choice == "7") 
                    LoadPreviousCheckoutListFromFile();
                
                else if (choice == "8")
                {
                     Console.WriteLine("Exiting The Program...");
                     break;
                }
                else Console.WriteLine("Invalid option. Please try again.");
            }
        }

        static void AddLibraryItem()
        {
            Console.WriteLine("Enter the ID");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the Title");
            string title = Console.ReadLine();
            Console.WriteLine("Enter the Type");
            string type = Console.ReadLine();
            Console.WriteLine("Enter Daily Late Fee");
            double DailyLateFee = double.Parse(Console.ReadLine());

            catalog.Add(new LibraryItem(id, title, type, DailyLateFee));

            Console.WriteLine("Item Added");
        }
        
            


        static void ViewAvailableItems()
        {
            Console.WriteLine("===== Catalog =====");
            foreach (var item in catalog)
            {
                item.ShowInfo();
            }
        }

        static void CheckoutLibraryItem()
        {
            ViewAvailableItems();
            Console.Write("Enter ID of item to checkout: ");
            int id = int.Parse(Console.ReadLine());

            var item = catalog.Find(i => i.Id == id);
            if (item != null)
            {
               
                Console.Write("Enter borrower name: ");
                string borrower = Console.ReadLine();
                Console.Write("Enter loan days: ");
                int days = int.Parse(Console.ReadLine());

                CheckoutItem checkout = new CheckoutItem(item, borrower, days);
                checkoutList.Add(checkout);

                catalog.Remove(item);

                Console.WriteLine("Item checked out");
            }
            else
            {
                Console.WriteLine("Item not found");
            }
        }

        static void ReturnItem()
        {
            Console.Write("Enter ID of item to return: ");
            int id = int.Parse(Console.ReadLine());

            var checkout = checkoutList.Find(c => c.LibraryItem.Id == id);
            if (checkout != null)
            {
                checkout.ReturnDate = DateTime.Now;

                catalog.Add(checkout.LibraryItem);

                checkoutList.Remove(checkout);

                Console.WriteLine("Item returned");
            }
            else
            {
                Console.WriteLine("No checkout found for that Item");
            }
        }

        static void ViewCheckoutReceipt()
        {
            Console.WriteLine("===== Receipt =====");
            foreach (var checkout in checkoutList)
            {
                Console.WriteLine(checkout.GetReceiptLine());
            }
            Console.WriteLine("--------------------");
            
         }
        
        static void SaveCheckOutListToFile()
        {
            string fileName = "checkoutlist.txt";  //Create a filename

            List<string> lines = new List<string>(); // create empty list of strings 
            
            foreach (var checkout in checkoutList) // loop through each checkout item
            {
                string line = $"{checkout.LibraryItem.Id}|{checkout.BorrowerName}|{checkout.CheckoutDate}|{checkout.DueDate}|{checkout.ReturnDate}";
                lines.Add(line); //add string to the list
            }
            File.WriteAllLines(fileName, lines); //write all lines to the file
            Console.WriteLine("Checkout list saved to file");
        }
        
        static void LoadPreviousCheckoutListFromFile()
        {
            string fileName = "checkoutlist.txt";
            if (!File.Exists(fileName))   //checks if file exists
            {
                Console.WriteLine("No previous checkout list found");
                return;
            }
            var lines = File.ReadAllLines(fileName);
            checkoutList.Clear();        //clears current checkout list

            foreach (var line in lines)
            {
                var parts = line.Split('|');
                int itemId = int.Parse(parts[0]);
                string borrowerName = parts[1];
                DateTime checkoutDate = DateTime.Parse(parts[2]);
                DateTime dueDate = DateTime.Parse(parts[3]);
                DateTime? returnDate = string.IsNullOrEmpty(parts[4]) ? null : DateTime.Parse(parts[4]);
                
                var libraryItem = catalog.Find(i => i.Id == itemId);
                if (libraryItem != null)
                {
                    var checkoutItem = new CheckoutItem(libraryItem, borrowerName, (dueDate - checkoutDate).Days)
                    {
                        CheckoutDate = checkoutDate,
                        DueDate = dueDate,
                        ReturnDate = returnDate
                    };
                    checkoutList.Add(checkoutItem);
                    catalog.Remove(libraryItem);
                }
            }
            Console.WriteLine("Previous checkout list loaded from file");
        }
    }

}
