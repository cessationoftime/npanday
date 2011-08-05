using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NPanday.Model
{
    /// <summary>
    /// Like SrcStructure, but does not depend on EnvDTE and VSLangProj
    /// </summary>
    public class SimpleSrcStructure
    {
        Res resources;
        DirectoryInfo srcFolder;
        protected DirectoryInfo projectFolder;
        public SimpleSrcStructure(MainOrTest mainOrTest,string ProjectName)
        {
            string mainTest = Enum.GetName(typeof(MainOrTest), mainOrTest).ToLower();

            DirectoryInfo workingDirectory = new DirectoryInfo(Environment.CurrentDirectory);


            if (!Environment.CurrentDirectory.Contains(ProjectName)) throw new Exception("given ProjectName: "+ProjectName+" not contained in the Currentdirectory: " + Environment.CurrentDirectory);

            Console.WriteLine(Environment.CurrentDirectory);

            do
            {
                workingDirectory = workingDirectory.Parent;
            } while (workingDirectory.Name != ProjectName);

            if (workingDirectory.Name != ProjectName) throw new Exception("Working directory Name does not == Projectname");

            //workingDirectory should now be the project folder.

            projectFolder = new DirectoryInfo(workingDirectory.FullName);
            srcFolder = new DirectoryInfo(workingDirectory.FullName + "\\src\\" + mainTest);
        }

        internal SimpleSrcStructure(string FullPathTo_CSPROJ_File)
        {
            srcFolder = new FileInfo(FullPathTo_CSPROJ_File).Directory.Parent;
            if (srcFolder.Name == "test" || srcFolder.Name == "main") projectFolder = srcFolder.Parent.Parent;
        }


        public bool isMainSrc
        {
            get
            {
                return (srcFolder.FullName.Contains("\\main"));
            }
        }

        public bool isTestSrc
        {
            get
            {
                return (srcFolder.FullName.Contains("\\test"));
            }
        }

        /// <summary>
        /// Represents the TestResources
        /// </summary>
        public Res Resources
        {
            get
            {
                if (resources == null)
                {
                    resources = new Res(srcFolder);
                }
                return resources;
            }
        }


     


        public DirectoryInfo SrcFolder
        {
            get
            {
                return srcFolder;
            }
        }

        public DirectoryInfo ProjectFolder
        {
            get
            {
                return projectFolder;
            }
        }

    }

}
