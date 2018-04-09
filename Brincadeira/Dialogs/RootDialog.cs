using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Brincadeira.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(RetornoAsync);
            await Task.CompletedTask;
        }

        private async Task RetornoAsync(IDialogContext context, IAwaitable<object> result)
        {
            var resposta = await result as IMessageActivity;
            await EnviaMsgInicial(context);
        }

        async Task EnviaMsgInicial(IDialogContext context)
        {
            await context.SayAsync("Olá, sou o teste multidialogo", "Olá, sou o teste multidialogo");
        }
       
    }
}