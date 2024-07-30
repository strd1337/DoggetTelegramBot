using System.ComponentModel;

namespace DoggetTelegramBot.Domain.Models.FamilyEntity.Enums
{
    public enum FamilyRole
    {
        [Description(nameof(Parent))]
        Parent,
        [Description(nameof(Daughter))]
        Daughter,
        [Description(nameof(Son))]
        Son,
        [Description(nameof(Dog))]
        Dog,
        [Description(nameof(Cat))]
        Cat,
    }
}
