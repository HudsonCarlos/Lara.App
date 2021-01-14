using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lara_MP.Utils
{
    public static class Constantes
    {
        public static string USUARIO_LOGADO { get; set; }
        public static string USUARIO_SISTEMA { get; private set; }
        public static string CAMINHO_EXECUCAO 
        {
            get { return CAMINHO_EXECUCAO; }
            private set 
            {
                AssistenteFilePathIO.AddPermissionsToDirectory(value);
                CAMINHO_EXECUCAO = value;
            } 
        }
        public static string VERSAO_APLICACAO { get; private set; }

        private static Log _logLara;
        public static Log LogLara
        {
            get
            {
                if (LogLara == null)
                    _logLara = new Log();
                return LogLara;
            }
        }

        private static string _logExecucao;
        public static string LogExecucao
        {
            get 
            {                
                return _logExecucao;
            }
            set 
            { 
                _logExecucao = Path.Combine(CAMINHO_EXECUCAO, $"log_execucao_{DateTime.Now.ToString("ddMMyyyy")}.log");

                if (!File.Exists(_logExecucao))
                    File.Create(_logExecucao);
            }
        }

        public static void CarregarConstantes()
        {
            try
            {
                //CaminhoLog = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("file:\\", "");
                USUARIO_SISTEMA = "Lara Neves Silva";
                
                string pathAssembly = Assembly.GetExecutingAssembly().Location;
                VERSAO_APLICACAO = FileVersionInfo.GetVersionInfo(pathAssembly).FileVersion;
                CAMINHO_EXECUCAO = Path.GetDirectoryName(pathAssembly);                
            }
            catch (Exception ex)
            {
                LogLara.EnfileirarLog(USUARIO_SISTEMA, "Erro ao carregar constantes.");
            }
        }
    }
}
