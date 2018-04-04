using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace Brincadeira.Dialogs
{
    [Serializable]
    public class IdadeDialogo : IDialog<int>
    {
        string nome;
        int tentativas = 3;
        public IdadeDialogo(string nome)
        {
            this.nome = nome;
        }

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync($"{this.nome}, quantos anos vc tem?");
            context.Wait(RetornoAsync);
        }

        async Task RetornoAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var resposta = await result;
            if (Int32.TryParse(resposta.Text, out int idade) && (idade > 0))
            {
                context.Done(idade);
            }
            else
            {
                --tentativas;
                if (tentativas > 0)
                {
                    await context.PostAsync("Não entendi sua idade, digite valores naturais");
                    context.Wait(RetornoAsync);
                }
                else
                {
                    context.Fail(new TooManyAttemptsException("3 tentativas erradas"));
                }
            }
        }
    }
}