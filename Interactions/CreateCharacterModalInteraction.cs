using Discord;
using Discord.Net;
using Discord.WebSocket;
using DiscordRpBot.Constants;

namespace DiscordRpBot.Interactions
{
    public class CreateCharacterModalInteraction : BaseModalInteraction
    {
        private SocketModal? _currentModalSubmitInteraction;
        private readonly string[] _supportedImageFormats = [".png", ".jpg", ".jpeg", ".gif"];
        private Dictionary<string, string>? _currentResponseDictionary;

        public CreateCharacterModalInteraction()
        {
            Id = ModalId.RegisterForm;
            Fields = [TextInputId.Name, TextInputId.Personality, TextInputId.ImageUrl, TextInputId.Power, TextInputId.Backstory];
        }

        protected override async Task ProcessAsync(SocketModal modalSubmitInteraction, DiscordSocketClient client)
        {
            _currentModalSubmitInteraction = modalSubmitInteraction;
            Dictionary<string, string> responseDictionary = GetResponseDictionary(modalSubmitInteraction);
            _currentResponseDictionary = responseDictionary;

            string name = responseDictionary[TextInputId.Name];
            string personality = responseDictionary[TextInputId.Personality];
            string imageUrl = responseDictionary[TextInputId.ImageUrl];
            string power = responseDictionary[TextInputId.Power];
            string backstory = responseDictionary[TextInputId.Backstory];

            if (responseDictionary.Values.Any(string.IsNullOrWhiteSpace))
            {
                await TrySendUserSketchAndErrorFeedback("Por favor, preencha todos os campos.");
                return;
            }

            if (!Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute) || !_supportedImageFormats.Any(imageUrl.EndsWith))
            {
                await TrySendUserSketchAndErrorFeedback("Por favor, insira uma URL de imagem válida, como: `https://example.com/image.png`");
                return;
            }

            //TODO: Save character to database with flag "pending" and send to sheet approval channel

        }

        private async Task TrySendUserSketchAndErrorFeedback(string feedback)
        {
            if (_currentModalSubmitInteraction == null || _currentResponseDictionary == null)
                return;

            await _currentModalSubmitInteraction.RespondAsync(feedback);

            try
            {
                await _currentModalSubmitInteraction.User.SendMessageAsync($@"Como a criação de seu personagem falhou, você está recebendo o seu esboço de volta para evitar reescrever tudo. Por favor, corrija os campos e tente novamente. Se precisar de ajuda, entre em contato com um administrador.

                **Nome**: {_currentResponseDictionary[TextInputId.Name]}
                **Personalidade**: {_currentResponseDictionary[TextInputId.Personality]}
                **Url de imagem**: {_currentResponseDictionary[TextInputId.ImageUrl]}
                **Poder**: {_currentResponseDictionary[TextInputId.Power]}
                **História**: {_currentResponseDictionary[TextInputId.Backstory]}");

            }
            catch (HttpException exception)
            {
                await _currentModalSubmitInteraction.FollowupAsync("Eu tentei te enviar o esboço da sua ficha através do privado, mas suas configurações de privacidade não permitem que eu te envie mensagens. Por favor, habilite as mensagens diretas se quiser utilizar esta funcionalidade.");
                Console.WriteLine(exception);
            }
        }
    }
}
