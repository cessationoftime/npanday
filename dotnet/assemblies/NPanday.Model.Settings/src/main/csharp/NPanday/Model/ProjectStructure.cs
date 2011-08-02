using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NPanday.Model
{
    public class ProjectStructure
    {
        string ProjectName;
        readonly DirectoryInfo projectFolder;
        Res  testResource;
        public ProjectStructure(string ProjectFolderName)
        {
            this.ProjectName = ProjectFolderName;


            //set the Projectfolder
            string path = Directory.GetCurrentDirectory();
            int index = path.IndexOf(ProjectName);
            DirectoryInfo pFolder;
            if (index > 0)
            {
                pFolder = new DirectoryInfo(path.Remove(index) + ProjectName);

            }
            else
            {
                pFolder = new DirectoryInfo(path);
            }

            if (pFolder.Exists) projectFolder = pFolder;
            else throw new FileNotFoundException("Project folder (" + pFolder + ") could not be found");
        }

        /// <summary>
        /// Path to the ProjectFolder
        /// </summary>
        /// <param name="targetProject"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public DirectoryInfo ProjectFolder
        {
            get
            {
                return projectFolder;
            }
        }

        /// <summary>
        /// Represents the TestResources
        /// </summary>
        public Res TestResource
        {
            get
            {
                if (testResource == null)
                {
                    testResource = new Res(ProjectFolder);
                }
                return testResource;
            }
        }

        public class Res
        {
            ResourceFolder resourceFolder;
            internal Res(DirectoryInfo ProjectFolder)
            {
                string path = ProjectFolder.FullName;

                path = Path.Combine(path, "src");
                path = Path.Combine(path, "test");
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
          

            public class ResourceFolder
            {
                DirectoryInfo folder;
                public ResourceFolder(DirectoryInfo folder) : base()
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

    }
}

