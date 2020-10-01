using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;

namespace FoodStyles.Utils
{
    public class Scrapper
    {
        private const string StartUrl = "https://www.pure.co.uk/menus/breakfast/";
        private const string RootUrl = "https://www.pure.co.uk";

        public Scrapper() { }

        public void Parse()
        {
            var menuList = GetMenu();
            var items = new List<MenuItem>();

            foreach (var item in menuList)
            {
                var path = RootUrl + item.Key;
                var docMenuPage = new HtmlWeb().Load(path);
                var headers = docMenuPage.DocumentNode.QuerySelector("header.menu-header");
                var headerDescription = headers.QuerySelector("p");

                var sections = docMenuPage.DocumentNode.QuerySelectorAll("h4.menu-title");
                foreach (var t in sections)
                {
                    var sectionDataHref = t.QuerySelector("a").Attributes["href"].Value.Split('#')[1];
                    
                    var dishBlock = docMenuPage.DocumentNode.QuerySelectorAll("div#" + sectionDataHref);
                    var dishes = dishBlock.QuerySelectorAll("div > div.menu-item");

                    foreach (var dish in dishes)
                    {
                        var tmp = new MenuItem();
                        var dishHref = dish.QuerySelector("a");
                        var dishPath = RootUrl + dishHref.Attributes["href"].Value;
                        var dishDoc = new HtmlWeb().Load(dishPath);
                        var dishDescription = dishDoc.QuerySelector("div:nth-child(3)");
                        tmp.MenuTitle = item.Value;
                        tmp.MenuSectionTitle = sectionDataHref;
                        tmp.MenuDescription = headerDescription.InnerText;
                        tmp.DishName = dishHref.Attributes["title"].Value;
                        tmp.DishDescription = dishDescription.InnerText;
                        items.Add(tmp);
                    }
                }
            }
            
            WriteFile<MenuItem>(items);
        }
        
        private void WriteFile<T>(IEnumerable<T> data)
        {
            var folder = Environment.CurrentDirectory;
            var path = Path.Combine(folder, "food.json");

            using var file = File.CreateText(path);
            var json = JsonSerializer.Serialize(data);
            File.WriteAllText(path, json);
        }

        private List<KeyValuePair<string, string>> GetMenu()
        {
            var startDoc = new HtmlWeb().Load(StartUrl);

            var menuList = new List<KeyValuePair<string, string>>();
            
            var node = startDoc.QuerySelector("ul.nav ul.submenu");

            var listMenu = node.QuerySelectorAll("li a");

            foreach (var menu in listMenu)
            {
                var href = menu.Attributes["href"].Value;

                menuList.Add(new KeyValuePair<string, string>(href, menu.InnerText));
            }

            return menuList;
        }
    }
}