namespace DoggetTelegramBot.Presentation.BotCommands
{
    public sealed class CommandInfo(string name, string description)
    {
        public string Name { get; private set; } = name;
        public string Description { get; private set; } = description;
    }
}
