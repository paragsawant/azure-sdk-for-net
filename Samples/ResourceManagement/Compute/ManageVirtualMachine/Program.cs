﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Azure.Management.Compute.Fluent;
using Microsoft.Azure.Management.Compute.Fluent.Models;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.Resource.Fluent;
using Microsoft.Azure.Management.Resource.Fluent.Authentication;
using Microsoft.Azure.Management.Resource.Fluent.Core;
using Microsoft.Azure.Management.Samples.Common;
using System;
using System.Linq;

namespace ManageVirtualMachine
{
    public class Program
    {
        private const string userName = "tirekicker";
        private const string password = "12NewPA$$w0rd!";
        private const string dataDiskName = "disk2";

        /**
         * Azure Compute sample for managing virtual machines -
         *  - Create a virtual machine
         *  - Start a virtual machine
         *  - Stop a virtual machine
         *  - Restart a virtual machine
         *  - Update a virtual machine
         *    - Expand the OS drive
         *    - Tag a virtual machine (there are many possible variations here)
         *    - Attach data disks
         *    - Detach data disks
         *  - List virtual machines
         *  - Delete a virtual machine.
         */
        public static void RunSample(IAzure azure)
        {
            string rgName = SharedSettings.RandomResourceName("rgCOMV", 24);
            string windowsVMName = SharedSettings.RandomResourceName("wVM", 24);
            string linuxVMName = SharedSettings.RandomResourceName("lVM", 24);
            try
            {
                var startTime = DateTimeOffset.Now.UtcDateTime;

                var windowsVM = azure.VirtualMachines.Define(windowsVMName)
                        .WithRegion(Region.US_EAST)
                        .WithNewResourceGroup(rgName)
                        .WithNewPrimaryNetwork("10.0.0.0/28")
                        .WithPrimaryPrivateIpAddressDynamic()
                        .WithoutPrimaryPublicIpAddress()
                        .WithPopularWindowsImage(KnownWindowsVirtualMachineImage.WINDOWS_SERVER_2012_R2_DATACENTER)
                        .WithAdminUsername(userName)
                        .WithAdminPassword(password)
                        .WithSize(VirtualMachineSizeTypes.StandardD3V2)
                        .Create();
                var endTime = DateTimeOffset.Now.UtcDateTime;

                Utilities.Log($"Created VM: took {(endTime - startTime).TotalSeconds} seconds");

                Utilities.PrintVirtualMachine(windowsVM);

                windowsVM.Update()
                        .WithTag("who-rocks", "open source")
                        .WithTag("where", "on azure")
                        .Apply();

                Utilities.Log("Tagged VM: " + windowsVM.Id);

                //=============================================================
                // Update - Attach data disks

                windowsVM.Update()
                        .WithNewDataDisk(10)
                        .DefineNewDataDisk(dataDiskName)
                            .WithSizeInGB(20)
                            .WithCaching(CachingTypes.ReadWrite)
                            .Attach()
                        .Apply();

                Utilities.Log("Attached a new data disk" + dataDiskName + " to VM" + windowsVM.Id);
                Utilities.PrintVirtualMachine(windowsVM);

                windowsVM.Update()
                    .WithoutDataDisk(dataDiskName)
                    .Apply();

                Utilities.Log("Detached data disk " + dataDiskName + " from VM " + windowsVM.Id);

                //=============================================================
                // Update - Resize (expand) the data disk
                // First, deallocate the virtual machine and then proceed with resize

                Utilities.Log("De-allocating VM: " + windowsVM.Id);

                windowsVM.Deallocate();

                Utilities.Log("De-allocated VM: " + windowsVM.Id);

                var dataDisk = windowsVM.DataDisks.First();

                windowsVM.Update()
                            .UpdateDataDisk(dataDisk.Name)
                            .WithSizeInGB(30)
                            .Parent()
                        .Apply();

                //=============================================================
                // Update - Expand the OS drive size by 10 GB

                int osDiskSizeInGb = windowsVM.OsDiskSize;
                if (osDiskSizeInGb == 0)
                {
                    // Server is not returning the OS Disk size, possible bug in server
                    Utilities.Log("Server is not returning the OS disk size, possible bug in the server?");
                    Utilities.Log("Assuming that the OS disk size is 256 GB");
                    osDiskSizeInGb = 256;
                }

                windowsVM.Update()
                        .WithOsDiskSizeInGb(osDiskSizeInGb + 10)
                        .Apply();

                Utilities.Log("Expanded VM " + windowsVM.Id + "'s OS disk to " + (osDiskSizeInGb + 10));

                //=============================================================
                // Start the virtual machine

                Utilities.Log("Starting VM " + windowsVM.Id);

                windowsVM.Start();

                Utilities.Log("Started VM: " + windowsVM.Id + "; state = " + windowsVM.PowerState);

                //=============================================================
                // Restart the virtual machine

                Utilities.Log("Restarting VM: " + windowsVM.Id);

                windowsVM.Restart();

                Utilities.Log("Restarted VM: " + windowsVM.Id + "; state = " + windowsVM.PowerState);

                //=============================================================
                // Stop (powerOff) the virtual machine

                Utilities.Log("Powering OFF VM: " + windowsVM.Id);

                windowsVM.PowerOff();

                Utilities.Log("Powered OFF VM: " + windowsVM.Id + "; state = " + windowsVM.PowerState);

                // Get the network where Windows VM is hosted
                var network = windowsVM.GetPrimaryNetworkInterface().PrimaryIpConfiguration.GetNetwork();

                //=============================================================
                // Create a Linux VM in the same virtual network

                Utilities.Log("Creating a Linux VM in the network");

                var linuxVM = azure.VirtualMachines.Define(linuxVMName)
                        .WithRegion(Region.US_EAST)
                        .WithExistingResourceGroup(rgName)
                        .WithExistingPrimaryNetwork(network)
                        .WithSubnet("subnet1") // Referencing the default subnet name when no name specified at creation
                        .WithPrimaryPrivateIpAddressDynamic()
                        .WithoutPrimaryPublicIpAddress()
                        .WithPopularLinuxImage(KnownLinuxVirtualMachineImage.UBUNTU_SERVER_16_04_LTS)
                        .WithRootUsername(userName)
                        .WithRootPassword(password)
                        .WithSize(VirtualMachineSizeTypes.StandardD3V2)
                        .Create();

                Utilities.Log("Created a Linux VM (in the same virtual network): " + linuxVM.Id);
                Utilities.PrintVirtualMachine(linuxVM);

                //=============================================================
                // List virtual machines in the resource group

                var resourceGroupName = windowsVM.ResourceGroupName;

                Utilities.Log("Printing list of VMs =======");

                foreach (var virtualMachine in azure.VirtualMachines.ListByGroup(resourceGroupName))
                {
                    Utilities.PrintVirtualMachine(virtualMachine);
                }

                //=============================================================
                // Delete the virtual machine
                Utilities.Log("Deleting VM: " + windowsVM.Id);

                azure.VirtualMachines.DeleteById(windowsVM.Id);

                Utilities.Log("Deleted VM: " + windowsVM.Id);
            }
            finally
            {
                try
                {
                    Utilities.Log("Deleting Resource Group: " + rgName);
                    azure.ResourceGroups.DeleteByName(rgName);
                    Utilities.Log("Deleted Resource Group: " + rgName);
                }
                catch (Exception ex)
                {
                    Utilities.Log(ex);
                }
            }
        }

        public static void Main(string[] args)
        {
            try
            {
                //=============================================================
                // Authenticate
                AzureCredentials credentials = SharedSettings.AzureCredentialsFactory.FromFile(Environment.GetEnvironmentVariable("AZURE_AUTH_LOCATION"));

                var azure = Azure
                    .Configure()
                    .WithLogLevel(HttpLoggingDelegatingHandler.Level.BASIC)
                    .Authenticate(credentials)
                    .WithDefaultSubscription();

                // Print selected subscription
                Utilities.Log("Selected subscription: " + azure.SubscriptionId);

                RunSample(azure);
            }
            catch (Exception ex)
            {
                Utilities.Log(ex);
            }
        }
    }
}