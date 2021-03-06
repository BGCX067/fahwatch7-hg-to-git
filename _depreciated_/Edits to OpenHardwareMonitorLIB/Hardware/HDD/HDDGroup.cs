﻿/*
  
  Version: MPL 1.1/GPL 2.0/LGPL 2.1

  The contents of this file are subject to the Mozilla Public License Version
  1.1 (the "License"); you may not use this file except in compliance with
  the License. You may obtain a copy of the License at
 
  http://www.mozilla.org/MPL/

  Software distributed under the License is distributed on an "AS IS" basis,
  WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
  for the specific language governing rights and limitations under the License.

  The Original Code is the Open Hardware Monitor code.

  The Initial Developer of the Original Code is 
  Michael Möller <m.moeller@gmx.ch>.
  Portions created by the Initial Developer are Copyright (C) 2009-2010
  the Initial Developer. All Rights Reserved.

  Contributor(s): Paul Werelds

  Alternatively, the contents of this file may be used under the terms of
  either the GNU General Public License Version 2 or later (the "GPL"), or
  the GNU Lesser General Public License Version 2.1 or later (the "LGPL"),
  in which case the provisions of the GPL or the LGPL are applicable instead
  of those above. If you wish to allow use of your version of this file only
  under the terms of either the GPL or the LGPL, and not to allow others to
  use your version of this file under the terms of the MPL, indicate your
  decision by deleting the provisions above and replace them with the notice
  and other provisions required by the GPL or the LGPL. If you do not delete
  the provisions above, a recipient may use your version of this file under
  the terms of any one of the MPL, the GPL or the LGPL.
 
*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace OpenHardwareMonitor.Hardware.HDD {
    public class HDDGroup : IGroup
    {

    private const int MAX_DRIVES = 32;

    private readonly List<HDD> hardware = new List<HDD>();

    public HDDGroup(ISettings settings) {
      int p = (int)Environment.OSVersion.Platform;
      if (p == 4 || p == 128) return;

      for (int drive = 0; drive < MAX_DRIVES; drive++) {
        IntPtr handle = SMART.OpenPhysicalDrive(drive);

        if (handle == SMART.INVALID_HANDLE_VALUE)
          continue;

        if (!SMART.EnableSmart(handle, drive)) {
          SMART.CloseHandle(handle);
          continue;
        }

        string name = SMART.ReadName(handle, drive);
        if (name == null) {
          SMART.CloseHandle(handle);
          continue;
        }

        List<SMART.DriveAttribute> attributes =
          new List<SMART.DriveAttribute>(SMART.ReadSmart(handle, drive));
        
        if (!(attributes.Count > 0)) {
          SMART.CloseHandle(handle);
          continue;
        }

        SMART.SSDLifeID ssdLifeID = GetSSDLifeID(attributes);
        if (ssdLifeID == SMART.SSDLifeID.None) {
          SMART.AttributeID temperatureID = GetTemperatureIndex(attributes);

          if (temperatureID != 0x00) {
            hardware.Add(new HDD(name, handle, drive, temperatureID, settings));
            continue;
          }
        } else {
          hardware.Add(new HDD(name, handle, drive, ssdLifeID, settings));
          continue;
        }
        
        SMART.CloseHandle(handle);
      }
    }

    private SMART.SSDLifeID GetSSDLifeID(List<SMART.DriveAttribute> attributes) {
      // ID E9 is present on Intel, JM, SF and Samsung
      // ID D2 is present on Indilinx
      // Neither ID has been found on a mechanical hard drive (yet),
      // So this seems like a good way to check if it's an SSD.
      bool isKnownSSD = (attributes.Exists(attr => (int)attr.ID == 0xE9) ||
              attributes.Exists(attr => (int)attr.ID == 0xD2)
      );

      if (!isKnownSSD) return SMART.SSDLifeID.None;

      // We start with a traditional loop, because there are 4 unique ID's
      // that potentially identify one of the vendors
      for (int i = 0; i < attributes.Count; i++) {

        switch ((int)attributes[i].ID) {
          case 0xB4:
            return SMART.SSDLifeID.Samsung;
          case 0xAB:
            return SMART.SSDLifeID.SandForce;
          case 0xD2:
            return SMART.SSDLifeID.Indilinx;
        }
      }

      // TODO: Find out JMicron's Life attribute ID; their unique ID = 0xE4

      // For Intel, we make sure we have their 3 most important ID's
      // We do a traditional loop again, because we all we need to know
      // is whether we can find all 3; pointless to use Exists()
      int intelRegisterCount = 0;
      foreach (SMART.DriveAttribute attribute in attributes) {
        if ((int)attribute.ID == 0xE1 ||
          (int)attribute.ID == 0xE8 ||
          (int)attribute.ID == 0xE9
        )
          intelRegisterCount++;
      }

      return (intelRegisterCount == 3)
        ? SMART.SSDLifeID.Intel
        : SMART.SSDLifeID.None;
    }

    private SMART.AttributeID GetTemperatureIndex(
      List<SMART.DriveAttribute> attributes)
    {
      SMART.AttributeID[] validIds = new[] {
        SMART.AttributeID.Temperature,
        SMART.AttributeID.DriveTemperature,
        SMART.AttributeID.AirflowTemperature
      };

      foreach (SMART.AttributeID validId in validIds) {
        SMART.AttributeID id = validId;
        if (attributes.Exists(attr => attr.ID == id))
          return validId;
      }

      return 0x00;
    }

    public IHardware[] Hardware {
      get {
        return hardware.ToArray();
      }
    }

    public string GetReport() {
      int p = (int)Environment.OSVersion.Platform;
      if (p == 4 || p == 128) return null;

      StringBuilder r = new StringBuilder();

      r.AppendLine("S.M.A.R.T Data");
      r.AppendLine();

      for (int drive = 0; drive < MAX_DRIVES; drive++) {
        IntPtr handle = SMART.OpenPhysicalDrive(drive);

        if (handle == SMART.INVALID_HANDLE_VALUE)
          continue;

        if (!SMART.EnableSmart(handle, drive)) {
          SMART.CloseHandle(handle);
          continue;
        }

        string name = SMART.ReadName(handle, drive);
        if (name == null) {
          SMART.CloseHandle(handle);
          continue;
        }

        List<SMART.DriveAttribute> attributes = SMART.ReadSmart(handle, drive);

        if (attributes != null) {
          r.AppendLine("Drive name: " + name);
          r.AppendLine();
          r.AppendFormat(CultureInfo.InvariantCulture, " {0}{1}{2}{3}{4}{5}",
            ("ID").PadRight(6),
            ("RawValue").PadRight(20),
            ("WorstValue").PadRight(12),
            ("AttrValue").PadRight(12),
            ("Name"),
            Environment.NewLine);

          foreach (SMART.DriveAttribute a in attributes) {
            if (a.ID == 0) continue;
            string raw = BitConverter.ToString(a.RawValue);
            r.AppendFormat(CultureInfo.InvariantCulture, " {0}{1}{2}{3}{4}{5}",
              a.ID.ToString("d").PadRight(6), 
              raw.Replace("-", " ").PadRight(20),
              a.WorstValue.ToString(CultureInfo.InvariantCulture).PadRight(12),
              a.AttrValue.ToString(CultureInfo.InvariantCulture).PadRight(12),
              a.ID,
              Environment.NewLine);
          }
          r.AppendLine();
        }

        SMART.CloseHandle(handle);
      }

      return r.ToString();
    }

    public void Close() {
      foreach (HDD hdd in hardware) 
        hdd.Close();
    }
  }
}
