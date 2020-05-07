//===============================================================================
// Microsoft FastTrack for Azure
// Video Playback Tracking Samples
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;
using VideoTracking.Web.Models;

namespace VideoTracking.Web.Controllers
{
    public class VideoTrackingController : ApiController
    {
        // POST api/<controller>
        public void Post([FromBody]string value)
        {
            string[] trackingData = value.Split(',');
            string url = trackingData[0];
            DateTime startDateTime = Convert.ToDateTime(trackingData[1]);
            DateTime endDateTime = Convert.ToDateTime(trackingData[2]);
            TimeSpan playTime = endDateTime.Subtract(startDateTime);

            // Write tracking information to database
            const string sql = "INSERT INTO [dbo].[VideoTracking] ([VideoTrackingId], [Url], [StartDateTime], [EndDateTime], [PlayTimeInSeconds]) VALUES (@VideoTrackingId, @Url, @StartDateTime, @EndDateTime, @PlayTimeInSeconds)";
            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlHelper sqlHelper = null;

            parameters.Add(new SqlParameter("@VideoTrackingId", Guid.NewGuid()));
            parameters.Add(new SqlParameter("@Url", url));
            parameters.Add(new SqlParameter("@StartDateTime", startDateTime));
            parameters.Add(new SqlParameter("@EndDateTime", endDateTime));
            parameters.Add(new SqlParameter("@PlayTimeInSeconds", Convert.ToInt32(playTime.TotalSeconds)));
            try
            {
                sqlHelper = new SqlHelper();
                sqlHelper.Execute(sql, CommandType.Text, ref parameters);
            }
            catch (Exception)
            {
                // Implement your logging, etc. here
                throw;
            }
            finally
            {
                if (sqlHelper != null) sqlHelper.Close();
            }
        }
    }
}