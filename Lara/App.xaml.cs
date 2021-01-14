using Lara.Data;
using Lara.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Lara
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                if (e.IsTerminating)
                {
                    try { TrataExcecao(e); } catch { }
                }
            };

            using var db = new DBContexto();
            db.Database.Migrate();
        }

        #region Metodos

        private void TrataExcecao(System.UnhandledExceptionEventArgs e)
        {
            try
            {
                if (e.ExceptionObject is Exception)
                {
                    // Criar envio para Fila RaabitMQ ou Api
                }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(DateTime.Now.ToString());
                sb.AppendLine("Houve um erro interno extremo dentro do Lara_MP e ele não pode continuar.");
                sb.AppendLine("Abaixo estão todas informações que ele conseguiu coletar.");
                sb.AppendLine("Versão Lara_MP:" + Constantes.VERSAO_APLICACAO);
                sb.AppendLine(e.ExceptionObject.ToString());

                Constantes.LogLara.EnfileirarLogErro(sb.ToString(), (Exception)e.ExceptionObject);
            }
            catch (Exception ex)
            {
                Constantes.LogLara.EnfileirarLogErro("Erro em TrataExcecao", ex);
            }
        }

        #endregion

        #region Invokes
        public static void Invoke(Action action)
        {
            Invoke(Lara.MainWindow.RetDispatcher(), action);
        }

        public static void Invoke<T1>(Action<T1> action, T1 arg1)
        {
            Invoke(Lara.MainWindow.RetDispatcher(), action, arg1);
        }

        public static void Invoke<T1, T2>(Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            Invoke(Lara.MainWindow.RetDispatcher(), action, arg1, arg2);
        }

        public static void Invoke<T1, T2, T3>(Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
        {
            Invoke(Lara.MainWindow.RetDispatcher(), action, arg1, arg2, arg3);
        }

        public static void Invoke<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            Invoke(Lara.MainWindow.RetDispatcher(), action, arg1, arg2, arg3, arg4);
        }

        public static TResult Invoke<TResult>(Func<TResult> func)
        {
            return Invoke(Lara.MainWindow.RetDispatcher(), func);
        }

        public static TResult Invoke<T, TResult>(Func<T, TResult> func, T arg)
        {
            return Invoke(Lara.MainWindow.RetDispatcher(), func, arg);
        }

        public static TResult Invoke<T1, T2, TResult>(Func<T1, T2, TResult> func, T1 arg1, T2 arg2)
        {
            return Invoke(Lara.MainWindow.RetDispatcher(), func, arg1, arg2);
        }

        public static TResult Invoke<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func, T1 arg1, T2 arg2, T3 arg3)
        {
            return Invoke(Lara.MainWindow.RetDispatcher(), func, arg1, arg2, arg3);
        }

        public static TResult Invoke<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return Invoke(Lara.MainWindow.RetDispatcher(), func, arg1, arg2, arg3, arg4);
        }

        public static void Invoke(Dispatcher dispatcher, Action action)
        {
            if (!dispatcher.CheckAccess())
            {
                dispatcher.Invoke(DispatcherPriority.Normal, action);
                return;
            }

            action.Invoke();
        }

        public static void Invoke<T1>(Dispatcher dispatcher, Action<T1> action, T1 arg1)
        {
            if (!dispatcher.CheckAccess())
            {
                dispatcher.Invoke(DispatcherPriority.Normal, action, arg1);
                return;
            }

            action.Invoke(arg1);
        }

        public static void Invoke<T1, T2>(Dispatcher dispatcher, Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            if (!dispatcher.CheckAccess())
            {
                dispatcher.Invoke(DispatcherPriority.Normal, action, arg1, arg2);
                return;
            }

            action.Invoke(arg1, arg2);
        }

        public static void Invoke<T1, T2, T3>(Dispatcher dispatcher, Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
        {
            if (!dispatcher.CheckAccess())
            {
                dispatcher.Invoke(DispatcherPriority.Normal, action, arg1, arg2, arg3);
                return;
            }

            action.Invoke(arg1, arg2, arg3);
        }

        public static void Invoke<T1, T2, T3, T4>(Dispatcher dispatcher, Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (!dispatcher.CheckAccess())
            {
                dispatcher.Invoke(DispatcherPriority.Normal, action, arg1, arg2, arg3, arg4);
                return;
            }

            action.Invoke(arg1, arg2, arg3, arg4);
        }

        public static TResult Invoke<TResult>(Dispatcher dispatcher, Func<TResult> func)
        {
            if (!dispatcher.CheckAccess())
            {
                return (TResult)dispatcher.Invoke(DispatcherPriority.Normal, func);
            }

            return func.Invoke();
        }

        public static TResult Invoke<T, TResult>(Dispatcher dispatcher, Func<T, TResult> func, T arg)
        {
            if (!dispatcher.CheckAccess())
            {
                return (TResult)dispatcher.Invoke(DispatcherPriority.Normal, func, arg);
            }

            return func.Invoke(arg);
        }

        public static TResult Invoke<T1, T2, TResult>(Dispatcher dispatcher, Func<T1, T2, TResult> func, T1 arg1, T2 arg2)
        {
            if (!dispatcher.CheckAccess())
            {
                return (TResult)dispatcher.Invoke(DispatcherPriority.Normal, func, arg1, arg2);
            }

            return func.Invoke(arg1, arg2);
        }

        public static TResult Invoke<T1, T2, T3, TResult>(Dispatcher dispatcher, Func<T1, T2, T3, TResult> func, T1 arg1, T2 arg2, T3 arg3)
        {
            if (!dispatcher.CheckAccess())
            {
                return (TResult)dispatcher.Invoke(DispatcherPriority.Normal, func, arg1, arg2, arg3);
            }

            return func.Invoke(arg1, arg2, arg3);
        }

        public static TResult Invoke<T1, T2, T3, T4, TResult>(Dispatcher dispatcher, Func<T1, T2, T3, T4, TResult> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (!dispatcher.CheckAccess())
            {
                return (TResult)dispatcher.Invoke(DispatcherPriority.Normal, func, arg1, arg2, arg3, arg4);
            }

            return func.Invoke(arg1, arg2, arg3, arg4);
        }
        #endregion

        #region Threading

        /// <summary>
        /// Executa a ação num thread separado
        /// </summary>
        /// <param name="action"></param>
        public static Thread ExecutarThread(Action action)
        {
            return ExecutarThread(null, action, null);
        }

        /// <summary>
        /// Executa a ação num thread separado, e chama a ação callback no thread original
        /// </summary>
        /// <param name="action"></param>
        /// <param name="callback"></param>
        public static Thread ExecutarThread(Action action, Action callback)
        {
            return ExecutarThread(null, action, callback);
        }

        /// <summary>
        /// Chama preAction no thread original, depois executa action num novo thread, e depois chama callback no thread original
        /// </summary>
        /// <param name="preAction"></param>
        /// <param name="action"></param>
        /// <param name="callback"></param>
        public static Thread ExecutarThread(Action preAction, Action action, Action callback)
        {
            preAction?.Invoke();

            Thread thread = new Thread(() =>
            {
                action.Invoke();

                if (callback != null)
                {
                    Invoke(callback);
                }
            });
            thread.Start();

            return thread;
        }

        #endregion
    }
}
