using System.ComponentModel.DataAnnotations;

namespace FoodStyles.Utils
{
    public class MenuItem
    {
        [Key]
        public long Id { get; set; }
        public string MenuTitle { get; set; }
        public string MenuDescription { get; set; }
        public string MenuSectionTitle { get; set; }
        public string DishName { get; set; }
        public string DishDescription { get; set; }

    }
}