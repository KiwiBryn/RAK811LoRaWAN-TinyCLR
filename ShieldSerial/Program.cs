﻿//---------------------------------------------------------------------------------
// Copyright (c) July 2020, devMobile Software
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
//---------------------------------------------------------------------------------
#define TINYCLR_V2_FEZDUINO 
//#define SERIAL_SYNC_READ
namespace devMobile.IoT.Rak811.ShieldSerial
{
   using System;
   using System.Diagnostics;

   using GHIElectronics.TinyCLR.Devices.Uart;
   using GHIElectronics.TinyCLR.Pins;

   using System.Collections;
   using System.Text;
   using System.Threading;

   public class Program
   {
      private static UartController serialDevice;
      private const string ATCommand = "at+version\r\n";
#if TINYCLR_V2_FEZDUINO
      private static string SerialPortId = SC20100.UartPort.Uart5;
#endif

      public static void Main()
      {
         Debug.WriteLine("devMobile.IoT.Rak811.ShieldSerial starting");

         try
         {
            serialDevice = UartController.FromName(SerialPortId);

            serialDevice.SetActiveSettings(new UartSetting()
            {
               BaudRate = 9600,
               Parity = UartParity.None,
               StopBits = UartStopBitCount.One,
               Handshaking = UartHandshake.None,
               DataBits = 8
            });

            serialDevice.Enable();

#if !SERIAL_SYNC_READ
            serialDevice.DataReceived += SerialDevice_DataReceived;
#endif

            while (true)
            {
               byte[] txBuffer = UTF8Encoding.UTF8.GetBytes(ATCommand);

               int txByteCount = serialDevice.Write(txBuffer);
               Debug.WriteLine($"TX: {txByteCount} bytes");

#if SERIAL_SYNC_READ
               while( serialDevice.BytesToWrite>0)
               {
                  Debug.WriteLine($" BytesToWrite {serialDevice.BytesToWrite}");
                  Thread.Sleep(100);
               }

               int rxByteCount = serialDevice.BytesToRead;
               if (rxByteCount>0)
               {
                  byte[] rxBuffer = new byte[rxByteCount];

                  serialDevice.Read(rxBuffer);

                  Debug.WriteLine($"RX sync:{rxByteCount} bytes read");
                  String response = UTF8Encoding.UTF8.GetString(rxBuffer);
                  Debug.WriteLine($"RX sync:{response}");
               }
#endif

               Thread.Sleep(20000);
            }
         }
         catch (Exception ex)
         {
            Debug.WriteLine(ex.Message);
         }
      }


#if !SERIAL_SYNC_READ
      private static void SerialDevice_DataReceived(UartController sender, DataReceivedEventArgs e)
      {
         byte[] rxBuffer = new byte[e.Count];

         serialDevice.Read(rxBuffer, 0, e.Count);

         Debug.WriteLine($"RX Async:{e.Count} bytes read");
         String response = UTF8Encoding.UTF8.GetString(rxBuffer);
         Debug.WriteLine($"RX Async:{response}");
      }
#endif
   }
}

