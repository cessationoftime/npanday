//
// Licensed to the Apache Software Foundation (ASF) under one
// or more contributor license agreements.  See the NOTICE file
// distributed with this work for additional information
// regarding copyright ownership.  The ASF licenses this file
// to you under the Apache License, Version 2.0 (the
// "License"); you may not use this file except in compliance
// with the License.  You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing,
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
// KIND, either express or implied.  See the License for the
// specific language governing permissions and limitations
// under the License.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using NMaven.Core;
using NMaven.Core.Impl;
using NMaven.Plugin;

namespace NMaven.Plugin.Solution
{
	/// <summary>
	/// Description
	/// </summary>
	[Serializable]
	[ClassAttribute(Phase = "package", Goal = "solution")]
	public sealed class SolutionMojo : AbstractMojo
	{
		public SolutionMojo()
		{
		}
		
		[FieldAttribute("basedir", Expression = "${basedir}", Type = "java.lang.String")]
		public String basedir;
		
		[FieldAttribute("mavenProject", Expression = "${project}", Type = "org.apache.maven.project.MavenProject")]
		public NMaven.Model.Model mavenProject;		
		
		private String profile = null;
		
		public override Type GetMojoImplementationType()
		{
			return this.GetType();
		}
		
		public override void Execute()
		{
			IProjectGenerator projectGenerator = new ProjectGeneratorImpl();
            FileInfo pomFileInfo = new FileInfo(basedir + @"\pom.xml");
			List<IProjectReference> projectReferences = Execute(new DirectoryInfo(pomFileInfo.DirectoryName),
			                                                           mavenProject, profile);
			String solutionFile = (profile == null) ? pomFileInfo.DirectoryName + @"\" + @mavenProject.artifactId  + ".sln" :
			    pomFileInfo.DirectoryName + @"\" + @mavenProject.artifactId + "." + profile + ".sln";
		 	projectGenerator.GenerateSolutionFor(new FileInfo(solutionFile), projectReferences);
			Console.WriteLine("Solution Plugin Working: " + basedir + ",  Packaging = " + mavenProject.packaging);
			
		}

		public List<IProjectReference> Execute(DirectoryInfo currentDirectory, NMaven.Model.Model model, string profile)
		{	
			
			if(model == null)
			{
				throw new ExecutionException("NMAVEN-000-000: Model is null");
			}
			
			if(currentDirectory == null)
			{
				throw new ExecutionException("NMAVEN-000-000: Current directory is null");
			}
			if(!currentDirectory.Exists)
			{
				throw new ExecutionException("NMAVEN-000-000: Could not find current directory: Path = "
				    + currentDirectory.FullName);
			}
			
			List<IProjectReference> projectReferences = new List<IProjectReference>();
			IProjectGenerator projectGenerator = new ProjectGeneratorImpl();
			if(model.packaging.Equals("pom"))
			{
				foreach(String module in GetModulesForProfile(profile, model))
				{
					DirectoryInfo newDir = new DirectoryInfo(currentDirectory.FullName + @"\" + module );
					NMaven.Model.Model m = projectGenerator.CreatePomModelFor(newDir.FullName + @"\pom.xml");
					projectReferences.AddRange(Execute(newDir, m, profile));
				}					   	
			} 
			else
			{
				IProjectReference mainProjectReference = null;
				if(new DirectoryInfo(currentDirectory.FullName + @"\src\main\csharp\").Exists)
				{
					mainProjectReference = 
			 			projectGenerator.GenerateProjectFor(model,
			 	                                    new DirectoryInfo(currentDirectory.FullName + @"\src\main\csharp\"),
			 	                                    model.artifactId, null);
					Console.WriteLine("NMAVEN-000-000: Generated project: File Name = "
					                  + mainProjectReference.CsProjFile.FullName);							
					projectReferences.Add(mainProjectReference);
				}
				if(new DirectoryInfo( currentDirectory.FullName + @"\src\test\csharp\").Exists)
				{
					List<IProjectReference> mainRef = new List<IProjectReference>();
					if(mainProjectReference != null)
					{
						mainRef.Add(mainProjectReference);
					}
					IProjectReference projectReference = 
			 			projectGenerator.GenerateProjectFor(model,
			 	                                    new DirectoryInfo(currentDirectory.FullName + @"\src\test\csharp\"),
			 	                                    model.artifactId + "-Test", mainRef);
					Console.WriteLine("NMAVEN-000-000: Generated test project: File Name = "
					                  + projectReference.CsProjFile.FullName);					
					projectReferences.Add(projectReference);					
				}
			}	
			return projectReferences;
		}
		
		private string GetArgFor(string name, string[] args)
		{
			char[] delim = {'='};
			foreach(string arg in args)
			{
                string[] tokens = arg.Split(delim);
                if (tokens[0].Equals(name)) return tokens[1];
			}
            return null;
		}


		private string[] GetModulesForProfile(string profile,  NMaven.Model.Model model)
		{
			NMaven.Model.Profile[] profiles = model.profiles;
			if(profiles == null)
				return model.modules;

			foreach(NMaven.Model.Profile p in profiles)
			{
				if(p.activation.property.name.Equals(profile))
				{
					return p.modules;
				}
			}
			return model.modules;
        }
	}
}