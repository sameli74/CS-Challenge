using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    class Program
    {
        static HashSet<string> categories=new HashSet<string>();
        static void Main(string[] args)
        {
            getCategories();
            while (true){
                print_main_menu();
                Console.Write("Your choice: ");
                char input=Char.ToLower(Console.ReadKey().KeyChar);
                if (input == 'c')
                {
                    Console.WriteLine("\n\nCategories: ");
                    DisplaySet(categories);
                }
                else if(input=='q')
                    break;
                else if (input == 'r')
                    random_joke();
                else{
                    Console.WriteLine("\nThe input is not valid. Please make a new selection.");
                    continue;
                }
                    
            }

        }
        private static void DisplaySet(HashSet<string> categories){
            foreach (string i in categories)
                Console.Write("{0}, ", i);
        }

        private static void print_main_menu(){
            Console.WriteLine("\n\nPlease select one of the following options:");
            Console.WriteLine("1. Press c to get categories");
            Console.WriteLine("2. Press r to get random jokes");
            Console.WriteLine("3. Press q to to quit from the program");
            Console.WriteLine("After entering your selection, please hit the enter button.");
        }

        private static int select_num_jokes(){
            int n ;
            while (true){
                Console.WriteLine("\n\nHow many jokes would you like to have?");
                Console.WriteLine("Please select a number between 1 and 9 (inclusive)");
                Console.Write("Your selection: ");
                n = Convert.ToInt32(Console.ReadKey().KeyChar);
                n=n-(int)'0';
                if (n>0 && n<=10)
                    break;
                else
                   Console.WriteLine("The input format is wrong. Please choose a proper option"); 
            }
            return n;
        }
        private static Tuple<string, string>select_name(){
                Tuple<string, string> name;
                while (true){
                Console.Write("\n\nWould you like to use a random name?[y|n|q]: ");
                char random_name=Char.ToLower(Console.ReadKey().KeyChar);
                if (random_name == 'y'){
                    name=GetNames();
                    break;
                }
                else if (random_name == 'n'){
                    name=null;
                    break;
                }
                    
                else if (random_name=='q')
                    System.Environment.Exit(1);
                else
                    Console.WriteLine("\nThe input format is wrong. Please choose a proper option");
                
            }
            return name;
        }

        private static void generate_jokes(Tuple<string, string> name, int n){
            string[] jokes;
            bool success=false;
            while(success==false){
                Console.Write("\n\nWould you like to select a category?[y|n|q]: ");
                char selection=Char.ToLower(Console.ReadKey().KeyChar);
                if (selection=='y'){
                    while (true){
                        Console.WriteLine("\n\nPlease note that the acceptable categories are:");
                        DisplaySet(categories);
                        Console.Write("\nYour selected category: ");
                        string category=Console.ReadLine().ToLower();
                        if (categories.Contains(category)){
                            jokes=GetRandomJokes(category, n, name);
                            Console.WriteLine("\n");
                            Console.WriteLine(string.Join("\n", jokes));
                            success=true;
                            break;
                        }
                        else{
                            Console.WriteLine("The input category is wrong. Please choose a proper category");
                            continue;
                        }
                    }
                }
                else if (selection=='n'){
                    jokes=GetRandomJokes(null, n, name);
                    Console.WriteLine("\n");
                    Console.WriteLine(string.Join("\n", jokes));
                    break;
            }
                else if (selection=='q')
                    System.Environment.Exit(1);
                else
                    Console.WriteLine("\nThe input format is wrong. Please choose a proper option"); 
            }
        }

        private static void random_joke(){
            Tuple<string, string>name=select_name();
            int n=select_num_jokes();
            generate_jokes(name, n);
        }


        private static string[] GetRandomJokes(string category, int number, Tuple<string, string> name)
        {
            new JsonFeed("https://api.chucknorris.io");
            return JsonFeed.GetRandomJokes(name?.Item1, name?.Item2, category, number);
        }

        private static void getCategories()
        {
            new JsonFeed("https://api.chucknorris.io/jokes/");
            string[] cat=JsonFeed.GetCategories();
            foreach (var c in cat)
				categories.Add(c);
        }

        private static Tuple<string, string> GetNames()
        {
            new JsonFeed("https://randomuser.me/api/?format=json/");
            dynamic result = JsonFeed.Getnames();
            return Tuple.Create(result.results[0].name.first.ToString(), result.results[0].name.last.ToString());
        }
    }
}
