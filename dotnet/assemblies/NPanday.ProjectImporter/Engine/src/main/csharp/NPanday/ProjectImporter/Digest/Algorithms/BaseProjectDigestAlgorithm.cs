﻿#region Apache License, Version 2.0
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
#endregion
using Microsoft.Build.BuildEngine;
using System.IO;

namespace NPanday.ProjectImporter.Digest.Algorithms
{
    public class BaseProjectDigestAlgorithm
    {
        public static string GetProjectAssemblyName(string projectFile)
        {
            Project project = ProjectDigester.GetProject(projectFile);

            if (project == null)
            {
                if (projectFile != null)
                    return Path.GetFileNameWithoutExtension(projectFile);

                return null;
            }

            foreach (BuildPropertyGroup buildPropertyGroup in project.PropertyGroups)
            {
                foreach (BuildProperty buildProperty in buildPropertyGroup)
                {
                    if (!buildProperty.IsImported && "AssemblyName".Equals(buildProperty.Name))
                    {
                        return buildProperty.Value;
                    }

                }
            }

            return null;
        }
    }
}