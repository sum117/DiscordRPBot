

using Discord;
using Discord.WebSocket;
using DiscordRpBot.Constants;

namespace DiscordRpBot.Interactions;
public class CreateCharacterButtonInteraction : BaseButtonInteraction
{
    public CreateCharacterButtonInteraction()
    {
        Id = "create_character";
    }
    protected override async Task ProcessAsync(SocketMessageComponent interaction, DiscordSocketClient client)
    {
        await interaction.RespondWithModalAsync(GetRegisterForm());
    }
    private static Modal GetRegisterForm()
    {
        TextInputBuilder nameInput = new TextInputBuilder()
          .WithLabel("Nome")
          .WithCustomId(TextInputId.Name)
          .WithStyle(TextInputStyle.Short)
          .WithMaxLength(TextInputLength.Short);

        TextInputBuilder personalityInput = new TextInputBuilder()
        .WithLabel("Personalidade")
        .WithCustomId(TextInputId.Personality)
        .WithStyle(TextInputStyle.Paragraph)
        .WithMaxLength(TextInputLength.Medium);

        TextInputBuilder appearanceInput = new TextInputBuilder()
         .WithLabel("Aparência")
         .WithCustomId(TextInputId.Appearance)
         .WithStyle(TextInputStyle.Paragraph)
         .WithMaxLength(TextInputLength.Medium);

        TextInputBuilder powerInput = new TextInputBuilder()
         .WithLabel("Poder")
         .WithCustomId(TextInputId.Power)
         .WithStyle(TextInputStyle.Paragraph)
         .WithMaxLength(TextInputLength.Paragraph);

        TextInputBuilder backstoryInput = new TextInputBuilder()
         .WithLabel("História")
         .WithCustomId(TextInputId.Backstory)
         .WithStyle(TextInputStyle.Paragraph)
         .WithMaxLength(TextInputLength.Paragraph);

        ModalBuilder builder = new ModalBuilder()
        .WithTitle("Criação de Personagem")
        .WithCustomId(ModalId.RegisterForm)
        .AddTextInput(nameInput)
        .AddTextInput(personalityInput)
        .AddTextInput(appearanceInput)
        .AddTextInput(powerInput)
        .AddTextInput(backstoryInput);

        return builder.Build();
    }
}
