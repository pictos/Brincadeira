using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Brincadeira.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        //Em produção armazenar esses dados em BD
        string nome;
        int idade;

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
            await context.PostAsync("Olá, sou o teste multidialogo");
            context.Call(new NomeDialog(), NomeDialogResume);
        }

        async Task NomeDialogResume(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                nome = await result;
                context.Call(new IdadeDialogo(nome), IdadeDialogResumo);
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("Queimou todas as tentativas. Bora tentar de novo");
                await EnviaMsgInicial(context);
            }
        }

        async Task IdadeDialogResumo(IDialogContext context, IAwaitable<int> result)
        {
            try
            {
                idade = await result;
                await context.PostAsync($"Seu nome é {nome} e você tem {idade} anos");
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("QUeimou todas as tentativas");
            }
            finally
            {
                await EnviaMsgInicial(context);
            }
        }
    }
}