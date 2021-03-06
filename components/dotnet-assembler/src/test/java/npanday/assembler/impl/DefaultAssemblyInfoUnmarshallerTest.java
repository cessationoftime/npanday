/*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */
package npanday.assembler.impl;

import junit.framework.TestCase;

import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;

import npanday.assembler.AssemblyInfo;

public class DefaultAssemblyInfoUnmarshallerTest
    extends TestCase
{
    private static String basedir = System.getProperty( "basedir" );

    public void testUnmarshall()
    {
        FileInputStream fis = null;
        try
        {
            fis = new FileInputStream(
                "src/test/resources/AssemblyInfo.cs" );
        }
        catch ( FileNotFoundException e )
        {
            fail("Could not find test file");
        }

        DefaultAssemblyInfoMarshaller um = new DefaultAssemblyInfoMarshaller();
        try
        {
            AssemblyInfo assemblyInfo = um.unmarshall( fis );
            assertEquals( "Incorrect Assembly Version", "1.0.0", assemblyInfo.getVersion());
        }
        catch ( Exception e )
        {
            fail("Problem iwht reading the assembly info input");
        }
    }
}
