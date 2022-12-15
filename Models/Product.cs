using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Products.Models
{
    public class Product
    {

        public int Id { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Поле не может быть пустым!")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Поле не может быть пустым!")]
        public string Description { get; set; }

        [Display(Name = "Цена")]
        [Required(ErrorMessage = "Поле не может быть пустым!")]
        [Column(TypeName="decimal(18,2)")]
        public decimal Price { get; set; }
    }
}
