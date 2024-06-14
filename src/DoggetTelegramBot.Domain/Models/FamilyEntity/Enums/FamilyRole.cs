using System.ComponentModel;

namespace DoggetTelegramBot.Domain.Models.FamilyEntity.Enums
{
    public enum FamilyRole
    {
        [Description(nameof(Grandparent))]
        Grandparent,
        [Description(nameof(Grandchild))]
        Grandchild,
        [Description(nameof(Parent))]
        Parent,
        [Description(nameof(Daughter))]
        Daughter,
        [Description(nameof(Son))]
        Son,
        [Description(nameof(Aunt))]
        Aunt,
        [Description(nameof(Uncle))]
        Uncle,
        [Description(nameof(Cousin))]
        Cousin,
        [Description(nameof(Niece))]
        Niece,
        [Description(nameof(Nephew))]
        Nephew,
        [Description(nameof(Dog))]
        Dog,
        [Description(nameof(Cat))]
        Cat,
    }
}
