using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleApp1{
    class JsonFeed{
        static string _url = "";
    		public JsonFeed(string endpoint){
                _url = endpoint;
            }
    		private static string gen_joke_url(string category){
    			string url = "jokes/random";
    			if (category != null){
    				if (url.Contains('?'))
    					url += "&";
    				else url += "?";
    				url += "category=";
    				url += category;
    			}
    			return url;
    		}
    		private static string replace_name(string joke, string firstname, string lastname){
    			int index = joke.IndexOf("Chuck");
				while (index!=-1){
					index = joke.IndexOf("Chuck");
					if (index==-1)
						index = joke.IndexOf("CHUCK");
					if (index==-1)
						break;
					string firstPart = joke.Substring(0, index);
					string secondPart = joke.Substring(0 + index + "Chuck Norris".Length, joke.Length - (index + "Chuck Norris".Length));
					joke = firstPart + firstname+ " "  + lastname + secondPart;
				}
				return joke;
    		}
    		public static string[] GetRandomJokes(string firstname, string lastname, string category, int num){
    			HttpClient client = new HttpClient();
    			client.BaseAddress = new Uri(_url);
    			string[] random_jokes=new String[num];
    			string url=gen_joke_url(category);
    			while (num>0){
    				string joke = Task.FromResult(client.GetStringAsync(url).Result).Result;
    				if (firstname != null && lastname != null){
    					joke=replace_name(joke, firstname, lastname);
                }
    				random_jokes[num-1]=JsonConvert.DeserializeObject<dynamic>(joke).value ;
    				num--;
    			}
                return random_jokes;
        }

        /// <summary>
            /// returns an object that contains name and surname
            /// </summary>
            /// <param name="client2"></param>
            /// <returns></returns>
    		public static dynamic Getnames(){
    			HttpClient client = new HttpClient();
    			client.BaseAddress = new Uri(_url);
    			var result = client.GetStringAsync("").Result;
    			return JsonConvert.DeserializeObject<dynamic>(result);
    		}
    		private static string[] parse_categories(string response){
    			var charsToRemove = new string[] { "[", "]", "\""};
    			foreach (var c in charsToRemove)
    				response = response.Replace(c, "");
    			string[] tokens = response.Split(',');
    			return tokens;
    		}
    		public static string[] GetCategories(){
    			HttpClient client = new HttpClient();
    			client.BaseAddress = new Uri(_url);
    			string response=Task.FromResult(client.GetStringAsync("categories").Result).Result;
    			return parse_categories(response);
    		}
    }
}
