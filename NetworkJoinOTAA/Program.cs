//---------------------------------------------------------------------------------
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
namespace devMobile.IoT.Rak811.NetworkJoinOTAA
{
   using System;
   using System.Diagnostics;
   using System.Text;
   using System.Threading;

   using GHIElectronics.TinyCLR.Devices.Uart;
   using GHIElectronics.TinyCLR.Pins;

   public class Program
   {
#if TINYCLR_V2_FEZDUINO
      private static string SerialPortId = SC20100.UartPort.Uart5;
#endif
      private const string DevEui = "...";
      private const string AppEui = "...";
      private const string AppKey = "...";
      private const byte messagePort = 1;
      private const string payload = "48656c6c6f204c6f526157414e"; // Hello LoRaWAN

      public static void Main()
      {
         UartController serialDevice;
         int txByteCount;
         int rxByteCount;

         Debug.WriteLine("devMobile.IoT.Rak811.NetworkJoinOTAA starting");

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

            // clear out the RX buffer
            rxByteCount = serialDevice.BytesToRead;
            while (rxByteCount > 0)
            {
               byte[] rxBuffer0 = new byte[rxByteCount];

               serialDevice.Read(rxBuffer0);
               if (rxByteCount > 0)
               {
                  serialDevice.Read(rxBuffer0);

                  Debug.WriteLine($"RX0 :{rxByteCount} bytes read");
                  String response = UTF8Encoding.UTF8.GetString(rxBuffer0);
                  Debug.WriteLine($"RX0 :{response}");
               }
            }

            // Set the Working mode to LoRaWAN
            txByteCount = serialDevice.Write(UTF8Encoding.UTF8.GetBytes("at+set_config=lora:work_mode:0\r\n"));
            Debug.WriteLine($"TX: work mode {txByteCount} bytes");
            Thread.Sleep(500);

            // Read the response
            rxByteCount = serialDevice.BytesToRead;
            if (rxByteCount > 0)
            {
               byte[] rxBuffer = new byte[rxByteCount];
               serialDevice.Read(rxBuffer);
               Debug.WriteLine($"RX :{UTF8Encoding.UTF8.GetString(rxBuffer)}");
            }

            // Set the Region to AS923
            txByteCount = serialDevice.Write(UTF8Encoding.UTF8.GetBytes("at+set_config=lora:region:AS923\r\n"));
            Debug.WriteLine($"TX: region {txByteCount} bytes");
            Thread.Sleep(500);

            // Read the response
            rxByteCount = serialDevice.BytesToRead;
            if (rxByteCount > 0)
            {
               byte[] rxBuffer = new byte[rxByteCount];
               serialDevice.Read(rxBuffer);
               Debug.WriteLine($"RX :{UTF8Encoding.UTF8.GetString(rxBuffer)}");
            }

            // Set the JoinMode
            txByteCount = serialDevice.Write(UTF8Encoding.UTF8.GetBytes("at+set_config=lora:join_mode:0\r\n"));
            Debug.WriteLine($"TX: join_mode {txByteCount} bytes");
            Thread.Sleep(500);

            // Read the response
            rxByteCount = serialDevice.BytesToRead;
            if (rxByteCount > 0)
            {
               byte[] rxBuffer = new byte[rxByteCount];
               serialDevice.Read(rxBuffer);
               Debug.WriteLine($"RX :{UTF8Encoding.UTF8.GetString(rxBuffer)}");
            }

            // OTAA set the devEUI
            txByteCount = serialDevice.Write(UTF8Encoding.UTF8.GetBytes($"at+set_config=lora:dev_eui:{DevEui}\r\n"));
            Debug.WriteLine($"TX: dev_eui: {txByteCount} bytes");
            Thread.Sleep(500);

            // Read the response
            rxByteCount = serialDevice.BytesToRead;
            if (rxByteCount > 0)
            {
               byte[] rxBuffer = new byte[rxByteCount];
               serialDevice.Read(rxBuffer);
               Debug.WriteLine($"RX :{UTF8Encoding.UTF8.GetString(rxBuffer)}");
            }

            // Set the appEUI
            txByteCount = serialDevice.Write(UTF8Encoding.UTF8.GetBytes($"at+set_config=lora:app_eui:{AppEui}\r\n"));
            Debug.WriteLine($"TX: app_eui {txByteCount} bytes");
            Thread.Sleep(500);

            // Read the response
            rxByteCount = serialDevice.BytesToRead;
            if (rxByteCount > 0)
            {
               byte[] rxBuffer = new byte[rxByteCount];
               serialDevice.Read(rxBuffer);
               Debug.WriteLine($"RX :{UTF8Encoding.UTF8.GetString(rxBuffer)}");
            }

            // Set the appKey
            txByteCount = serialDevice.Write(UTF8Encoding.UTF8.GetBytes($"at+set_config=lora:app_key:{AppKey}\r\n"));
            Debug.WriteLine($"TX: app_key {txByteCount} bytes");
            Thread.Sleep(500);

            // Read the response
            rxByteCount = serialDevice.BytesToRead;
            if (rxByteCount > 0)
            {
               byte[] rxBuffer = new byte[rxByteCount];
               serialDevice.Read(rxBuffer);
               Debug.WriteLine($"RX :{UTF8Encoding.UTF8.GetString(rxBuffer)}");
            }

            // Set the Confirm flag
            txByteCount = serialDevice.Write(UTF8Encoding.UTF8.GetBytes("at+set_config=lora:confirm:0\r\n"));
            Debug.WriteLine($"TX: confirm {txByteCount} bytes");
            Thread.Sleep(500);

            // Read the response
            rxByteCount = serialDevice.BytesToRead;
            if (rxByteCount > 0)
            {
               byte[] rxBuffer = new byte[rxByteCount];
               serialDevice.Read(rxBuffer);
               Debug.WriteLine($"RX :{UTF8Encoding.UTF8.GetString(rxBuffer)}");
            }

            // Join the network
            txByteCount = serialDevice.Write(UTF8Encoding.UTF8.GetBytes("at+join\r\n"));
            Debug.WriteLine($"TX: join {txByteCount} bytes");
            Thread.Sleep(10000);

            // Read the response
            rxByteCount = serialDevice.BytesToRead;
            if (rxByteCount > 0)
            {
               byte[] rxBuffer = new byte[rxByteCount];
               serialDevice.Read(rxBuffer);
               Debug.WriteLine($"RX :{UTF8Encoding.UTF8.GetString(rxBuffer)}");
            }

            while (true)
            {
               txByteCount = serialDevice.Write(UTF8Encoding.UTF8.GetBytes($"at+send=lora:{messagePort}:{payload}\r\n"));
               Debug.WriteLine($"TX: send {txByteCount} bytes");
               Thread.Sleep(5000);

               // Read the response
               rxByteCount = serialDevice.BytesToRead;
               if (rxByteCount > 0)
               {
                  byte[] rxBuffer = new byte[rxByteCount];
                  serialDevice.Read(rxBuffer);
                  Debug.WriteLine($"RX :{UTF8Encoding.UTF8.GetString(rxBuffer)}");
               }

               Thread.Sleep(20000);
            }
         }
         catch (Exception ex)
         {
            Debug.WriteLine(ex.Message);
         }
      }
   }
}
