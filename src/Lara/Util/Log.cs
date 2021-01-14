using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;

namespace Lara.Util
{
    public class Log : IDisposable
    {
        internal class ModeloLog
        {
            public DateTime Data { get; set; }
            public string LoginUsuario { get; set; }
            public string Mensagem { get; set; }

            public ModeloLog(string mensagem)
            {
                Data = DateTime.Now;
                LoginUsuario = string.IsNullOrEmpty(Constantes.USUARIO_LOGADO) ? Constantes.USUARIO_SISTEMA : Constantes.USUARIO_LOGADO;
                Mensagem = mensagem;
            }
        }

        private readonly Queue<ModeloLog> FilaLog;
        private readonly Queue<ModeloLog> FilaLogExecucao;
        private readonly Thread thread;
        private bool ImprimindoMsg = false;
        private bool ParaProcessamento = false;

        public Log()
        {
            FilaLog = new Queue<ModeloLog>();
            FilaLogExecucao = new Queue<ModeloLog>();

            thread = new Thread(new ThreadStart(ProcessamentoFilaLog));
            thread.Start();
        }

        private void ProcessamentoFilaLog()
        {
            while (!ParaProcessamento)
            {
                try
                {
                    if (FilaLog.Count > 0)
                    {
                        ModeloLog log = FilaLog.Dequeue();

                        using StreamWriter sw = File.AppendText(Constantes.LogExecucao);
                        sw.WriteLine($"{log.Data}, {log.LoginUsuario} \n{log.Mensagem}");
                    }
                    else if (FilaLogExecucao.Count > 0 && !ImprimindoMsg)
                    {
                        ImprimindoMsg = true;
                        ModeloLog log = FilaLogExecucao.Dequeue();

                        App.Invoke(() =>
                        {
                            Constantes.INSTANCIA_LARA.txbLogExecucao.Text = $"{log.Mensagem}";
                            Thread.Sleep(1000);
                        });

                        ImprimindoMsg = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro em ProcessamentoFilaLog! = {ex.Message} - {ex.StackTrace}");
                }
            }
        }

        public void EnfileirarLog(string mensagem, params object[] param)
        {
            TratarMensagem(mensagem, null, param);
        }

        public void EnfileirarLogErro(string mensagem, Exception ex, params object[] param)
        {
            TratarMensagem(mensagem, ex, param);
        }

        public void EnfileirarLogExecucao(string mensagem, params object[] param)
        {
            try
            {
                var strParam = $"[{DateTime.Now}] {mensagem} ";

                foreach (var item in param)
                    strParam += $"{item.GetType()}: {item} ";

                FilaLogExecucao.Enqueue(new ModeloLog(mensagem));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro em EnfileirarLogExecucao! = {ex.Message} - {ex.StackTrace}");
            }
        }

        private void TratarMensagem(string mensagem, Exception erro = null, params object[] param)
        {
            try
            {
                var strParam = $"[{DateTime.Now}] {mensagem} ";

                foreach (var item in param)
                    strParam += $"{item.GetType()}: {item} ";

                if (erro != null)
                    strParam += $"{erro.Message} -\n {erro.StackTrace} ";

                FilaLog.Enqueue(new ModeloLog(mensagem));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro em TratarMensagem! = {ex.Message} - {ex.StackTrace}");
            }
        }

        public void Dispose()
        {
            try
            {
                ParaProcessamento = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro em TratarMensagem! = {ex.Message} - {ex.StackTrace}");
            }
        }
    }
}
