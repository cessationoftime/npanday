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
package npanday.vendor.impl;

import junit.framework.TestCase;

import java.lang.reflect.Field;
import java.util.List;
import java.util.ArrayList;
import java.io.File;

import npanday.model.settings.DefaultSetup;
import npanday.model.settings.Framework;
import npanday.model.settings.Vendor;
import npanday.vendor.VendorTestFactory;
import npanday.PlatformUnsupportedException;

public class SettingsRepositoryTest
    extends TestCase
{
    public void testGetInstallRoot()
    {
        DefaultSetup defaultSetup = VendorTestFactory.getDefaultSetup( "MICROSOFT", "2.0.50727", "2.0.50727" );

        //Supported Types
        List<Vendor> vendors = new ArrayList<Vendor>();
        Vendor vendor = new Vendor();
        vendor.setVendorName("MICROSOFT");
        vendor.setVendorVersion("2.0.50727");

        Framework framework = new Framework();
        framework.setFrameworkVersion("2.0.50727");
        framework.setInstallRoot(System.getenv("SystemRoot") + "\\Microsoft.NET\\Framework\\v2.0.50727");
        framework.setSdkInstallRoot(System.getenv("SystemDrive") + "\\Program Files\\Microsoft.NET\\SDK\\v2.0");
        vendor.addFramework( framework );

        vendors.add( vendor );

        SettingsRepository settingsRepository = Factory.createSettingsRepository( vendors, defaultSetup );
        try
        {
            File installRoot = settingsRepository.getInstallRootFor(  "MICROSOFT", "2.0.50727", "2.0.50727");
            assertEquals( new File(System.getenv("SystemRoot") + "\\Microsoft.NET\\Framework\\v2.0.50727"), installRoot );
        }
        catch ( PlatformUnsupportedException e )
        {
            fail("Unsupported Platform: Message = " + e.getMessage());
        }
    }

    private static class Factory
    {
        static SettingsRepository createSettingsRepository( List<Vendor> vendors, DefaultSetup defaultSetup )
        {
            SettingsRepository settingsRepository = new SettingsRepository();
            try
            {
                Field defaultSetupField = settingsRepository.getClass().getDeclaredField( "defaultSetup" );
                defaultSetupField.setAccessible( true );
                defaultSetupField.set( settingsRepository, defaultSetup );

                Field vendorsField = settingsRepository.getClass().getDeclaredField( "vendors" );
                vendorsField.setAccessible( true );
                vendorsField.set( settingsRepository, vendors );
            }
            catch ( NoSuchFieldException e )
            {
                e.printStackTrace();
            }
            catch ( IllegalAccessException e )
            {
                e.printStackTrace();
            }
            return settingsRepository;
        }
    }
}
