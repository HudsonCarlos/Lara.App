using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;

namespace Lara.Util
{
    static class AssistenteFilePathIO
    {
        public static bool CopyFile(string origin, string destination)
        {
            return CopyFile(origin, destination, 0, new FileInfo(origin).Length);
        }

        private static bool CopyFile(string origin, string destination, long start, long totalSize)
        {
            try
            {
                string fileName = Path.GetFileName(origin);

                using (FileStream fsR = File.OpenRead(origin))
                using (FileStream fsW = new FileStream(Path.Combine(destination, fileName), FileMode.Create))
                {
                    fsR.CopyTo(fsW);
                    return true;
                }
            }
            catch (System.Exception e)
            {
                return false;
            }
        }

        public static bool CopyDirectory(string origin, string destination)
        {
            return CopyFiles(Directory.GetFiles(origin), destination);
        }

        public static bool CopyFiles(string[] origins, string destination)
        {
            try
            {
                long folderSize = GetTotalFolderSize(origins);

                long total = 0;
                foreach (string file in origins)
                {
                    if (!CopyFile(file, destination, total, folderSize))
                    {
                        return false;
                    }
                    total += new FileInfo(file).Length;
                }

                return true;
            }
            catch (System.Exception e)
            {
                return false;
            }
        }

        public static long GetTotalFolderSize(string[] folderPath)
        {
            long total = 0;

            foreach (string file in folderPath)
            {
                total += new FileInfo(file).Length;
            }

            return total;
        }

        public static void DeleteFile(string file)
        {
            if (File.Exists(file))
            {
                AddPermissionsToFile(file);
                File.Delete(file);
            }
        }

        public static void DeleteDirectory(string file)
        {
            if (Directory.Exists(file))
            {
                AddPermissionsToDirectory(file);
                Directory.Delete(file, true);
            }
        }

        public static void AddPermissionsToDirectory(string directory, bool CriarPasta = false)
        {
            if (!Directory.Exists(Path.GetDirectoryName(directory)) && !CriarPasta)
                return;
            else if (CriarPasta)
                Directory.CreateDirectory(Path.GetDirectoryName(directory));

            try
            {
                // Tenta dar controle total para 'Todos'
                DirectorySecurity ds = new DirectorySecurity();
                ds.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl,
                    InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));

                FileSystemAclExtensions.SetAccessControl(new DirectoryInfo(directory), ds);
            }
            catch
            {
                try
                {
                    // Tenta dar controle total para 'Administradores'
                    DirectorySecurity ds = new DirectorySecurity();
                    ds.AddAccessRule(new FileSystemAccessRule(WindowsIdentity.GetCurrent().Name, FileSystemRights.FullControl,
                        InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                    ds.SetOwner(new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null));

                    FileSystemAclExtensions.SetAccessControl(new DirectoryInfo(directory), ds);
                }
                catch { }
            }

            foreach (string file in Directory.GetFiles(directory))
            {
                AddPermissionsToFile(file);
            }
        }

        public static void AddPermissionsToFile(string file)
        {
            // Se o arquivo não existe terminamos aqui
            if (!File.Exists(file))
                return;

            try
            {
                // Seta o arquivo como 'Normal'
                File.SetAttributes(file, FileAttributes.Normal);

                // Tenta dar controle total para 'Todos'
                FileSecurity fs = new FileSecurity();
                fs.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, AccessControlType.Allow));

                FileSystemAclExtensions.SetAccessControl(new FileInfo(file), fs);
            }
            catch
            {
                try
                {
                    // Seta o arquivo como 'Normal'
                    File.SetAttributes(file, FileAttributes.Normal);

                    // Tenta dar control total para os 'Administradores'
                    FileSecurity fs = new FileSecurity();
                    fs.AddAccessRule(new FileSystemAccessRule(WindowsIdentity.GetCurrent().Name, FileSystemRights.FullControl, AccessControlType.Allow));
                    fs.SetOwner(new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null));

                    FileSystemAclExtensions.SetAccessControl(new FileInfo(file), fs);
                }
                catch { }
            }
        }

        public static byte[] FileToBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        public static string FileToString(string path, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.GetEncoding(1252);
            }

            return File.ReadAllText(path, encoding);
        }

        public static void StringToFile(string path, string str, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.GetEncoding(1252);
            }

            File.WriteAllText(path, str, encoding);
        }

        public static string[] FileToStrings(string path, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.GetEncoding(1252);
            }

            return File.ReadAllLines(path, encoding);
        }
    }
}
