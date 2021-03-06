﻿/**
 * Kopernicus Planetary System Modifier
 * Copyright (C) 2014 Bryce C Schroeder (bryce.schroeder@gmail.com), Nathaniel R. Lewis (linux.robotdude@gmail.com)
 * 
 * http://www.ferazelhosting.net/~bryce/contact.html
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,
 * MA 02110-1301  USA
 * 
 * This library is intended to be used as a plugin for Kerbal Space Program
 * which is copyright 2011-2014 Squad. Your usage of Kerbal Space Program
 * itself is governed by the terms of its EULA, not the license above.
 * 
 * https://kerbalspaceprogram.com
 */

using System;
using System.IO;

using UnityEngine;

namespace Kopernicus
{
	// A message logging class to replace Debug.Log
	public class Logger
	{
		// Logger output path
		private static string LogDirectory 
		{
			get { return KSPUtil.ApplicationRootPath + "Logs/"; }
		}

		// Default logger
		private static Logger _DefaultLogger = null;
		public static Logger Default 
		{
			get 
			{
				if (_DefaultLogger == null)
					_DefaultLogger = new Logger ();
				return _DefaultLogger;
			}
		}

		// Currently active logger
		public static Logger Active { get ; private set; }

		// The complete path of this log
		TextWriter loggerStream;

		// Write text to the log
		public void Log(object o)
		{
			loggerStream.WriteLine ("[LOG " + DateTime.Now.ToString ("HH:mm:ss") + "]: " + o);
		}

		// Write text to the log
		public void LogException(Exception e)
		{
			loggerStream.WriteLine ("[LOG " + DateTime.Now.ToString ("HH:mm:ss") + "]: Exception Was Recorded: " + e.Message + "\n" + e.StackTrace);

			if(e.InnerException != null)
				loggerStream.WriteLine ("[LOG " + DateTime.Now.ToString ("HH:mm:ss") + "]: Inner Exception Was Recorded: " + e.InnerException.Message + "\n" + e.InnerException.StackTrace);
		}

		// Set logger as the active logger
		public void SetAsActive()
		{
			Logger.Active = this;
		}

		public void Flush()
		{
			loggerStream.Flush ();
		}

		// Close the logger
		public void Close()
		{
			loggerStream.Flush ();
			loggerStream.Close ();
		}

		// Create a logger
		public Logger (string LogFileName = "Kopernicus")
		{
			// Open the log file (overwrite existing logs)
			Directory.CreateDirectory (Logger.LogDirectory);
			string LogFile = Logger.LogDirectory + LogFileName + ".log";
			loggerStream = new StreamWriter(File.Create (LogFile));

			// Write an opening message
			Log ("Logger \"" + LogFileName + "\" was created");
		}

		// Cleanup the logger
		~Logger()
		{
			loggerStream.Flush ();
			loggerStream.Close ();
		}
	}
}

