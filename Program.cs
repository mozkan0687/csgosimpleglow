using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace CSGO_Simple_Glow
{
    class Program
    {

        public static string process = "csgo";

        public static int bClient;
        static void Main(string[] args)
        {
            VAMemory vam = new VAMemory(process);

            if (getModule())
            {
                Console.WriteLine("CLIENT :" + bClient.ToString());
                Console.WriteLine("Working");
                
                while (true)
                {
                    dvr En = new dvr()
                    {
                        r = 0.2f,
                        g = 0.4f,
                        b = 0,
                        a = 0.8f,
                        dwo = true,
                        dwou = false
                    };



                    dvr mt = new dvr()
                    {
                        r = 0,
                        g = 0.2f,
                        b = 0.4f,
                        a = 1f,
                        dwo = true,
                        dwou = false

                    };

                    int adress;
                    int i = 0;
                    
                    do
                    {


                        int pl = vam.ReadInt32((IntPtr)bClient + Offsets.lp);
                        int mtm = vam.ReadInt32((IntPtr)pl + Offsets.myt);

                        int ettl = vam.ReadInt32((IntPtr)bClient + Offsets.entt + (i * 0x10));
                        int pt = vam.ReadInt32((IntPtr)ettl + Offsets.myt);

                        
                        int life = vam.ReadInt32((IntPtr)ettl + Offsets.hlt);


                        adress = ettl + Offsets.drmnt;
                        if (!vam.ReadBoolean((IntPtr)adress))
                        {

                                int gind = vam.ReadInt32((IntPtr)ettl + Offsets.gin);


                            
                            if (pt != mtm)
                            {
                            

                                if (life > 79)
                                {
                                    En.r = 0;
                                    En.g = 1;
                                    En.b = 0;
                                }
                                if (life < 80 && life > 20)
                                {
                                    En.r = 1f;
                                    En.g = 0.5f;
                                    En.b = 0;
                                }
                                if (life < 21)
                                {
                                    En.r = 1;
                                    En.g = 0;
                                    En.b = 0;

                                }
                                int gobj = vam.ReadInt32((IntPtr)bClient + Offsets.gob);

                                int calculation = (gind * 0x38) + 0x8;
                                int current = gobj + calculation;
                                vam.WriteFloat((IntPtr)current, En.r);

                                calculation = (gind * 0x38) + 0xC;
                                current = gobj + calculation;
                                vam.WriteFloat((IntPtr)current, En.g);

                                calculation = (gind * 0x38) + 0x10;
                                current = gobj + calculation;
                                vam.WriteFloat((IntPtr)current, En.b);

                                calculation = (gind * 0x38) + 0x14;
                                current = gobj + calculation;
                                vam.WriteFloat((IntPtr)current, En.a);

                                calculation = (gind * 0x38) + 0x28;
                                current = gobj + calculation;
                                vam.WriteBoolean((IntPtr)current, En.dwo);

                                calculation = (gind * 0x38) + 0x29;
                                current = gobj + calculation;
                                vam.WriteBoolean((IntPtr)current, En.dwou);


                            }
                            else if (pt == mtm)
                            {
                                
                                adress = bClient + Offsets.gob;
                                int gobj = vam.ReadInt32((IntPtr)adress);
                                int calculation;
                                int current;
                                calculation = (gind * 0x38) + 0x8;
                                current = gobj + calculation;
                                vam.WriteFloat((IntPtr)current, mt.r);

                                calculation = (gind * 0x38) + 0xC;
                                current = gobj + calculation;
                                vam.WriteFloat((IntPtr)current, mt.g);

                                calculation = (gind * 0x38) + 0x10;
                                current = gobj + calculation;
                                vam.WriteFloat((IntPtr)current, mt.b);

                                calculation = (gind * 0x38) + 0x14;
                                current = gobj + calculation;
                                vam.WriteFloat((IntPtr)current, mt.a);

                                calculation = (gind * 0x38) + 0x28;
                                current = gobj + calculation;
                                vam.WriteBoolean((IntPtr)current, mt.dwo);

                                calculation = (gind * 0x38) + 0x29;
                                current = gobj + calculation;
                                vam.WriteBoolean((IntPtr)current, mt.dwou);
                            }

                        }


                        
                        i++;
                    }
                    while (i < 21);

                    Thread.Sleep(10);


                }

            }

            Console.ReadKey();

        }

        public class Offsets
        {
            public static int lp = 0xDEA964; //dwLocalPlayer
            public static int myt = 0xF4;  //m_iTeamNum
            public static int entt = 0x4DFFF14; //dwEntityList
            public static int drmnt = 0xED;  //m_bDormant
            public static int gin = 0x10488;  //m_iGlowIndex
            public static int gob = 0x535A9D8;  //dwGlowObjectManager
            public static int hlt = 0x100;  //m_iHealth
            public static int pli = 0x180; //dwClientState_GetLocalPlayer

        }
        public struct dvr
        {
            public float r;
            public float g;
            public float b;
            public float a;
            public bool dwo;
            public bool dwou;
        }
        #region GETMODULE
        static bool getModule()
        {
            try
            {
                Process[] p = Process.GetProcessesByName(process);
                if (p.Length > 0)
                {

                    foreach (ProcessModule m in p[0].Modules)
                    {

                        if (m.ModuleName == "client.dll")
                        {

                            bClient = (int)m.BaseAddress;
                            return true;
                        }
                    }
                    return true;

                }
                else
                {

                    Console.WriteLine("Run CSGO first");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
                throw;
            }
        }
    }
}
#endregion