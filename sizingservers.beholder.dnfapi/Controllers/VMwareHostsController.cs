﻿/*
 * 2018 Sizing Servers Lab
 * University College of West-Flanders, Department GKG
 * 
 */

using System.Linq;
using System.Web.Http;

namespace sizingservers.beholder.dnfapi.Controllers {
    public class VMwareHostsController : ApiController {
        /// <summary>
        /// GET all stored host connection infos.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        [HttpGet]
        public Models.VMwareHostConnectionInfo[] List(string apiKey = null) {
            if (!Helpers.AuthorizationHelper.Authorize(apiKey))
                return null;

            return DA.VMwareHostConnectionInfosDA.GetAll().ToArray();
        }
        /// <summary>
        /// GET (poll) all system informations based on the host infos requested using the VMware SDK.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        [HttpGet]
        public Models.VMwareHostSystemInformation[] ListSystemInformation(string apiKey = null) {
            if (!Helpers.AuthorizationHelper.Authorize(apiKey))
                return null;

            return Helpers.Poller.PollVHSystemInformation();
        }
        /// <summary>
        /// Adds or update vmware host connection information.
        /// </summary>
        /// <param name="vmwareHostConnectionInfo"></param>
        /// <param name="apiKey">The API key.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddOrUpdate([FromBody]Models.VMwareHostConnectionInfo vmwareHostConnectionInfo, string apiKey = null) {
            if (!Helpers.AuthorizationHelper.Authorize(apiKey))
                return Unauthorized();

            DA.VMwareHostConnectionInfosDA.AddOrUpdate(vmwareHostConnectionInfo);

            return Created("list", typeof(string)); //Return a 201. Tell the client that the post did happen and were it can be requested.
        }
        /// <summary>
        /// Adds the or update comments.
        /// </summary>
        /// <param name="comments">The comments.</param>
        /// <param name="hostname">The hostname.</param>
        /// <param name="apiKey">The API key.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult AddOrUpdateComments([FromBody]string comments, string hostname, string apiKey = null) {
            if (!Helpers.AuthorizationHelper.Authorize(apiKey))
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (comments != null)
                try {
                    var systemInformation = DA.VMwareHostSystemInformationsDA.Get(hostname);
                    systemInformation.comments = comments;
                    DA.VMwareHostSystemInformationsDA.AddOrUpdate(systemInformation);
                }
                catch {
#warning handle this
                }
            return Created("list", typeof(string)); //Return a 201. Tell the client that the post did happen and were it can be requested.
        }
        /// <summary>
        /// Removes the connection info having the given ip or hostname, if any.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="hostname"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult Remove(string hostname, string apiKey = null) {
            if (!Helpers.AuthorizationHelper.Authorize(apiKey))
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            DA.VMwareHostSystemInformationsDA.Remove(hostname);
            DA.VMwareHostConnectionInfosDA.Remove(hostname);

            return StatusCode(System.Net.HttpStatusCode.NoContent); //Http PUT response --> 200 OK or 204 NoContent. Latter equals done.
        }
        /// <summary>
        /// Clear (PUT) all host connection infos in the database.
        /// <param name="apiKey"></param>
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult Clear(string apiKey = null) {
            if (!Helpers.AuthorizationHelper.Authorize(apiKey))
                return Unauthorized();

            DA.VMwareHostSystemInformationsDA.Clear();
            DA.VMwareHostConnectionInfosDA.Clear();

            return StatusCode(System.Net.HttpStatusCode.NoContent); //Http PUT response --> 200 OK or 204 NoContent. Latter equals done.
        }
    }
}