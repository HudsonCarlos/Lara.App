using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Lara.Util
{
    public static class Constantes
    {
        public static string USUARIO_LOGADO { get; set; }
        public static string USUARIO_SISTEMA { get; private set; }
        public static string VERSAO_APLICACAO { get; private set; }
        public static MainWindow INSTANCIA_LARA { get; set; }

        private static string _caminho_execucao;
        public static string CAMINHO_EXECUCAO
        {
            get { return _caminho_execucao; }
            private set
            {
                AssistenteFilePathIO.AddPermissionsToDirectory(value);
                _caminho_execucao = value;
            }
        }

        private static Log _logLara;
        public static Log LogLara
        {
            get
            {
                if (_logLara == null)
                    _logLara = new Log();
                return _logLara;
            }
        }

        private static string _logExecucao;
        public static string LogExecucao
        {
            get { return _logExecucao; }
            private set
            {
                _logExecucao = Path.Combine(CAMINHO_EXECUCAO, $"log_execucao_{DateTime.Now:ddMMyyyy}.log");

                if (!File.Exists(_logExecucao))
                    File.Create(_logExecucao);
            }
        }

        public static void CarregarConstantes()
        {
            try
            {
                USUARIO_SISTEMA = "Lara Neves Silva";

                string pathAssembly = Assembly.GetExecutingAssembly().Location;
                VERSAO_APLICACAO = FileVersionInfo.GetVersionInfo(pathAssembly).FileVersion;
                CAMINHO_EXECUCAO = Path.GetDirectoryName(pathAssembly);
            }
            catch (Exception ex)
            {
                LogLara.EnfileirarLogErro("Erro ao carregar constantes.", ex);
            }
        }
    }
}
