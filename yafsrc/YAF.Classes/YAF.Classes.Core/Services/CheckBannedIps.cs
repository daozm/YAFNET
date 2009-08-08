﻿/* YetAnotherForum.NET
 * Copyright (C) 2006-2009 Jaben Cargman
 * http://www.yetanotherforum.net/
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using YAF.Classes.Data;
using YAF.Classes.Utils;

namespace YAF.Classes.Core
{
	public class YafCheckBannedIps : InitService
	{
		protected const string _initVarName = "YafCheckBannedIps_Init";

		protected override string InitVarName
		{
			get
			{
				return "YafCheckBannedIps_Init";
			}
		}

		protected override bool RunService()
		{
			string key = YafCache.GetBoardCacheKey(Constants.Cache.BannedIP);

			// load the banned IP table...
			DataTable bannedIPs = (DataTable)YafContext.Current.Cache[key];

			if (bannedIPs == null)
			{
				// load the table and cache it...
				bannedIPs = DB.bannedip_list(YafContext.Current.PageBoardID, null);
				YafContext.Current.Cache[key] = bannedIPs;
			}

			// check for this user in the list...
			foreach (DataRow row in bannedIPs.Rows)
			{
				if (IPHelper.IsBanned((string)row["Mask"], HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]))
					HttpContext.Current.Response.End();
			}

			return true;
		}
	}
}
