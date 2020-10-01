using System.Collections.Generic;
using System.Text.Json;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using Serilog;

namespace FoodStyles.Utils
{
    public class Scrapper
    {
        private readonly string _startUrl;
        private const string RootUrl = "https://www.pure.co.uk";

        
        public Scrapper(string startUrl)
        {
            _startUrl = startUrl;
        }

        public IEnumerable<MenuItem> Parse()
        {
            Log.Logger.Information("Start parsing...");
            var menuList = GetMenu();
            var items = new List<MenuItem>();

            foreach (var item in menuList)
            {
                var path = RootUrl + item.Key;
                Log.Logger.Information($"Parsing {item.Value}");
                var docMenuPage = new HtmlWeb().Load(path);
                var headers = docMenuPage.DocumentNode.QuerySelector("header.menu-header");
                
                Log.Logger.Information("Parsing description");
                var headerDescription = headers.QuerySelector("p");
                
                var sections = docMenuPage.DocumentNode.QuerySelectorAll("h4.menu-title");
                Log.Logger.Information("Parsing sections");
                foreach (var t in sections)
                {
                    var sectionDataHref = t.QuerySelector("a").Attributes["href"].Value.Split('#')[1];
                    var dishBlock = docMenuPage.DocumentNode.QuerySelectorAll("div#" + sectionDataHref);
                    var dishes = dishBlock.QuerySelectorAll("div > div.menu-item");

                    Log.Logger.Information("Parsing each dish");
                    foreach (var dish in dishes)
                    {
                        var tmp = new MenuItem();
                        var dishHref = dish.QuerySelector("a");
                        var dishPath = RootUrl + dishHref.Attributes["href"].Value;
                        var dishDoc = new HtmlWeb().Load(dishPath);
                        var dishDescription = dishDoc.QuerySelector("header+div");
                        tmp.MenuTitle = item.Value;
                        tmp.MenuSectionTitle = sectionDataHref;
                        tmp.MenuDescription = headerDescription.InnerText;
                        tmp.DishName = dishHref.Attributes["title"].Value;
                        tmp.DishDescription = dishDescription.InnerText;
                        
                        Log.Logger.Information("Item created: " + JsonSerializer.Serialize(tmp));
                        items.Add(tmp);
                    }
                }
            }

            Log.Logger.Information("Parsing was finished.");
            return items;
        }

        private List<KeyValuePair<string, string>> GetMenu()
        {
            var startDoc = new HtmlWeb().Load(_startUrl);

            var menuList = new List<KeyValuePair<string, string>>();
            
            var node = startDoc.QuerySelector("ul.nav ul.submenu");

            var listMenu = node.QuerySelectorAll("li a");

            foreach (var menu in listMenu)
            {
                var href = menu.Attributes["href"].Value;

                menuList.Add(new KeyValuePair<string, string>(href, menu.InnerText));
            }
            
            Log.Logger.Information("Get menus link and text");

            return menuList;
        }
    }
}