using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmileService.Models;

public partial class User
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? ContactInfo { get; set; }

    public virtual ICollection<OrderPart> OrderParts { get; set; } = new List<OrderPart>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [NotMapped] // Этот атрибут говорит Entity Framework, что этого поля нет в самой БД
    public string RoleDisplayName
    {
        get
        {
            return Role switch
            {
                "Admin" => "Администратор",
                "Master" => "Мастер",
                "Receptionist" => "Приемщик",
                "Storekeeper" => "Кладовщик",
                _ => Role // Если роль какая-то другая, вернет её как есть
            };
        }
    }
}
