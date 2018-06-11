﻿/*
 * 2018 Sizing Servers Lab
 * University College of West-Flanders, Department GKG
 * 
 */

using System.ComponentModel.DataAnnotations;

namespace sizingservers.beholder.dnfapi.Models {
    public class VMwareHostConnectionInfo {
        [Key]
        public string ipOrHostname { get; set; }
        /// <summary>
        /// For when the current machine is a ESXi (>= 6.5) host. Assign quest ips to this range so a more usable UI can be build.
        /// </summary>
        public string guestHostnames { get; set; }
        /// <summary>
        /// For when the current machine is a ESXi (>= 6.5) host. Be carefull with credentials.
        /// We are interested in Vim25.HostHardwareInfo so only the property "HostSystem" with the path "hardware" should be accesible via https://ip of current machine/sdk using the Vim25 API (VMWare SDK 6.7).
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// For when the current machine is a ESXi (>= 6.5) host. Be carefull with credentials. 
        /// We are interested in Vim25.HostHardwareInfo so only the property "HostSystem" with the path "hardware" should be accesible via https://ip of current machine/sdk using the Vim25 API (VMWare SDK 6.7).
        /// </summary>
        public string password { get; set; }

    }
}