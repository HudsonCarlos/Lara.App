using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lara_MP.Utils
{
    public class Log
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
        private readonly Thread thread;

        public Log()
        {
            FilaLog = new Queue<ModeloLog>();
            
            thread = new Thread(new ThreadStart(ProcessamentoFilaLog));
            thread.Start();
        }
        
        private void ProcessamentoFilaLog()
        {
            while (true)
            {
                try
                {
                    if (FilaLog.Count > 0)
                    {
                        ModeloLog log = FilaLog.Dequeue();                        

                        using (StreamWriter sw = File.AppendText(Constantes.LogExecucao))
                        {
                            sw.WriteLine($"{log.Data}, {log.LoginUsuario} \n{log.Mensagem}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("");
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
                throw;
            }
        }
    }
}
