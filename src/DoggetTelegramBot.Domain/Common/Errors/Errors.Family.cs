using ErrorOr;

namespace DoggetTelegramBot.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Family
        {
            public static Error NotFound => Error.NotFound(
                code: "Family.NotFound",
                description: "Family is not found.");

            public static Error NewMemberHasFamily => Error.Conflict(
                code: "Family.NewMemberHasFamily",
                description: "The requested user has family with a child or pet role.");

            public static Error TurnParentIntoChildOrPet => Error.Conflict(
                code: "Family.TurnParentIntoChildOrPet",
                description: "You cannot turn yourself or your spouse into a child or pet.");

            public static Error SomeoneIsPet => Error.Conflict(
                code: "Family.SomeoneIsPet",
                description: "Some of you are a pet in another family.");
        }
    }
}
