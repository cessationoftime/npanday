using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NPanday.Model
{

    public class Res
    {
        ResourceFolder resourceFolder;
        internal Res(DirectoryInfo SrcFolder)
        {
            string path = SrcFolder.FullName;

            path = Path.Combine(path, "resource");
            DirectoryInfo resource = new DirectoryInfo(path);
            if (resource.Exists) resourceFolder = new ResourceFolder(resource);
            else throw new FileNotFoundException("Resource folder (" + resource.FullName + ") could not be found");
        }

        /// <summary>
        /// Get Path to Resources folder
        /// </summary>
        /// <returns></returns>
        public ResourceFolder Folder()
        {

            return resourceFolder;

        }



        /// <summary>
        /// Get Path to specific Resources
        /// </summary>
        /// <returns></returns>
        public FileInfo File(string path, bool verifyExists)
        {
            return Folder().File(path, verifyExists);
        }

        /// <summary>
        /// Get Path to specific Resource file
        /// </summary>
        /// <returns></returns>
        public FileInfo File(string path)
        {
            return Folder().File(path);
        }


        public ResourceFolder Folder(string path, bool verifyExists)
        {
            return Folder().Folder(path, verifyExists);
        }

        public ResourceFolder Folder(string path)
        {
            return Folder().Folder(path);
        }


    }

    public class ResourceFolder
    {
        DirectoryInfo folder;
        public ResourceFolder(DirectoryInfo folder)
            : base()
        {
            this.folder = folder;
        }

        public DirectoryInfo Info
        {
            get
            {
                return folder;
            }
        }

        public string FullName
        {
            get
            {
                return folder.FullName;
            }
        }

        public string Name
        {
            get
            {
                return folder.Name;
            }
        }

        /// <summary>
        /// Get Path to specific Resources
        /// </summary>
        /// <returns></returns>
        public ResourceFolder Folder(string path, bool verifyExists)
        {
            string p = Path.Combine(folder.FullName, path);
            DirectoryInfo resource = new DirectoryInfo(p);
            if (!verifyExists) return new ResourceFolder(resource);
            if (resource.Exists) return new ResourceFolder(resource);
            else throw new FileNotFoundException("Resource folder (" + resource.FullName + ") could not be found");
        }

        /// <summary>
        /// Get Path to specific Resource folder
        /// </summary>
        /// <returns></returns>
        public ResourceFolder Folder(string path)
        {
            return Folder(path, true);
        }

        /// <summary>
        /// Get Path to specific Resources
        /// </summary>
        /// <returns></returns>
        public FileInfo File(string path, bool verifyExists)
        {
            string p = Path.Combine(folder.FullName, path);
            FileInfo resource = new FileInfo(p);
            if (!verifyExists) return resource;
            if (resource.Exists) return resource;
            else throw new FileNotFoundException("Resource file (" + resource.FullName + ") could not be found");
        }

        /// <summary>
        /// Get Path to specific Resource file
        /// </summary>
        /// <returns></returns>
        public FileInfo File(string path)
        {
            return File(path, true);
        }
    }
}
