using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace Brincadeira.Dialogs
{
    [Serializable]
    public class NomeDialog : IDialog<string>
    {
        int tentativas = 3;
       
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Como você gostaria de ser chamado?");

            context.Wait(MenssagemAsync);
        }

        private async Task MenssagemAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var menssagem = await result;

            if ((menssagem.Text != null) && (menssagem.Text.Trim().Length > 0))
            {
                context.Done(menssagem.Text);
            }
            else
            {
                --tentativas;
                if(tentativas>0)
                {
                    await context.PostAsync("Desculpe, não consegui entender");
                    context.Wait(MenssagemAsync);
                }
                else
                {
                    context.Fail(new TooManyAttemptsException("Você não digitou nada ou não é um texto"));
                }
            }
        }
    }
}